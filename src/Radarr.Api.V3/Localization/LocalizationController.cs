using Microsoft.AspNetCore.Mvc;
using NzbDrone.Core.Localization;
using Radarr.Http;

namespace Radarr.Api.V3.Localization
{
    [V3ApiController]
    public class LocalizationController : Controller
    {
        private readonly ILocalizationService _localizationService;

        public LocalizationController(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        [HttpGet]
        [Produces("application/json")]
        public LocalizationResource GetLocalizationDictionary()
        {
            return _localizationService.GetLocalizationDictionary().ToResource();
        }

        [HttpGet("language")]
        [Produces("application/json")]
        public LocalizationLanguageResource GetLanguage()
        {
            var identifier = _localizationService.GetLanguageIdentifier();

            return new LocalizationLanguageResource
            {
                Identifier = identifier
            };
        }
    }
}
