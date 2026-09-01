using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SkeletonRevenge;

public class TextureManager
{
    private Dictionary<string, Color[]> _textures;

    public int TextureWidth { get; private set; } = 128;
    public int TextureHeight { get; private set; } = 128;

    private Color[] _missingTexture;
    
    public TextureManager()
    {
        _textures = new Dictionary<string, Color[]>();
        
        CreateMissingTexture();
    }

    public void LoadTextures(ContentManager content)
    {
        _textures[TextureNames.Stone] = LoadAndExtract(content, "textures/stone_texture");
        _textures[TextureNames.BarrelWall] = LoadAndExtract(content,"textures/barrel_wall_texture");
        _textures[TextureNames.WoodWall] = LoadAndExtract(content,"textures/wood_wall_texture");
        _textures[TextureNames.MansionWall] = LoadAndExtract(content,"textures/mansion_wall_texture");
    }

    private Color[] LoadAndExtract(ContentManager content, string path)
    {
        Texture2D texture = content.Load<Texture2D>(path);
        Color[] data = new Color[TextureWidth * TextureHeight];
        
        texture.GetData(data);
        return data;
    }

    public Color[] GetTextureColor(string textureName)
    {
        if (_textures.TryGetValue(textureName, out Color[] colors))
        {
            return colors;
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