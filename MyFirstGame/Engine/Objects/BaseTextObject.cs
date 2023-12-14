using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstGame.Engine.Objects;

public class BaseTextObject : BaseGameObject
{
    protected SpriteFont _font;

    public string Text { get; set; }

    public override void Render(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(_font, Text, _position, Color.White);
    }
}
