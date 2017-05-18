using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace ShootGame
{
    public class Bullet
    {
        public Weapon Name;
        public readonly Bitmap BullIMG;

        public readonly Vector Location;
        public double Direction;
        private static double MaxDistance = 2000;

        public readonly int MaxAmmo;
        private readonly int Speed;
        public readonly int Cooldown;
        public readonly int Reload;
        public readonly int Damage;
        public readonly int Scatter;
        public readonly bool IsMy;
        
        public static readonly HashSet<Bullet> Bullets = new HashSet<Bullet>();

        public Bullet(Weapon name, Vector location, double direction, bool isMy)
        {
            Name = name;
            BullIMG = Hero.GetImage(WeaponInfo[Name].Item7);
            IsMy = isMy;
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

        public void Move(Hero hero)
        {
            if (Name == Weapon.Rocket) Direction = (hero.Location - Location).Angle;
            Location.X += Speed*Math.Cos(Direction);
            Location.Y += Speed*Math.Sin(Direction);
            if (Location.Length > MaxDistance) Bullets.Remove(this);
        }

        public static readonly Dictionary<Weapon, Tuple<int, int, int, int, int, int, string>> WeaponInfo = new Dictionary<Weapon, Tuple<int, int, int, int, int, int, string>>
        {
            [Weapon.UZI] = Tuple.Create(20, 10, 270, 1000, 8, 20, "bullRED"),
            [Weapon.Shotgun] = Tuple.Create(5, 10, 700, 2000, 15, 40, "bullMETAL"),
            [Weapon.Plasmagun] = Tuple.Create(40, 10, 100, 1200, 12, 20, "bullGREEN"),
            [Weapon.Rocket] = Tuple.Create(0, 2, 5000, 0, 5, 20, "bullRocket"),
            [Weapon.Laser] = Tuple.Create(0, 10, 300, 0, 2, 20, "bullBLUE"),
        };
    }//  item1 - maxAmmo, item2 - speed, item3 - cooldown, item4 - reload, item5 - damage, item6 - scatter
    //item1 - speed, item2 - cooldown, item3-damage, item4 - scatter
}