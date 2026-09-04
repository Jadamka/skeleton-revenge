using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkeletonRevenge.Entities;
using SkeletonRevenge.Entities.Weapons;
using SkeletonRevenge.Graphics;
using SkeletonRevenge.UI;
using SkeletonRevenge.World;

namespace SkeletonRevenge.Core;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private KeyboardState _oldState; // For key being pressed not held

    private readonly int _screenWidth = 1280;
    private readonly int _screenHeight = 720;
    private readonly int _bufferWidth = 640;
    private readonly int _bufferHeight = 360;

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
        _oldState = Keyboard.GetState();

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
        _level = LevelLoader.LoadLevelXML(Content, _textureManager,"levels/level01.xml");
        
        Texture2D shotgunSpritesheet = Content.Load<Texture2D>("spritesheets/shotgun_spritesheet");
        Weapon shotgun = new Weapon(shotgunSpritesheet, 5, 2.5f);
        _player.EquipWeapon(shotgun);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardState newState = Keyboard.GetState();
        if (newState.IsKeyDown(Keys.F1))
        {
            if (!_oldState.IsKeyDown(Keys.F1))
            {
                DebugOverlay.SetIsVisible();
            }
        }
        
        _player.Update(gameTime, _level);

        _oldState = newState;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        _renderer.Render3D(_spriteBatch, _textureManager, _player, _level);
        
        _player.Draw(_spriteBatch, GraphicsDevice);
        
        DebugOverlay.Draw(_spriteBatch, gameTime, _player);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}