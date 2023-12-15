using MyFirstGame.Engine.States;

namespace MyFirstGame.Levels;

public class LevelEvents : BaseGameStateEvent
{
    public class GenerateEnemies : LevelEvents
    {
        public int NbEnemies { get; private set; }
        public GenerateEnemies(int nbEnemies)
        {
            NbEnemies = nbEnemies;
        }
    }

    public class GenerateTurret : LevelEvents
    {
        public float XPosition { get; private set; }
        public GenerateTurret(float xPosition)
        {
            XPosition = xPosition;
        }
    }

    public class StartLevel : LevelEvents { }

    public class EndLevel : LevelEvents { }

    public class NoRowEvent : LevelEvents { }
}
