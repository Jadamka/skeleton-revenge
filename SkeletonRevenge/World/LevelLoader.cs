using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SkeletonRevenge.Entities;
using SkeletonRevenge.Graphics;

namespace SkeletonRevenge.World;

public static class LevelLoader
{
    public static Level LoadLevelXML(ContentManager content, TextureManager textureManager, string filePath)
    {
        string fullPath = Path.Combine(content.RootDirectory, filePath);

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException();
        }

        Dictionary<int, string> wallPalette = new();
        Dictionary<int, string> floorPalette = new();
        Dictionary<int, string> ceilingPalette = new();
        List<Entity> entities = new();

        XDocument doc = XDocument.Load(fullPath);
        XElement settings = doc.Root.Element("Settings");

        int mapWidth = int.Parse(settings!.Attribute("width")!.Value);
        int mapHeight = int.Parse(settings!.Attribute("height")!.Value);

        XElement palettesNode = doc.Root.Element("Palettes");
        foreach (XElement paletteNode in palettesNode!.Descendants())
        {
            string paletteName = paletteNode.Name.ToString();
            if (paletteName == "Wall")
                wallPalette = GetTexturePalette(paletteNode);
            else if (paletteName == "Floor")
                floorPalette = GetTexturePalette(paletteNode);
            else if (paletteName == "Ceiling")
                ceilingPalette = GetTexturePalette(paletteNode);
        }

        // TODO: Set player data
        //XElement player = doc.Root.Element("Player");

        XElement entitiesNode = doc.Root.Element("Entities");
        foreach (XElement entityNode in entitiesNode.Elements("Entity"))
        {
            // TODO: Add certain types of entities
            float x = float.Parse(entityNode.Attribute("x")!.Value);
            float y = float.Parse(entityNode.Attribute("y")!.Value);
            entities.Add(new Entity(new Vector2(x, y),
                textureManager.GetCpuTextureColor(TextureNames.Sprites.RatSprite1)));
        }

        XElement mapsNode = doc.Root.Element("Maps");
        XElement wallMapNode = mapsNode!.Element("WallMap");
        XElement floorMapNode = mapsNode!.Element("FloorMap");
        XElement ceilingMapNode = mapsNode!.Element("CeilingMap");

        string wallMapData = wallMapNode!.Value.Replace("\n", "").Replace("\r", "").Trim();
        string floorMapData = floorMapNode!.Value.Replace("\n", "").Replace("\r", "").Trim();
        string ceilingMapData = ceilingMapNode!.Value.Replace("\n", "").Replace("\r", "").Trim();

        int[,] wallMap = CreateMap(wallMapData, mapWidth, mapHeight);
        int[,] floorMap = CreateMap(floorMapData, mapWidth, mapHeight);
        int[,] ceilingMap = CreateMap(ceilingMapData, mapWidth, mapHeight);

        Level level = new Level(wallPalette, floorPalette, ceilingPalette, entities, mapWidth, mapHeight,
            wallMap, floorMap, ceilingMap);

        return level;
    }

    private static Dictionary<int, string> GetTexturePalette(XElement paletteNode)
    {
        Dictionary<int, string> dictionary = new();
        foreach (XElement textureNode in paletteNode.Elements("Texture"))
        {
            int id = int.Parse(textureNode.Attribute("id")!.Value);
            string textureName = textureNode.Attribute("name")!.Value;
            dictionary.Add(id, textureName);
        }
        return dictionary;
    }

    private static int[,] CreateMap(string rawData, int w, int h)
    {
        int[,] map = new int[h, w];

        string[] data = rawData.Split(',');

        if (data.Length != w * h) 
            throw new InvalidDataException($"Map size mismatch! Expected {w * h} tiles, but got {data.Length}.");
        
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                map[y, x] = int.Parse(data[x + y * w]);
            }
        }
        
        return map;
    }
}