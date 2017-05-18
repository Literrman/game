using System;
using System.Collections.Generic;
using System.Drawing;
using static ShootGame.Properties.Resources;

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

        public Hero(Vector location, int health, int experiens, double direction)
        {
            Location = location;
            Health = health;
            Experiens = experiens;
            Direction = direction;
        }
        public Hero(Vector location) => Location = location;

        public Hero(double direction, Vector location)
        {
            Direction = direction;
            Location = location;
        }

        //public void MoveHero(Size space, bool[] movement)
        //{
        //    var x = 0;
        //    var y = 0;

        //    if (movement[(int)Step.Left]) x -= StepLen;
        //    if (movement[(int)Step.Right]) x += StepLen;
        //    if (movement[(int)Step.Up]) y -= StepLen;
        //    if (movement[(int)Step.Down]) y += StepLen;

        //    var loc = Location + new Vector(x, y) / (x != 0 && y != 0 ? Math.Sqrt(2) : 1);

        //    if (loc.X - 20 < 0) loc = new Vector(20, loc.Y);
        //    if (loc.Y - 20 < 0) loc = new Vector(loc.X, 20);
        //    if (loc.X + 20 > space.Width) loc = new Vector(space.Width - 20, loc.Y);
        //    if (loc.Y + 20 > space.Height) loc = new Vector(loc.X, space.Height - 20);

        //    Location = loc;
        //}

        //public void RotateHero(Point e)
        //{
        //    var length = Math.Sqrt(Math.Pow(e.X - Location.X, 2) + Math.Pow(e.Y - Location.Y, 2));
        //    var angle = Math.Acos((e.X - Location.X) / length);
        //    if (e.Y <= Location.Y) angle = -angle;
        //    Direction = angle;
        //}

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

        //public static readonly Dictionary<MainHero, Dictionary<Weapon, Tuple<string[], int, int, int, int, int>>> HeroInfo = new Dictionary<MainHero, Dictionary<Weapon, Tuple<string[], int, int, int, int, int>>>
        //{
        //    [MainHero.Halloween] = new Dictionary<Weapon, Tuple<string[], int, int, int, int, int>>
        //    {
        //        [Weapon.UZI] = Tuple.Create(new[] { "hero0_07", "hero0_08", "hero0_09", "hero0_06", "hero0_05" }, 10, 800, 700, 8, 20),
        //        [Weapon.Shotgun] = Tuple.Create(new[] { "hero0_17", "hero0_18", "hero0_19", "hero0_16", "hero0_15" }, 4, 600, 1500, 20, 40),
        //        [Weapon.Plasmagun] = Tuple.Create(new[] { "hero0_27", "hero0_28", "hero0_29", "hero0_26", "hero0_25" }, 50, 1200, 1000, 20, 20),

        //    },

        //    [MainHero.Death] = new Dictionary<Weapon, Tuple<string[], int, int, int, int, int>>
        //    {
        //        [Weapon.UZI] = Tuple.Create(new[] { "hero1_07", "hero1_08", "hero1_09", "hero1_06", "hero1_05" }, 10, 800, 700, 8, 20),
        //        [Weapon.Shotgun] = Tuple.Create(new[] { "hero1_17", "hero1_18", "hero1_19", "hero1_16", "hero1_15" }, 4, 600, 1500, 20, 40),
        //        [Weapon.Plasmagun] = Tuple.Create(new[] { "hero1_27", "hero1_28", "hero1_29", "hero1_26", "hero1_25" }, 50, 1200, 1000, 20, 20),
        //    }
        //};

        //item1 - img, item2 - bullet,  item3 - maxAmmo, item4 - speed, item5 - cooldown, item6 - damage, item7 - scatter
        public static Bitmap GetImage(string e) => (Bitmap)ResourceManager.GetObject(e);
    }
}


public enum Weapon
{
    UZI = 0,
    Shotgun = 1,
    Plasmagun = 2,
}

public enum MainHero
{
    Halloween,
    Death,
}