using Newtonsoft.Json;

namespace DbwViewer.Models;

public class Area
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("nazwa")]
    public string? Name { get; set; }
    
    [JsonProperty("id-nadrzedny-element")]
    public int PrecedentElementId { get; set; }
    
    [JsonProperty("id-poziom")]
    public int LevelId { get; set; }
    
    [JsonProperty("nazwa-poziom")]
    public string? LevelName { get; set; }
    
    [JsonProperty("czy-zmienne")]
    public bool IsChangeable { get; set; }
}