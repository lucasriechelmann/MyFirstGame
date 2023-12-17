using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Engine.Input;
using MyFirstGame.Engine.Objects;
using MyFirstGame.Engine.Sound;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstGame.Engine.States;
public abstract class BaseGameState
{
    protected bool _debug = false;
    protected bool _indestructible = false;

    private ContentManager _contentManager;
    protected int _viewPortHeight;
    protected int _viewPortWidth;
    protected SoundManager _soundManager = new SoundManager();

    private readonly List<BaseGameObject> _gameObjects = new List<BaseGameObject>();

    protected InputManager InputManager { get; set; }

    private const string StatsFont = "font/Stats";
    private StatsObject _statsText;

    public void Initialize(ContentManager contentManager, int viewPortWidth, int viewPortHeight)
    {
        _contentManager = contentManager;
        _viewPortHeight = viewPortHeight;
        _viewPortWidth = viewPortWidth;

        SetInputManager();
    }

    public virtual void LoadContent()
    {
        if(_debug)
        {
            _statsText = new StatsObject(LoadFont(StatsFont));
            _statsText.Position = new Vector2(10, 10);
            AddGameObject(_statsText);
        }        
    }
    public abstract void HandleInput(GameTime gameTime);
    public abstract void UpdateGameState(GameTime gameTime);

    public event EventHandler<BaseGameState> OnStateSwitched;
    public event EventHandler<BaseGameStateEvent> OnEventNotification;
    protected abstract void SetInputManager();

    public void UnloadContent() => _contentManager.Unload();

    public void Update(GameTime gameTime)
    {
        UpdateGameState(gameTime);

        if (_debug)
            _statsText.Update(gameTime);

        _soundManager.PlaySoundtrack();
    }

    protected Texture2D LoadTexture(string textureName) => _contentManager.Load<Texture2D>(textureName);
    protected SpriteFont LoadFont(string fontName) => _contentManager.Load<SpriteFont>(fontName);
    protected SoundEffect LoadSound(string soundName) => _contentManager.Load<SoundEffect>(soundName);
    protected void NotifyEvent(BaseGameStateEvent gameEvent)
    {
        OnEventNotification?.Invoke(this, gameEvent);

        foreach (var gameObject in _gameObjects)
        {
            if (gameObject != null)
                gameObject.OnNotify(gameEvent);
        }

        _soundManager.OnNotify(gameEvent);
    }
    protected void SwitchState(BaseGameState gameState) => OnStateSwitched?.Invoke(this, gameState);
    protected void AddGameObject(BaseGameObject gameObject) => _gameObjects.Add(gameObject);
    protected void RemoveGameObject(BaseGameObject gameObject) => _gameObjects.Remove(gameObject);

    public virtual void Render(SpriteBatch spriteBatch)
    {
        foreach (var gameObject in _gameObjects.Where(a => a != null).OrderBy(a => a.zIndex))
        {
            if (_debug)
            {
                gameObject.RenderBoundingBoxes(spriteBatch);
            }

            gameObject.Render(spriteBatch);
        }
    }
}
