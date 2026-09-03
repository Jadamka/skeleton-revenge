using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SkeletonRevenge.Entities;
using SkeletonRevenge.Graphics;

namespace SkeletonRevenge.World;

public enum TexturePallete
{
    WALL,
    FLOOR,
    CEILING,
    NONE
}

public class Level
{
    public Dictionary<int, string> WallPalette { get; private set; }
    public Dictionary<int, string> FloorPalette { get; private set; }
    public Dictionary<int, string> CeilingPalette { get; private set; }
    
    public int MapWidth { get; private set; }
    public int MapHeight { get; private set; }

    public List<Entity> entities;
    
    public int[,] WallMap { get; private set; }
    public int[,] FloorMap { get; private set; }
    public int[,] CeilingMap { get; private set; }
    
    public Level(Dictionary<int, string> wallPalette, Dictionary<int, string> floorPalette, 
        Dictionary<int, string> ceilingPalette, List<Entity> entities, int mapWidth, int mapHeight, 
        int[,] wallMap, int[,] floorMap, int[,] ceilingMap)
    {
        WallPalette = wallPalette;
        FloorPalette = floorPalette;
        CeilingPalette = ceilingPalette;

        this.entities = entities;

        MapWidth = mapWidth;
        MapHeight = mapHeight;

        WallMap = wallMap;
        FloorMap = floorMap;
        CeilingMap = ceilingMap;
    }
    
    public string GetTextureName(int key, TexturePallete pallete = TexturePallete.NONE)
    {
        if (pallete == TexturePallete.WALL)
        {
            if (WallPalette.TryGetValue(key, out string textureName))
            {
                return textureName;
            }
        }
        else if (pallete == TexturePallete.FLOOR)
        {
            if (FloorPalette.TryGetValue(key, out string textureName))
            {
                return textureName;
            }
        }
        else if (pallete == TexturePallete.CEILING)
        {
            if (CeilingPalette.TryGetValue(key, out string textureName))
            {
                return textureName;
            }
        }

        return TextureNames.Textures.MissingTexture;
    }
}