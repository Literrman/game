using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ShootGame
{
    public class Bullet
    {
        public Weapon Name;
        public readonly Vector Location;
        public readonly double Direction;
        private static double MaxDistance = 2000;

        public readonly int MaxAmmo;
        private readonly int Speed;
        public readonly int Cooldown;
        public readonly int Reload;
        public readonly int Damage;
        public readonly int Scatter;

        private static Stopwatch timer = Stopwatch.StartNew();
        private static long tmptime;
        
        public static readonly HashSet<Bullet> Bullets = new HashSet<Bullet>();

        private static readonly int[] t = new int[3];
        private static readonly long[] reloadTime = new long[3]; 
        private static readonly bool[] isRewrite = new bool[3];

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

        public static void Shoot(Weapon name, Vector location, double direction)
        {
            if (timer.ElapsedMilliseconds - reloadTime[(int) name] >= WeaponInfo[name].Item4 && isRewrite[(int) name])
            {
                isRewrite[(int) name] = false;
                t[(int) name] = 0;
            }
            if (timer.ElapsedMilliseconds - tmptime < WeaponInfo[name].Item3 || isRewrite[(int) name]) return;

            if (name == Weapon.Shotgun)
                for (var i = -2; i <= 2; i++)
                    new Bullet(name, location, direction + i / Math.PI / 4);
            if (name == Weapon.UZI) new Bullet(name, location, direction);
            if (name == Weapon.Plasmagun) new Bullet(name, location, direction);

            tmptime = timer.ElapsedMilliseconds;
            t[(int) name]++;

            if (t[(int) name] < WeaponInfo[name].Item1) return;

            reloadTime[(int) name] = timer.ElapsedMilliseconds;
            isRewrite[(int) name] = true;
        }

        private static readonly Dictionary<Weapon, Tuple<int, int, int, int, int, int>> WeaponInfo = new Dictionary<Weapon, Tuple<int, int, int, int, int, int>>
        {
            [Weapon.UZI] = Tuple.Create(20, 10, 270, 1000, 8, 20),
            [Weapon.Shotgun] = Tuple.Create(5, 10, 700, 2000, 15, 40),
            [Weapon.Plasmagun] = Tuple.Create(40, 10, 100, 1200, 12, 20),
        };


    }//  item1 - maxAmmo, item2 - speed, item3 - cooldown, item4 - reload, item5 - damage, item6 - scatter
}