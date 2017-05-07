using System;
using System.Collections.Generic;

namespace ShootGame
{
    public class Bullet
    {
        public Vector Location;
        public readonly double Direction;
        public List<Bullet> Bullets = new List<Bullet>();

        public Bullet(Vector location, double direction)
        {
            Location = location;
            Direction = direction;
            Bullets.Add(this);
        }

        private void Move()
        {
            Location.X += 1*Math.Cos(Direction);
            Location.Y += 1*Math.Sin(Direction);
        }
    }
}