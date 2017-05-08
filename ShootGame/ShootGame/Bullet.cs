using System;
using System.Collections.Generic;

namespace ShootGame
{
    public class Bullet
    {
        public readonly Vector Location;
        public readonly double Direction;
        private readonly double Speed;
        private static double MaxDistance = 2000;
        public static readonly List<Bullet> Bullets = new List<Bullet>();

        public Bullet(Vector location, double direction, double speed)
        {
            Location = new Vector(location);
            Direction = direction;
            Speed = speed;
            Bullets.Add(this);
        }

        public void Move()
        {          
            Location.X += Speed*Math.Cos(Direction);
            Location.Y += Speed*Math.Sin(Direction);
            if (Location.Length > MaxDistance) Bullets.Remove(this);
        }
    }
}