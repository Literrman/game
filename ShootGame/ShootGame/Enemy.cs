using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static ShootGame.Hero;

namespace ShootGame
{
    class Enemy
    {
        private EName Name;
        public Vector Location;
        public int Health;
        public int Damage;
        public readonly int Experiens;
        public double Direction;
        public int HitBox;

        public Image EnemyIMG;
        private int animation;
        private int animation2;
        public static Queue<Enemy> Blood = new Queue<Enemy>();
        public static readonly HashSet<Enemy> Enemies = new HashSet<Enemy>();

        private static Random rnd = new Random();
        public static int Count { get; private set; }

        public Enemy(EName name, Vector location, double direction = 0)
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

        public Enemy(double direction, Vector location)
        {
            Direction = direction;
            Location = location;
        }

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

            foreach (var bull in Bullet.Bullets.ToList())
                if ((Location - bull.Location).Length < HitBox)
                {
                    Bullet.Bullets.Remove(bull);
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

        public static readonly Dictionary<EName, string[]> EnemiesIMG = new Dictionary<EName, string[]>
        {
            [EName.robot0] = new[] {"r0_02", "r0_01", "r0_00", "r0_03", "r0_04", "r0_05", "r0_06", "r0_07", "r0_08", "dead_00"},
            [EName.robot1] = new[] {"r1_02", "r1_01", "r1_00", "r1_03", "r1_04", "r1_05", "r1_06", "r1_07", "r1_08", "dead_01"},
            [EName.robot2] = new[] {"r2_02", "r2_01", "r2_00", "r2_03", "r2_04", "r2_05", "r2_06", "r2_07", "r2_08", "dead_02"},
            [EName.robot3] = new[] {"r3_02", "r3_01", "r3_00", "r3_03", "r3_04", "r3_05", "r3_06", "r3_07", "r3_08", "dead_02"},
        };

        public static readonly Dictionary<EName, Tuple<int, int, int, int>> EnemiesInfo = new Dictionary<EName, Tuple<int, int, int, int>>
        {
            [EName.robot0] = Tuple.Create(20, 10, 5, 18),
            [EName.robot1] = Tuple.Create(20, 10, 5, 18),
            [EName.robot2] = Tuple.Create(20, 10, 5, 18),
            [EName.robot3] = Tuple.Create(30, 15, 5, 25),

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