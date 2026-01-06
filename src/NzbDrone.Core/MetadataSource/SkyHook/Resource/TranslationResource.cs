using System.Collections.Generic;

namespace NzbDrone.Core.MetadataSource.SkyHook.Resource
{
    public class TranslationResource
    {
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Language { get; set; }
        public IReadOnlyCollection<ImageResource> Images { get; set; }
    }
}
