using System.Globalization;
using NLog;
using NzbDrone.Core.Parser.Model;

namespace NzbDrone.Core.DecisionEngine.Specifications;

public class ExternalIdsSpecification : IDownloadDecisionEngineSpecification
{
    private readonly Logger _logger;

    public SpecificationPriority Priority => SpecificationPriority.Default;
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

        if (subject.Release.ImdbId != 0 && Parser.Parser.TryNormalizeImdbId(subject.Release.ImdbId.ToString(CultureInfo.InvariantCulture), out var normalizedImdbId) && normalizedImdbId != subject.Movie.ImdbId)
        {
            _logger.Debug("Wrong movie. IMDb ID {0} wanted, but found {1}.", subject.Movie.ImdbId, normalizedImdbId);
            return DownloadSpecDecision.Reject(DownloadRejectionReason.WrongMovie, "Wrong movie. IMDb ID {0} wanted, but found {1}.", subject.Movie.ImdbId, normalizedImdbId);
        }

        return DownloadSpecDecision.Accept();
    }
}
