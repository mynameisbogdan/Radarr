using NzbDrone.Core.Datastore;

namespace NzbDrone.Core.Movies
{
    public class MonitoringOptions : IEmbeddedDocument
    {
        public MonitorTypes Monitor { get; set; }
    }

    public enum MonitorTypes
    {
        MovieOnly,
        MovieAndCollection,
        None
    }
}
