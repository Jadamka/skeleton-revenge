using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SkeletonRevenge.Graphics;

public class TextureManager
{
    private Dictionary<string, Color[]> _cpuTextures;

    public int TextureWidth { get; private set; } = 128;
    public int TextureHeight { get; private set; } = 128;

    private Color[] _missingTexture;
    
    public TextureManager()
    {
        _cpuTextures = new Dictionary<string, Color[]>();
        
        CreateMissingTexture();
    }

    public void LoadTextures(ContentManager content)
    {
        // CPU -> For raycasting
        _cpuTextures[TextureNames.Textures.Stone] = LoadAndExtract(content, "textures/stone_texture");
        _cpuTextures[TextureNames.Textures.BarrelWall] = LoadAndExtract(content,"textures/barrel_wall_texture");
        _cpuTextures[TextureNames.Textures.WoodWall] = LoadAndExtract(content,"textures/wood_wall_texture");
        _cpuTextures[TextureNames.Textures.MansionWall] = LoadAndExtract(content,"textures/mansion_wall_texture");
        _cpuTextures[TextureNames.Textures.GrassFloor] = LoadAndExtract(content, "textures/grass_floor_texture");
        _cpuTextures[TextureNames.Textures.BloodyBrickWall] = LoadAndExtract(content, "textures/bloody_brick_wall_texture");

        _cpuTextures[TextureNames.Sprites.RatSprite1] = LoadAndExtract(content, "sprites/rat_sprite_1");
    }

    private Color[] LoadAndExtract(ContentManager content, string path)
    {
        Texture2D texture = content.Load<Texture2D>(path);
        Color[] data = new Color[TextureWidth * TextureHeight];
        
        texture.GetData(data);
        return data;
    }

    public Color[] GetCpuTextureColor(string textureName)
    {
        if (_cpuTextures.TryGetValue(textureName, out Color[] colors))
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