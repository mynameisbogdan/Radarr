using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.ImportLists.ImportListMovies;

namespace NzbDrone.Core.ImportLists.TMDb.Dump;

public class TMDbDumpParser(TMDbDumpSettings settings) : TMDbParser
{
    public override IList<ImportListMovie> ParseResponse(ImportListResponse importResponse)
    {
        var movies = new List<ImportListMovie>();

        if (!PreProcess(importResponse))
        {
            return movies;
        }

        using var fileStream = new MemoryStream(importResponse.HttpResponse.ResponseData);
        using var gzip = new GZipStream(fileStream, CompressionMode.Decompress);

        using var reader = new StreamReader(gzip, Encoding.UTF8);

        var count = 0;
        while (count < settings.Limit && reader.ReadLine() is { } line)
        {
            var movieDumpResource = JsonConvert.DeserializeObject<MovieDumpResource>(line);

            if ((movieDumpResource.Adult.HasValue && movieDumpResource.Adult.Value) || (movieDumpResource.Video.HasValue && movieDumpResource.Video.Value))
            {
                continue;
            }

            movies.AddIfNotNull(new ImportListMovie
            {
                TmdbId = movieDumpResource.Id,
                Title = movieDumpResource.OriginalTitle,
            });

            count++;
        }

        return movies;
    }
}

public class MovieDumpResource
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("original_title")]
    public string OriginalTitle { get; init; }

    [JsonPropertyName("adult")]
    public bool? Adult { get; init; } = false;

    [JsonPropertyName("video")]
    public bool? Video { get; init; } = false;
}
