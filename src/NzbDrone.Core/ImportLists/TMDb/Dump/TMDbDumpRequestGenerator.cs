using System;
using System.Collections.Generic;
using NzbDrone.Common.Http;

namespace NzbDrone.Core.ImportLists.TMDb.Dump;

public class TMDbDumpRequestGenerator : IImportListRequestGenerator
{
    public virtual ImportListPageableRequestChain GetMovies()
    {
        var pageableRequests = new ImportListPageableRequestChain();

        pageableRequests.Add(GetMoviesRequest());

        return pageableRequests;
    }

    private static IEnumerable<ImportListRequest> GetMoviesRequest()
    {
        yield return new ImportListRequest($"https://files.tmdb.org/p/exports/movie_ids_{DateTime.UtcNow:MM_dd_yyyy}.json.gz", HttpAccept.Json);
    }
}
