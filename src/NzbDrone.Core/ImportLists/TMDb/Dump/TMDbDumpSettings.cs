using NzbDrone.Core.Annotations;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.ImportLists.TMDb.Dump;

public class TMDbDumpSettings : TMDbSettingsBase<TMDbDumpSettings>
{
    private static readonly TMDbSettingsBaseValidator<TMDbDumpSettings> Validator = new();

    [FieldDefinition(1, Label = "Limit")]
    public int Limit { get; set; } = 100;

    public override NzbDroneValidationResult Validate()
    {
        return new NzbDroneValidationResult(Validator.Validate(this));
    }
}
