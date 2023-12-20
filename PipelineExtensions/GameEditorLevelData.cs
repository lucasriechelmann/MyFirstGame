using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace PipelineExtensions;

public class GameEditorLevelData
{
    public string GroundGrid { get; set; }
    public List<GameEditorTileData> Buildings { get; set; }
    public List<GameEditorTileData> Objects { get; set; }
    public List<GameEditorTileData> LevelEvents { get; set; }
    public int GridWidth { get; set; }
    public int GridLength { get; set; }
    public GameEditorLevelData()
    {
        
    }
    public GameEditorLevelData(int gridWidth, int gridLength, string[,] groundGrid, List<GameEditorTileData> buildings,
        List<GameEditorTileData> objects, List<GameEditorTileData> levelEvents)
    {
        GridLength = gridLength;
        GridWidth = gridWidth;
        GroundGrid = ArrayToString(groundGrid);
        Buildings = buildings;
        Objects = objects;
        LevelEvents = levelEvents;
    }
    public string[,] StringToArray(string gridData)
    {

        var grid = new string[GridWidth, GridLength];
        var allData = gridData.Split(',');

        for(int y = 0; y < GridLength; y++)
        {
            for(int x = 0; x < GridWidth; x++)
            {
                var i = y * GridWidth + x;
                grid[x, y] = allData[i];                
            }
        }

        return grid;
    }
    public void Save(int levelNb)
    {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;

        using(XmlWriter writer = XmlWriter.Create($"level{levelNb}.xml", settings))
        {
            IntermediateSerializer.Serialize(writer, this, null);
        }
    }
    private string ArrayToString(string[,] grid)
    {
        var lines = new string[GridLength];
        for(int y = 0; y < GridLength; y++)
        {
            lines[y] = string.Join(",", GetRow(grid, y));
        }

        return string.Join(",", lines);
    }   
    public string[] GetRow(string[,] grid, int rowNumber)
    {
        return Enumerable.Range(0, grid.GetLength(0))
            .Select(x => grid[x, rowNumber])
            .ToArray();
    }
}
