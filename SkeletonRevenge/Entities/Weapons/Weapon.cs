using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkeletonRevenge.Entities.Weapons;

public class Weapon
{
    private Texture2D _spriteSheet;
    private int _frameWidth;
    private int _frameHeight;
    private int _currentFrame;

    public Weapon(Texture2D spriteSheet, int frames)
    {
        _spriteSheet = spriteSheet;
        _frameWidth = spriteSheet.Width / frames;
        _frameHeight = spriteSheet.Height;
        _currentFrame = 0;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 screenPosition)
    {
        Rectangle sourceRectangle = new Rectangle(_currentFrame * _frameWidth, 0,  _frameWidth, _frameHeight);
        spriteBatch.Draw(_spriteSheet, screenPosition, sourceRectangle, Color.White);
    }
}