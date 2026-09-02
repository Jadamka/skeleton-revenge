using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkeletonRevenge;

public class Renderer
{
    private Color[] _buffer;
    private int _bufferWidth;
    private int _bufferHeight;
    private Texture2D _screenTexture;

    private Level _cachedLevel = null;
    private Dictionary<int, Color[]> _wallTextureCache = new Dictionary<int, Color[]>();
    private Dictionary<int, Color[]> _floorTextureCache = new Dictionary<int, Color[]>();
    private Dictionary<int, Color[]> _ceilingTextureCache = new Dictionary<int, Color[]>();
    private Color[] _missingTextureColors;

    private GraphicsDevice _graphicsDevice;

    public Renderer(GraphicsDevice graphicsDevice, int bufferWidth, int bufferHeight)
    {
        _bufferWidth = bufferWidth;
        _bufferHeight = bufferHeight;

        _graphicsDevice = graphicsDevice;
        _screenTexture = new Texture2D(graphicsDevice, bufferWidth, bufferHeight);
        
        _buffer = new Color[_bufferWidth * _bufferHeight];
        ClearBuffer();
    }

    private void FloorAndCeilingCasting(TextureManager textureManager, Player player, Level level)
    {
        for (int y = (_bufferHeight)/2 + 1; y < _bufferHeight; y++)
        {
            float rayDirX0 = (float)(player.Direction.X - player.Plane.X);
            float rayDirY0 = (float)(player.Direction.Y - player.Plane.Y);
            float rayDirX1 = (float)(player.Direction.X + player.Plane.X);
            float rayDirY1 = (float)(player.Direction.Y + player.Plane.Y);

            int p = y - _bufferHeight / 2;
            float posZ = 0.5f * _bufferHeight;
            float rowDistance = posZ / p;

            float floorStepX = rowDistance * (rayDirX1 - rayDirX0) / _bufferWidth;
            float floorStepY = rowDistance * (rayDirY1 - rayDirY0) / _bufferWidth;

            float floorX = (float)(player.Position.X + rowDistance * rayDirX0);
            float floorY = (float)(player.Position.Y + rowDistance * rayDirY0);

            for (int x = 0; x < _bufferWidth; x++)
            {
                int cellX = (int)(floorX);
                int cellY = (int)(floorY);
                
                int tx = (int)(textureManager.TextureWidth * (floorX - cellX)) & (textureManager.TextureWidth - 1);
                int ty = (int)(textureManager.TextureHeight * (floorY - cellY)) & (textureManager.TextureHeight - 1);

                floorX += floorStepX;
                floorY += floorStepY;

                // FIXME: get rid of this magic number
                if (cellX >= 0 && cellX < 24 && cellY >= 0 && cellY < 24)
                {
                    int texNum = level.floorMap[cellY, cellX];
                    if (!_floorTextureCache.TryGetValue(texNum, out Color[] floorTextureColors))
                    {
                        floorTextureColors = _missingTextureColors;
                    }
                    
                    Color texel = floorTextureColors[textureManager.TextureWidth * ty + tx];
                    texel = new Color(texel.R / 2, texel.G / 2, texel.B / 2);
                    _buffer[x + _bufferWidth * y] = texel;

                    texNum = level.ceilingMap[cellY, cellX];
                    if (!_ceilingTextureCache.TryGetValue(texNum, out Color[] ceilingTextureColors))
                    {
                        ceilingTextureColors = _missingTextureColors;
                    }

                    texel = ceilingTextureColors[textureManager.TextureWidth * ty + tx];
                    texel = new Color(texel.R / 2, texel.G / 2, texel.B / 2);
                    _buffer[x + (_bufferWidth * (_bufferHeight - y - 1))] = texel;

                }
            }
        }
    }

    private void WallCasting(TextureManager textureManager, Player player, Level level)
    {
        for (int x = 0; x < _bufferWidth; x++)
        {
            double cameraX = 2 * x / (double)(_bufferWidth) - 1;
            double rayDirX = player.Direction.X + player.Plane.X * cameraX;
            double rayDirY = player.Direction.Y + player.Plane.Y * cameraX;

            int mapX = (int)player.Position.X;
            int mapY = (int)player.Position.Y;

            double sideDistX;
            double sideDistY;

            double deltaDistX = (rayDirX == 0) ? 1e30 : Math.Abs(1 / rayDirX);
            double deltaDistY = (rayDirY == 0) ? 1e30 : Math.Abs(1 / rayDirY);
            double perpWallDist;

            int stepX;
            int stepY;

            bool hitWall = false;
            int side = 0; // NS or EW wall, hit?

            if (rayDirX < 0)
            {
                stepX = -1;
                sideDistX = (player.Position.X - mapX) * deltaDistX;
            }
            else
            {
                stepX = 1;
                sideDistX = (mapX + 1.0 - player.Position.X) * deltaDistX;
            }

            if (rayDirY < 0)
            {
                stepY = -1;
                sideDistY = (player.Position.Y - mapY) * deltaDistY;
            }
            else
            {
                stepY = 1;
                sideDistY = (mapY + 1.0 - player.Position.Y) * deltaDistY;
            }
            
            // DDA
            while (!hitWall)
            {
                if (sideDistX < sideDistY)
                {
                    sideDistX += deltaDistX;
                    mapX += stepX;
                    side = 0;
                }
                else
                {
                    sideDistY += deltaDistY;
                    mapY += stepY;
                    side = 1;
                }

                if (level.wallMap[mapY, mapX] > 0) hitWall = true;
            }

            if (side == 0) perpWallDist = (sideDistX - deltaDistX);
            else perpWallDist = (sideDistY - deltaDistY);

            int lineHeight = (int)(_bufferHeight / perpWallDist);

            int drawStart = -lineHeight / 2 + _bufferHeight / 2;
            if (drawStart < 0) drawStart = 0;
            int drawEnd = lineHeight / 2 + _bufferHeight / 2;
            if (drawEnd >= _bufferHeight) drawEnd = _bufferHeight - 1;
            
            int texNum = level.wallMap[mapY, mapX];
            if (!_wallTextureCache.TryGetValue(texNum, out Color[] wallTextureColors))
            {
                wallTextureColors = _missingTextureColors;
            }
            
            double wallX;
            if (side == 0) wallX = player.Position.Y + perpWallDist * rayDirY;
            else wallX = player.Position.X + perpWallDist * rayDirX;
            wallX -= Math.Floor(wallX);

            int texX = (int)(wallX * (double)(textureManager.TextureWidth));
            if (side == 0 && rayDirX > 0) texX = textureManager.TextureWidth - texX - 1;
            if (side == 1 && rayDirY < 0) texX = textureManager.TextureWidth - texX - 1;

            double step = 1.0 * textureManager.TextureHeight / lineHeight;
            double texPos = (drawStart - _bufferHeight / 2 + lineHeight / 2) * step;

            for (int y = drawStart; y < drawEnd; y++)
            {
                int texY = (int)texPos & (textureManager.TextureHeight - 1);
                texPos += step;
                Color texel = wallTextureColors[texX + textureManager.TextureWidth * texY];
                if (side == 1)
                    texel = new Color(texel.R / 2, texel.G / 2, texel.B / 2);
                
                _buffer[x + _bufferWidth * y] = texel;
            }
        }
    }
    
    public void Render3D(SpriteBatch spriteBatch, TextureManager textureManager, Player player, Level level)
    {
        if (_cachedLevel != level)
        {
            _missingTextureColors = textureManager.GetTextureColor(TextureNames.MissingTexture);
            
            _wallTextureCache.Clear();
            foreach (var kvp in level.WallPallete)
                _wallTextureCache[kvp.Key] = textureManager.GetTextureColor(kvp.Value);
            
            _floorTextureCache.Clear();
            foreach (var kvp in level.FloorPallete)
                _floorTextureCache[kvp.Key] = textureManager.GetTextureColor(kvp.Value);

            _ceilingTextureCache.Clear();
            foreach(var kvp in level.CeilingPallete)
                _ceilingTextureCache[kvp.Key] = textureManager.GetTextureColor(kvp.Value);
            
            _cachedLevel = level;
        }
        
        FloorAndCeilingCasting(textureManager, player, level);
        WallCasting(textureManager, player, level);
        
        _screenTexture.SetData(_buffer);
        ClearBuffer();

        Rectangle screenRect = new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
        spriteBatch.Draw(_screenTexture, screenRect, Color.White);
    }

    private void ClearBuffer()
    {
        Array.Fill(_buffer, Color.Black);
    }
}