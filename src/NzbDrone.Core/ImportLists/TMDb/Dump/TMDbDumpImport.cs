using NLog;
using NzbDrone.Common.Cloud;
using NzbDrone.Common.Http;
using NzbDrone.Core.Configuration;
using NzbDrone.Core.MetadataSource;
using NzbDrone.Core.Parser;

namespace NzbDrone.Core.ImportLists.TMDb.Dump;

public class TMDbDumpImport : TMDbImportListBase<TMDbDumpSettings>
{
    public override string Name => "TMDb Dump";
    public override bool Enabled => true;
    public override bool EnableAuto => false;
    public override int PageSize => 1;

    public TMDbDumpImport(IRadarrCloudRequestBuilder requestBuilder,
        IHttpClient httpClient,
        IImportListStatusService importListStatusService,
        IConfigService configService,
        IParsingService parsingService,
        ISearchForNewMovie searchForNewMovie,
        Logger logger)
        : base(requestBuilder, httpClient, importListStatusService, configService, parsingService, searchForNewMovie, logger)
    {
    }

    public override IParseImportListResponse GetParser()
    {
        return new TMDbDumpParser(Settings);
    }

    public override IImportListRequestGenerator GetRequestGenerator()
    {
        return new TMDbDumpRequestGenerator();
    }
}
