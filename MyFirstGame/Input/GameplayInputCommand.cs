﻿using MyFirstGame.Input.Base;

namespace MyFirstGame.Input;

public class GameplayInputCommand : BaseInputCommand
{
    public class GameExit : GameplayInputCommand { }
    public class PlayerMoveLeft : GameplayInputCommand { }
    public class PlayerMoveRight : GameplayInputCommand { }
    public class PlayerShoots : GameplayInputCommand { }
}
