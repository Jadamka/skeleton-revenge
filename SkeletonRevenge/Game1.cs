using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SkeletonRevenge;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private readonly int _screenWidth = 1280;
    private readonly int _screenHeight = 720;
    private readonly int _bufferWidth = 800;
    private readonly int _bufferHeight = 600;

    private Player _player;
    private Level _level;
    private Renderer _renderer;
    private TextureManager _textureManager;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = _screenWidth;
        _graphics.PreferredBackBufferHeight = _screenHeight;
        
        Content.RootDirectory = "Content";
        
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        this.Window.Title = "Skeleton Revenge";

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        SpriteFont font1 = Content.Load<SpriteFont>("fonts/ArialFont");
        DebugOverlay.SetFont(font1);
        
        _renderer = new Renderer(GraphicsDevice, _bufferWidth, _bufferHeight);
        
        _textureManager = new TextureManager();
        _textureManager.LoadTextures(Content);
        
        _player = new Player(new Vector2(12, 22), new Vector2(-1, 0));
        _level = new Level();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.F12))
        {
            DebugOverlay.SetIsVisible();
        }
        
        _player.Update(gameTime, _level);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        _renderer.Render3D(_spriteBatch, _textureManager, _player, _level);
        DebugOverlay.Draw(_spriteBatch, gameTime, _player);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}