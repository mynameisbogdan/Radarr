using NLog;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.Parser.Model;

namespace NzbDrone.Core.DecisionEngine.Specifications;

public class ExternalIdsSpecification : IDownloadDecisionEngineSpecification
{
    private readonly Logger _logger;

    public SpecificationPriority Priority => SpecificationPriority.Database;
    public RejectionType Type => RejectionType.Permanent;

    public ExternalIdsSpecification(Logger logger)
    {
        _logger = logger;
    }

    public DownloadSpecDecision IsSatisfiedBy(RemoteMovie subject, ReleaseDecisionInformation information)
    {
        if (subject.Release.TmdbId != 0 && subject.Release.TmdbId != subject.Movie.TmdbId)
        {
            _logger.Debug("Wrong movie. TMDb Id {0} wanted, but found {1}.", subject.Movie.TmdbId, subject.Release.TmdbId);
            return DownloadSpecDecision.Reject(DownloadRejectionReason.WrongMovie, "Wrong movie. TMDb Id {0} wanted, but found {1}.", subject.Movie.TmdbId, subject.Release.TmdbId);
        }

        var releaseImdbId = Parser.Parser.NormalizeImdbId(subject.Release.ImdbId.ToString());

        if (releaseImdbId.IsNotNullOrWhiteSpace() && releaseImdbId != "0" && releaseImdbId != subject.Movie.ImdbId)
        {
            _logger.Debug("Wrong movie. IMDb ID {0} wanted, but found {1}.", subject.Movie.ImdbId, releaseImdbId);
            return DownloadSpecDecision.Reject(DownloadRejectionReason.WrongMovie, "Wrong movie. IMDb ID {0} wanted, but found {1}.", subject.Movie.ImdbId, releaseImdbId);
        }

        return DownloadSpecDecision.Accept();
    }
}
