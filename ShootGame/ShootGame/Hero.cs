using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using static ShootGame.Properties.Resources;
using static ShootGame.Bullet;

namespace ShootGame
{
    public class Hero
    {
        public Vector Location;
        public int Health;
        public int Experiens;
        public double Direction;
        public const int HitBox = 30;

        public readonly Image HeroImage;
        private const int StepLen = 3;

        private static Stopwatch timer = Stopwatch.StartNew();
        private static long tmptime;

        private static readonly int[] t = new int[3];
        private static readonly long[] reloadTime = new long[3];
        private static readonly bool[] isRewrite = new bool[3];

        public Hero(Vector location, int health, int experiens, double direction)
        {
            Location = location;
            Health = health;
            Experiens = experiens;
            Direction = direction;
        }

        public static void Shoot(Weapon name, Vector location, double direction)
        {
            if (timer.ElapsedMilliseconds - reloadTime[(int)name] >= WeaponInfo[name].Item4 && isRewrite[(int)name])
            {
                isRewrite[(int)name] = false;
                t[(int)name] = 0;
            }
            if (timer.ElapsedMilliseconds - tmptime < WeaponInfo[name].Item3 || isRewrite[(int)name]) return;

            switch (name)
            {
                case Weapon.Shotgun:
                    for (var i = -2; i <= 2; i++)
                        new Bullet(name, location, direction + i / Math.PI / 4, true);
                    break;
                case Weapon.UZI:
                    new Bullet(name, location, direction, true);
                    break;
                case Weapon.Plasmagun:
                    new Bullet(name, location, direction, true);
                    break;
            }

            tmptime = timer.ElapsedMilliseconds;
            t[(int)name]++;

            if (t[(int)name] < WeaponInfo[name].Item1) return;

            reloadTime[(int)name] = timer.ElapsedMilliseconds;
            isRewrite[(int)name] = true;
        }

        public void CheckEnemyBull()
        {
            foreach (var bull in Bullets.ToList())
            {
                if (bull.IsMy || (Location - bull.Location).Length >= HitBox) continue;
                Bullets.Remove(bull);
                Health -= bull.Damage;
            }
        }

        public static readonly Dictionary<MainHero, Dictionary<Weapon, string[]>> Heroes = new Dictionary<MainHero, Dictionary<Weapon, string[]>>
        {
            [MainHero.Halloween] = new Dictionary<Weapon, string[]>
            {
                [Weapon.UZI] = new[] { "hero0_07", "hero0_08", "hero0_09", "hero0_06", "hero0_05" },
                [Weapon.Shotgun] = new[] { "hero0_17", "hero0_18", "hero0_19", "hero0_16", "hero0_15" },
                [Weapon.Plasmagun] = new[] { "hero0_27", "hero0_28", "hero0_29", "hero0_26", "hero0_25" }
            },

            [MainHero.Death] = new Dictionary<Weapon, string[]>
            {
                [Weapon.UZI] = new[] { "hero1_07", "hero1_08", "hero1_09", "hero1_06", "hero1_05" },
                [Weapon.Shotgun] = new[] { "hero1_17", "hero1_18", "hero1_19", "hero1_16", "hero1_15" },
                [Weapon.Plasmagun] = new[] { "hero1_27", "hero1_28", "hero1_29", "hero1_26", "hero1_25" }
            }
        };
        public static Bitmap GetImage(string e) => (Bitmap)ResourceManager.GetObject(e);
    }
}

public enum Weapon
{
    UZI = 0,
    Shotgun = 1,
    Plasmagun = 2,

    Rocket,
    Laser,
}

public enum MainHero
{
    Halloween,
    Death,
}