using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkeletonRevenge;

// This should be thread-safe, I believe...
public sealed class DebugOverlay
{
    // Singleton stuff
    private static readonly DebugOverlay _instance = new  DebugOverlay();
    static DebugOverlay()
    {
    }
    private DebugOverlay()
    {
    }
    public static DebugOverlay Instance => _instance;

    private static SpriteFont _font;
    private static bool _isFontSet = false;

    private static bool _isVisible = true;

    private static int _frameCount = 0;
    private static double _timeElapsed = 0;
    private static int _displayFps = 30;
    
    public static void SetFont(SpriteFont font)
    {
        _font = font;
        _isFontSet = true;
    }

    public static void SetIsVisible(bool isVisible)
    {
        _isVisible = isVisible;
    }

    public static void Draw(SpriteBatch spriteBatch, GameTime gameTime, Player player)
    {
        if (!_isVisible || !_isFontSet) return;

        _frameCount++;
        _timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;

        if (_timeElapsed >= 1.0)
        {
            _displayFps = _frameCount;
            _frameCount = 0;
            _timeElapsed -= 1.0;
        }

        string debugText = $"FPS: {_displayFps:F4}\n" +
                           $"X: {player.Position.X:F2}\n" +
                           $"Y: {player.Position.Y:F2}";

        spriteBatch.DrawString(_font, debugText, new Vector2(10, 10), Color.White);
    }
}