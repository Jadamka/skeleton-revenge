using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkeletonRevenge;

public class Renderer
{
    private Color[] _buffer;
    private int _screenWidth;
    private int _screenHeight;
    private Texture2D _screenTexture;

    public Renderer(GraphicsDevice graphicsDevice, int screenWidth, int screenHeight)
    {
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
        
        _screenTexture = new Texture2D(graphicsDevice, screenWidth, screenHeight);
        
        _buffer = new Color[_screenWidth * _screenHeight];
        ClearBuffer();
    }

    public void Render3D(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Player player, Level level)
    {
        for (int x = 0; x < _screenWidth; x++)
        {
            double cameraX = 2 * x / (double)(_screenWidth) - 1;
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

                if (level.worldMap[mapY, mapX] > 0) hitWall = true;
            }

            if (side == 0) perpWallDist = (sideDistX - deltaDistX);
            else perpWallDist = (sideDistY - deltaDistY);

            int lineHeight = (int)(_screenHeight / perpWallDist);

            int drawStart = -lineHeight / 2 + _screenHeight / 2;
            if (drawStart < 0) drawStart = 0;
            int drawEnd = lineHeight / 2 + _screenHeight / 2;
            if (drawEnd >= _screenHeight) drawEnd = _screenHeight - 1;

            Color color;
            switch (level.worldMap[mapY, mapX])
            {
                case 1: color = Color.Red; break;
                case 2: color = Color.Green; break;
                case 3: color = Color.Blue; break;
                case 4: color = Color.White; break;
                default: color = Color.Yellow; break;
            }

            if (side == 1) color = new Color(color.R / 2, color.G / 2, color.B / 2);

            for (int y = drawStart; y < drawEnd; y++)
            {
                _buffer[x + _screenWidth * y] = color;
            }
        }
        
        _screenTexture.SetData(_buffer);
        ClearBuffer();
        spriteBatch.Draw(_screenTexture, Vector2.Zero, Color.White);
    }

    private void ClearBuffer()
    {
        for (int i = 0; i < _screenWidth * _screenHeight; i++)
        {
            _buffer[i] = Color.Black;
        }
    }
}