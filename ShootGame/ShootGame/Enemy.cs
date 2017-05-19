using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using static ShootGame.Hero;
using static ShootGame.Bullet;

namespace ShootGame
{
    class Enemy
    {
        public EName Name;
        public Vector Location;
        public double Direction;

        public int Health;
        public int Damage;
        public readonly int Experiens;
        public int HitBox;

        public Image EnemyIMG;
        private int animation;
        private int animation2;

        public static Queue<Enemy> Blood = new Queue<Enemy>();
        public static readonly HashSet<Enemy> Enemies = new HashSet<Enemy>();

        private static Random rnd = new Random();
        public static int Count { get; private set; }

        private static Stopwatch timer = Stopwatch.StartNew();
        private static long tmptime;

        private Enemy(EName name, Vector location, double direction = 0)
        {
            Name = name;
            EnemyIMG = GetImage(EnemiesIMG[Name][0]);
            Location = location;
            Direction = direction;

            Health = EnemiesInfo[Name].Item1;
            Damage = EnemiesInfo[Name].Item2;
            Experiens = EnemiesInfo[Name].Item3;
            HitBox = EnemiesInfo[Name].Item4;           
            Enemies.Add(this);
        }

        public Enemy(Size mapSize) => new Enemy(RndName(), RndStartLoc(mapSize));

        public void Move(Hero hero, int timeCount)
        {
            Direction = (hero.Location - Location).Angle;
            EnemyIMG = GetImage(EnemiesIMG[Name][animation++ / 10]);
            animation %= 50;

            if ((Location - hero.Location).Length < Hero.HitBox)
            {
                EnemyIMG = GetImage(EnemiesIMG[Name][animation2++ / 10 + 5]);
                animation2 %= 40;
                hero.Health -= timeCount % 500 == 0 ? Damage : 0;
            }
            else Location += 0.5 * new Vector(Math.Cos(Direction), Math.Sin(Direction));        
        }

        public void CheckIsDead(Hero hero)
        {
            foreach (var bull in Bullets.ToList())
            {
                foreach (var bullet in Bullets.ToList())
                {
                    if (bullet.IsMy == bull.IsMy || (bull.Location - bullet.Location).Length >= 7) continue;
                    Bullets.Remove(bull);
                    Bullets.Remove(bullet);
                    break;
                }

                if (!bull.IsMy || (Location - bull.Location).Length >= HitBox) continue;
                Bullets.Remove(bull);
                Health -= bull.Damage;
                if (Health <= 0)
                {
                    hero.Experiens += Experiens;
                    Enemies.Remove(this);
                    EnemyIMG = GetImage(EnemiesIMG[Name][9]);
                    Blood.Enqueue(this);
                    Count++;
                    if (Blood.Count == 50) Blood.Dequeue();
                }
                break;
            }
        }

        public static void Shoot(Weapon name, Enemy enemy)
        {
            if (timer.ElapsedMilliseconds - tmptime < WeaponInfo[name].Item3) return;
            if (name == Weapon.Rocket || name == Weapon.Laser) new Bullet(name, enemy.Location, enemy.Direction, false);
            tmptime = timer.ElapsedMilliseconds;
        }

        public static void KillMobs()
        {
            Enemies.Clear();
            Blood.Clear();
            Count = 0;
        }

        private static Vector RndStartLoc(Size mapSize)
        {
            var a = mapSize.Width;
            var b = mapSize.Height;
            var perimetr = (a + b) * 2;
            var n = rnd.Next(perimetr);

            if (n < a) return new Vector(n, -20);
            if (n - a < b) return new Vector(a + 20, n - a);
            return n - a - b < a ? new Vector(n - a - b, b + 20) : new Vector(0, n - 2 * a - b);
        }

        private static EName RndName()
        {
            switch (rnd.Next(4))
            {
                case 0: return EName.robot0;
                case 1: return EName.robot1;
                case 2: return EName.robot2;
                case 3: return EName.robot3;
                default: throw new Exception();
            }
        }

        private static readonly Dictionary<EName, string[]> EnemiesIMG = new Dictionary<EName, string[]>
        {
            [EName.robot0] = new[] {"r0_02", "r0_01", "r0_00", "r0_03", "r0_04", "r0_05", "r0_06", "r0_07", "r0_08", "dead_00"},
            [EName.robot1] = new[] {"r1_02", "r1_01", "r1_00", "r1_03", "r1_04", "r1_05", "r1_06", "r1_07", "r1_08", "dead_01"},
            [EName.robot2] = new[] {"r2_02", "r2_01", "r2_00", "r2_03", "r2_04", "r2_05", "r2_06", "r2_07", "r2_08", "dead_02"},
            [EName.robot3] = new[] {"r3_02", "r3_01", "r3_00", "r3_03", "r3_04", "r3_05", "r3_06", "r3_07", "r3_08", "dead_02"},
        };

        private static readonly Dictionary<EName, Tuple<int, int, int, int>> EnemiesInfo = new Dictionary<EName, Tuple<int, int, int, int>>
        {
            [EName.robot0] = Tuple.Create(20, 10, 5, 18),
            [EName.robot1] = Tuple.Create(20, 10, 5, 18),
            [EName.robot2] = Tuple.Create(20, 10, 5, 18),
            [EName.robot3] = Tuple.Create(120, 15, 5, 25),
        };
    }
}

internal enum EName
{
    robot0,
    robot1,
    robot2,
    robot3,
    monstr,
}