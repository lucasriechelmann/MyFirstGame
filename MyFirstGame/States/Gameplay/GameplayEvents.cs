using MyFirstGame.Engine.States;

namespace MyFirstGame.States.Gameplay;

public class GameplayEvents : BaseGameStateEvent
{
    public class PlayerShoots : GameplayEvents { }
    public class PlayerShootsMissile : GameplayEvents { }
}
