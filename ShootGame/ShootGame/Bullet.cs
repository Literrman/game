using System;
using System.Collections.Generic;
using System.Linq;

namespace ShootGame
{
    public class Bullet
    {
        public readonly Vector Location;
        public readonly double Direction;
        private readonly double Speed;
        public readonly ushort Damage;
        private static double MaxDistance = 2000;
        public static readonly HashSet<Bullet> Bullets = new HashSet<Bullet>();

        public Bullet(Vector location, ushort damage, double direction, double speed)
        {
            Location = new Vector(location);
            Damage = damage;
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