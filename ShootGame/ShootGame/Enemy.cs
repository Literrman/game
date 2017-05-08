using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootGame
{
    class Enemy
    {
        public /*readonly*/ Vector Location;
        public readonly ushort Health;
        public readonly int Experiens;
        public /*readonly*/ double Direction;
        public static int Count;

        public Enemy(Vector location, ushort health, int experiens, double direction)
        {
            Location = location;
            Health = health;
            Experiens = experiens;
            Direction = direction;
        }
        public Enemy(Vector location) => Location = location;
        public Enemy(ushort health) => Health = health;
        public Enemy(int experiens) => Experiens = experiens;
        public Enemy(double direction) => Direction = direction;

        public Enemy(double direction, Vector location)
        {
            Direction = direction;
            Location = location;
        }

        public void Move(Vector targetLoc)
        {
            Direction = (targetLoc - Location).Angle;
            Location += new Vector(Math.Cos(Direction), Math.Sin(Direction));
        }

        public static readonly string[] Enemies = { "enemy0_02", "enemy0_01", "enemy0_00", "enemy0_03", "enemy0_04" };
    }
}
