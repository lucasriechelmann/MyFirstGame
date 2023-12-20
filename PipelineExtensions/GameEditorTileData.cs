namespace PipelineExtensions;

public class GameEditorTileData
{
    public string Name { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public GameEditorTileData()
    {
        
    }
    public GameEditorTileData(string name, int x, int y)
    {
        Name = name;
        X = x;
        Y = y;
    }
}