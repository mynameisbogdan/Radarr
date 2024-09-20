using System;
using System.IO;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using NzbDrone.Common.Disk;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.Configuration;
using NzbDrone.Core.MediaFiles;
using NzbDrone.Core.Movies;
using NzbDrone.Core.Test.Framework;
using NzbDrone.Test.Common;

namespace NzbDrone.Core.Test.MediaFiles.UpdateMovieFileServiceTests
{
    [TestFixture]
    public class ChangeFileDateForFileFixture : CoreTest<UpdateMovieFileService>
    {
        private readonly DateTime _veryOldAirDateUtc = new(1965, 01, 01, 0, 0, 0, 512, 512, DateTimeKind.Utc);
        private DateTime _lastWrite = new(2025, 07, 27, 0, 0, 0, 512, 512, DateTimeKind.Utc);
        private Movie _movie;
        private MovieFile _movieFile;
        private string _movieFolder;

        [SetUp]
        public void Setup()
        {
            _movieFolder = @"C:\Test\Movies\Movie Title 2026".AsOsAgnostic();

            _movie = Builder<Movie>.CreateNew()
                                     .With(s => s.Path = _movieFolder)
                                     .With(s => s.MovieMetadata.Value.DigitalRelease = _lastWrite.AddDays(2))
                                     .Build();

            _movieFile = Builder<MovieFile>.CreateNew()
                                               .With(f => f.Path = Path.Combine(_movie.Path, "Movie Title 2026.mkv").AsOsAgnostic())
                                               .With(f => f.RelativePath = @"Movie Title 2026.mkv".AsOsAgnostic())
                                               .Build();

            Mocker.GetMock<IDiskProvider>()
                .Setup(x => x.FileGetLastWrite(_movieFile.Path))
                .Returns(() => _lastWrite);

            Mocker.GetMock<IDiskProvider>()
                .Setup(x => x.FileSetLastWriteTime(_movieFile.Path, It.IsAny<DateTime>()))
                .Callback<string, DateTime>((path, dateTime) =>
                {
                    _lastWrite = dateTime.Kind == DateTimeKind.Utc
                        ? dateTime
                        : dateTime.ToUniversalTime();
                });

            Mocker.GetMock<IConfigService>()
                .Setup(x => x.FileDate)
                .Returns(FileDateType.Release);
        }

        [Test]
        public void should_change_date_once_only()
        {
            var previousWrite = new DateTime(_lastWrite.Ticks, _lastWrite.Kind);

            Subject.ChangeFileDateForFile(_movieFile, _movie);
            Subject.ChangeFileDateForFile(_movieFile, _movie);

            Mocker.GetMock<IDiskProvider>()
                .Verify(v => v.FileSetLastWriteTime(_movieFile.Path, It.IsAny<DateTime>()), Times.Once());

            var actualWriteTime = Mocker.GetMock<IDiskProvider>().Object.FileGetLastWrite(_movieFile.Path).ToLocalTime();
            actualWriteTime.Should().Be(_movie.MovieMetadata.Value.DigitalRelease.Value.ToLocalTime().WithTicksFrom(previousWrite));
        }

        [Test]
        public void should_clamp_mtime_on_posix()
        {
            PosixOnly();

            var previousWrite = new DateTime(_lastWrite.Ticks, _lastWrite.Kind);
            _movie.MovieMetadata.Value.DigitalRelease = _veryOldAirDateUtc;

            Subject.ChangeFileDateForFile(_movieFile, _movie);
            Subject.ChangeFileDateForFile(_movieFile, _movie);

            Mocker.GetMock<IDiskProvider>()
                .Verify(v => v.FileSetLastWriteTime(_movieFile.Path, It.IsAny<DateTime>()), Times.Once());

            var actualWriteTime = Mocker.GetMock<IDiskProvider>().Object.FileGetLastWrite(_movieFile.Path).ToLocalTime();
            actualWriteTime.Should().Be(DateTimeExtensions.EpochTime.ToLocalTime().WithTicksFrom(previousWrite));
        }

        [Test]
        public void should_not_clamp_mtime_on_windows()
        {
            WindowsOnly();

            var previousWrite = new DateTime(_lastWrite.Ticks, _lastWrite.Kind);
            _movie.MovieMetadata.Value.DigitalRelease = _veryOldAirDateUtc;

            Subject.ChangeFileDateForFile(_movieFile, _movie);
            Subject.ChangeFileDateForFile(_movieFile, _movie);

            Mocker.GetMock<IDiskProvider>()
                .Verify(v => v.FileSetLastWriteTime(_movieFile.Path, It.IsAny<DateTime>()), Times.Once());

            var actualWriteTime = Mocker.GetMock<IDiskProvider>().Object.FileGetLastWrite(_movieFile.Path).ToLocalTime();
            actualWriteTime.Should().Be(_movie.MovieMetadata.Value.DigitalRelease.Value.ToLocalTime().WithTicksFrom(previousWrite));
        }
    }
}
