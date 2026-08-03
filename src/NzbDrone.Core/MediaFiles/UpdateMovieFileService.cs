using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using NzbDrone.Common.Disk;
using NzbDrone.Common.EnvironmentInfo;
using NzbDrone.Common.Extensions;
using NzbDrone.Common.Instrumentation.Extensions;
using NzbDrone.Core.Configuration;
using NzbDrone.Core.MediaFiles.Events;
using NzbDrone.Core.Messaging.Events;
using NzbDrone.Core.Movies;

namespace NzbDrone.Core.MediaFiles
{
    public interface IUpdateMovieFileService
    {
        void ChangeFileDateForFile(MovieFile movieFile, Movie movie);
    }

    public class UpdateMovieFileService : IUpdateMovieFileService,
                                            IHandle<MovieScannedEvent>
    {
        private readonly IDiskProvider _diskProvider;
        private readonly IConfigService _configService;
        private readonly IMediaFileService _mediaFileService;
        private readonly Logger _logger;

        public UpdateMovieFileService(IDiskProvider diskProvider,
                                      IConfigService configService,
                                      IMediaFileService mediaFileService,
                                      Logger logger)
        {
            _diskProvider = diskProvider;
            _configService = configService;
            _mediaFileService = mediaFileService;
            _logger = logger;
        }

        public void ChangeFileDateForFile(MovieFile movieFile, Movie movie)
        {
            ChangeFileDate(movieFile, movie);
        }

        private bool ChangeFileDate(MovieFile movieFile, Movie movie)
        {
            var movieFilePath = Path.Combine(movie.Path, movieFile.RelativePath);

            switch (_configService.FileDate)
            {
                case FileDateType.Release:
                    var releaseDate = new[] { movie.MovieMetadata.Value.DigitalRelease, movie.MovieMetadata.Value.PhysicalRelease }
                        .Where(x => x.HasValue)
                        .Min();

                    return releaseDate.HasValue && ChangeFileDateToLocalDate(movieFilePath, releaseDate.Value.ToLocalTime());

                case FileDateType.Cinemas:
                    var inCinemas = movie.MovieMetadata.Value.InCinemas;

                    return inCinemas.HasValue && ChangeFileDateToLocalDate(movieFilePath, inCinemas.Value.ToLocalTime());
            }

            return false;
        }

        private bool ChangeFileDateToLocalDate(string filePath, DateTime localDate)
        {
            // FileGetLastWrite returns UTC; convert to local to compare
            var oldLastWrite = _diskProvider.FileGetLastWrite(filePath).ToLocalTime();

            if (OsInfo.IsNotWindows && localDate.ToUniversalTime() < DateTimeExtensions.EpochTime)
            {
                _logger.Debug("Setting date of file to 1970-01-01 as actual release date is before that time and will not be set properly");
                localDate = DateTimeExtensions.EpochTime.ToLocalTime();
            }

            if (!DateTime.Equals(localDate.WithoutTicks(), oldLastWrite.WithoutTicks()))
            {
                try
                {
                    // Preserve prior mtime subseconds per https://github.com/Sonarr/Sonarr/issues/7228
                    var mtime = localDate.WithTicksFrom(oldLastWrite);

                    _diskProvider.FileSetLastWriteTime(filePath, mtime);
                    _logger.Debug("Date of file [{0}] changed from '{1}' to '{2}'", filePath, oldLastWrite, mtime);

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.Warn(ex, "Unable to set date of file [" + filePath + "]");
                }
            }

            return false;
        }

        public void Handle(MovieScannedEvent message)
        {
            if (_configService.FileDate == FileDateType.None)
            {
                return;
            }

            var movieFiles = _mediaFileService.GetFilesByMovie(message.Movie.Id);
            var updated = new List<MovieFile>();

            foreach (var movieFile in movieFiles)
            {
                if (ChangeFileDate(movieFile, message.Movie))
                {
                    updated.Add(movieFile);
                }
            }

            if (updated.Any())
            {
                _logger.ProgressDebug("Changed file date for {0} files of {1} in {2}", updated.Count, movieFiles.Count, message.Movie.Title);
            }
            else
            {
                _logger.ProgressDebug("No file dates changed for {0}", message.Movie.Title);
            }
        }
    }
}
