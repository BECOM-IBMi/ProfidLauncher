using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ProfidLauncher.Models;

public class OpenTabsRequestResult
{
    [JsonPropertyName("hasOpenTabs")]
    public bool HasOpenTabs { get; set; }

    [JsonPropertyName("info")]
    public IEnumerable<Info> Info { get; set; } = Enumerable.Empty<Info>();
}

public class Info
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = "";
}