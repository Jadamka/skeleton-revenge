using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SkeletonRevenge;

public class Player
{
    public Vector2 Position;
    public Vector2 Direction;
    // Camera plane
    public Vector2 Plane;

    private readonly float _moveSpeed;
    private readonly float _rotationSpeed;

    public Player(Vector2 position, Vector2 direction)
    {
        Position = position;
        Direction = direction;

        Plane = new Vector2(0, 0.66f); // FOV
        
        _moveSpeed = 5.0f;
        _rotationSpeed = 3.0f;
    }

    public void Update(GameTime gameTime, Level level)
    {
        double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            if (level.wallMap[(int)(Position.Y), (int)(Position.X + Direction.X * _moveSpeed * deltaTime)] == 0)
                Position.X += (float)(Direction.X * _moveSpeed * deltaTime);
            if (level.wallMap[(int)(Position.Y + Direction.Y * _moveSpeed * deltaTime), (int)(Position.X)] == 0)
                Position.Y += (float)(Direction.Y * _moveSpeed * deltaTime);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            if (level.wallMap[(int)(Position.Y), (int)(Position.X - Direction.X * _moveSpeed * deltaTime)] == 0)
                Position.X -= (float)(Direction.X * _moveSpeed * deltaTime);
            if (level.wallMap[(int)(Position.Y - Direction.Y * _moveSpeed * deltaTime), (int)(Position.X)] == 0)
                Position.Y -= (float)(Direction.Y * _moveSpeed * deltaTime);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            double oldDirX = Direction.X;
            Direction.X = (float)(Direction.X * Math.Cos(-_rotationSpeed * deltaTime) -
                          Direction.Y * Math.Sin(-_rotationSpeed * deltaTime));
            Direction.Y = (float)(oldDirX * Math.Sin(-_rotationSpeed * deltaTime) +
                                  Direction.Y * Math.Cos(-_rotationSpeed * deltaTime));
            double oldPlaneX = Plane.X;
            Plane.X = (float)(Plane.X * Math.Cos(-_rotationSpeed * deltaTime) -
                              Plane.Y * Math.Sin(-_rotationSpeed * deltaTime));
            Plane.Y = (float)(oldPlaneX * Math.Sin(-_rotationSpeed * deltaTime) +
                              Plane.Y * Math.Cos(-_rotationSpeed * deltaTime));
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            double oldDirX = Direction.X;
            Direction.X = (float)(Direction.X * Math.Cos(_rotationSpeed * deltaTime) -
                                  Direction.Y * Math.Sin(_rotationSpeed * deltaTime));
            Direction.Y = (float)(oldDirX * Math.Sin(_rotationSpeed * deltaTime) +
                                  Direction.Y * Math.Cos(_rotationSpeed * deltaTime));
            double oldPlaneX = Plane.X;
            Plane.X = (float)(Plane.X * Math.Cos(_rotationSpeed * deltaTime) -
                              Plane.Y * Math.Sin(_rotationSpeed * deltaTime));
            Plane.Y = (float)(oldPlaneX * Math.Sin(_rotationSpeed * deltaTime) +
                              Plane.Y * Math.Cos(_rotationSpeed * deltaTime));
        }
    }
}