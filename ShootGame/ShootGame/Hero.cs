using System.Collections.Generic;
using System.Drawing;
using static ShootGame.Properties.Resources;

namespace ShootGame
{
    public class Hero
    {
        public readonly Vector Location;
        public readonly ushort Health;
        public readonly int Experiens;
        public readonly double Direction;

        public Hero(Vector location, ushort health, int experiens, double direction)
        {
            Location = location;
            Health = health;
            Experiens = experiens;
            Direction = direction;
        }
        public Hero(Vector location) => Location = location;
        public Hero(ushort health) => Health = health;
        public Hero(int experiens) => Experiens = experiens;
        public Hero(double direction) => Direction = direction;

        public Hero(double direction, Vector location)
        {
            Direction = direction;
            Location = location;
        }

        public static readonly string[] Heros = { "hero_07", "hero_08", "hero_09", "hero_06", "hero_05" };

        public static Image GetImage(string e) => (Image)ResourceManager.GetObject(e);
    }
}