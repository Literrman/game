using System;
using System.Collections.Generic;
using System.Linq;

namespace ShootGame
{
    public class Bullet
    {
        public Weapon Name;
        public readonly Vector Location;

        public readonly int MaxAmmo;
        private readonly int Speed;
        public readonly int Cooldown;
        public readonly int Reload;
        public readonly int Damage;
        public readonly int Scatter;

        public readonly double Direction;
        private static double MaxDistance = 2000;
        public static readonly HashSet<Bullet> Bullets = new HashSet<Bullet>();

        public Bullet(Weapon name, Vector location, double direction)
        {
            Name = name;
            Location = new Vector(location);
            Direction = direction;

            MaxAmmo = WeaponInfo[Name].Item1;
            Speed = WeaponInfo[Name].Item2;
            Cooldown = WeaponInfo[Name].Item3;
            Reload = WeaponInfo[Name].Item4;
            Damage = WeaponInfo[Name].Item5;
            Scatter = WeaponInfo[Name].Item6;

            Bullets.Add(this);
        }

        public void Move()
        {          
            Location.X += Speed*Math.Cos(Direction);
            Location.Y += Speed*Math.Sin(Direction);
            if (Location.Length > MaxDistance) Bullets.Remove(this);
        }

        public readonly Dictionary<Weapon, Tuple<int,int,int,int,int,int>> WeaponInfo = new Dictionary<Weapon, Tuple<int, int, int, int, int, int>>
        {
            [Weapon.UZI] = Tuple.Create(10, 10, 8, 700, 8, 20),
            [Weapon.Shotgun] = Tuple.Create(4, 10, 20, 1500, 20, 40),
            [Weapon.Plasmagun] = Tuple.Create(50, 40, 15, 1000, 20, 20),
        };
    }//  item1 - maxAmmo, item2 - speed, item3 - cooldown, item4 - reload, item5 - damage, item6 - scatter
}