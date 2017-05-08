using System;
using System.Collections.Generic;

namespace ShootGame
{
    public class Bullet
    {
        public Vector Location;
        public readonly double Direction;
        public static List<Bullet> Bullets = new List<Bullet>();

        public Bullet(Vector location, double direction)
        {
            Location = location;
            Direction = direction;
            Bullets.Add(this);
        }

        public void Move()
        {
            Location.X += 2*Math.Cos(Direction);
            Location.Y += 2*Math.Sin(Direction);
        }
    }
}