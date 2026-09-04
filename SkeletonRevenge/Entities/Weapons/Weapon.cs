using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkeletonRevenge.Entities.Weapons;

public class Weapon
{
    private Texture2D _spriteSheet;
    private int _frameWidth;
    private int _frameHeight;
    private int _currentFrame;
    private float _scale;
    
    public float Width => _frameWidth * _scale;
    public float Height => _frameHeight * _scale;
    
    public Weapon(Texture2D spriteSheet, int frames, float scale = 1)
    {
        _spriteSheet = spriteSheet;
        _frameWidth = spriteSheet.Width / frames;
        _frameHeight = spriteSheet.Height;
        _currentFrame = 0;
        _scale = scale;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 screenPosition)
    {
        Rectangle sourceRectangle = new Rectangle(_currentFrame * _frameWidth, 0,  _frameWidth, _frameHeight);
        spriteBatch.Draw(
            _spriteSheet,
            screenPosition,
            sourceRectangle,
            Color.White,
            0.0f,
            Vector2.Zero,
            new Vector2(_scale, _scale),
            SpriteEffects.None,
            0
        );
    }
}