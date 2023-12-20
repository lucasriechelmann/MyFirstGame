namespace PipelineExtensions;

public class LevelEvent
{
    public class Nothing : LevelEvent { }
    public class GenerateEnemies : LevelEvent
    {
        public GenerateEnemies(int nbEnemies)
        {
            NbEnemies = nbEnemies;
        }
        public int NbEnemies { get; private set; }
    }
    public class GenerateTurret : LevelEvent
    {
        public GenerateTurret(float xPosition)
        {
            XPosition = xPosition;
        }
        public float XPosition { get; set; }
    }
    public class StartLevel : LevelEvent { }
    public class EndLevel : LevelEvent { }
    public class NoRowEvent : LevelEvent { }
}
