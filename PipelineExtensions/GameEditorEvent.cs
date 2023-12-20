using System;

namespace PipelineExtensions;

public class GameEditorEvent
{
    public int Y { get; set; }
    public static GameEditorEvent GetEvent(string typeName)
    {
        var fullyQualifiedName = $"PipelineExtensions.{typeName}";
        var eventType = Type.GetType(fullyQualifiedName);

        if (eventType is null)
            return null;

        return (GameEditorEvent)Activator.CreateInstance(eventType);
    }
}
