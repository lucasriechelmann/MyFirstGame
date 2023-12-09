using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Enums;
using MyFirstGame.Objects.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstGame.States.Base;
public abstract class BaseGameState
{
    private readonly List<BaseGameObject> _gameObjects = new List<BaseGameObject>();
    public abstract void LoadContent(ContentManager contentManager);
    public abstract void UnloadContent(ContentManager contentManager);
    public abstract void HandleInput();
    public event EventHandler<BaseGameState> OnStateSwitched;
    public event EventHandler<Events> OnEventNotification;
    protected void NotifyEvent(Events eventType, object argument = null)
    {
        OnEventNotification?.Invoke(this, eventType);

        foreach(var gameObject in _gameObjects)
        {
            gameObject.OnNotify(eventType);
        }
    }
    protected void SwitchState(BaseGameState state) => OnStateSwitched?.Invoke(this, state);
    protected void AddGameObject(BaseGameObject gameObject) => _gameObjects.Add(gameObject);
    public void Render(SpriteBatch spriteBatch)
    {
        foreach(var gameObject in _gameObjects.OrderBy(x => x.zIndex))
        {
            gameObject.Render(spriteBatch);
        }
    }
}
