using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MyFirstGame.Engine.Input;

public class BaseInputMapper
{
    public virtual IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state) => new List<BaseInputCommand>();
    public virtual IEnumerable<BaseInputCommand> GetMouseState(MouseState state) => new List<BaseInputCommand>();
    public virtual IEnumerable<BaseInputCommand> GetGamePadState(GamePadState state) => new List<BaseInputCommand>();
}
