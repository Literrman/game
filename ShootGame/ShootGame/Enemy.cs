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
        private Name Name;
        public /*readonly*/ Vector Location;
        public /*readonly*/ int Health;
        public readonly int Experiens;
        public /*readonly*/ double Direction;
        public int HitBox;

        public Image EnemyIMG;
        private uint animation;
        public static Queue<Enemy> Blood = new Queue<Enemy>();
        public static readonly HashSet<Enemy> Enemies = new HashSet<Enemy>();

        public Enemy(Name name, Vector location, double direction = 0)
        {
            Name = name;
            Location = location;
            Health = EnemiesInfo[Name].Item1;
            Experiens = EnemiesInfo[Name].Item2;
            HitBox = EnemiesInfo[Name].Item3;
            Direction = direction;
            Enemies.Add(this);
        }

        public Enemy(double direction, Vector location)
        {
            Direction = direction;
            Location = location;
        }

        public void Move(Vector targetLoc)
        {
            Direction = (targetLoc - Location).Angle;
            Location += new Vector(Math.Cos(Direction), Math.Sin(Direction));
            EnemyIMG = GetImage(EnemiesIMG[Name][++animation / 10 % 5]);
            if (animation == 50) animation = 0;

            foreach(var bull in Bullet.Bullets.ToList())
                if ((Location - bull.Location).Length < HitBox)
                {                    
                    Bullet.Bullets.Remove(bull);
                    Health -= bull.Damage;
                    if (Health <= 0)
                    {
                        Enemies.Remove(this);
                        EnemyIMG = GetImage(EnemiesIMG[Name][9]);
                        Blood.Enqueue(this);
                    }
                    break;
                }
        }

        public static readonly Dictionary<Name, string[]> EnemiesIMG = new Dictionary<Name, string[]>
        {
            [Name.robot0] = new[] {"r0_02", "r0_01", "r0_00", "r0_03", "r0_04", "r0_05", "r0_05", "r0_07", "r0_08", "dead_00"},
            [Name.robot1] = new[] {"r1_02", "r1_01", "r1_00", "r1_03", "r1_04", "r1_05", "r1_05", "r1_07", "r1_08", "dead_00"},
            [Name.robot2] = new[] {"r2_02", "r2_01", "r2_00", "r2_03", "r2_04", "r2_05", "r2_05", "r2_07", "r2_08", "dead_00"},
        };

        public static readonly Dictionary<Name, Tuple<int, int, int>> EnemiesInfo = new Dictionary<Name, Tuple<int, int, int>>
        {
            [Name.robot0] = Tuple.Create(20,5,15),
            [Name.robot1] = Tuple.Create(20,5,15),
            [Name.robot2] = Tuple.Create(30,5,20)

        };
    }
}

internal enum Name
{
    robot0,
    robot1,
    robot2,
    monstr,
}