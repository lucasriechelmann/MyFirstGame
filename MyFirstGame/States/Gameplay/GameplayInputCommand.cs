using MyFirstGame.Engine.Input;

namespace MyFirstGame.States.Gameplay;

public class GameplayInputCommand : BaseInputCommand
{
    public class GameExit : GameplayInputCommand { }
    public class PlayerMoveUp : GameplayInputCommand { }
    public class PlayerMoveDown : GameplayInputCommand { }
    public class PlayerMoveLeft : GameplayInputCommand { }
    public class PlayerMoveRight : GameplayInputCommand { }
    public class PlayerShoots : GameplayInputCommand { }
}
