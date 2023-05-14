namespace DbwViewer.Models;

public class Area
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int PrecedentElementId { get; set; }
    public int LevelId { get; set; }
    public string? LevelName { get; set; }
    public bool IsChangeable { get; set; }
}