using Microsoft.Xna.Framework;

namespace SkeletonRevenge.Entities;

public class Entity
{
    public Vector2 Position;
    public Color[] Pixels;

    public Entity(Vector2 positoin, Color[] pixels)
    {
        Position = positoin;
        Pixels = pixels;
    }
}