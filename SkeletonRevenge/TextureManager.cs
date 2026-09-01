using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SkeletonRevenge;

public class TextureManager
{
    private Dictionary<string, Texture2D> _textures;

    public int TextureWidth { get; private set; } = 64;
    public int TextureHeight { get; private set; } = 64;

    private Color[] _missingTexture;
    
    public TextureManager()
    {
        _textures = new Dictionary<string, Texture2D>();
        
        CreateMissingTexture();
    }

    public void LoadTextures(ContentManager content)
    {
    }

    public Color[] GetTextureColor(string textureName)
    {
        if (_textures.TryGetValue(textureName, out Texture2D texture))
        {
            Color[] color = new Color[TextureWidth * TextureHeight];
            texture.GetData(color);
            return color;
        }

        return _missingTexture;
    }

    private void CreateMissingTexture()
    {
        _missingTexture = new Color[TextureWidth * TextureHeight];
        Color currentColor = Color.Fuchsia;
        for (int x = 0; x < TextureWidth; x++)
        {
            for (int y = 0; y < TextureHeight; y++)
            {
                if (x <= TextureWidth / 2 && y <= TextureHeight / 2)
                    currentColor = Color.Black;
                else if (x > TextureWidth / 2 && y > TextureHeight / 2)
                    currentColor = Color.Black;
                else
                    currentColor = Color.Fuchsia;
                
                _missingTexture[x + TextureWidth * y] = currentColor;
            }
        }
    }
}