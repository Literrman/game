using System;
using System.Collections;
using System.Drawing;
using static ShootGame.Properties.Resources;


namespace ShootGame
{
    public class Level
    {    
        private static Vector _step = new Vector(1,0);
        public Level(string name, Hero hero)
        {
            Name = name;
            Hero = hero;
            InitialHero = hero;
        }

        public readonly string Name;
        public readonly Hero InitialHero;
        public Hero Hero;

        /*public bool IsCompleted => Hero.Location.Length < 20;*/ /////////////////////////////////////////////////////////////////////////////////

        //public bool IsDead => (Hero.Location - Monster.Location).Length < 20;
        public Image GetImage(string e) => (Image)ResourceManager.GetObject(e);

        public void MoveHero(Size space, bool[] movement)
        {
            var x = 0;
            var y = 0;
            Vector loc;

            if (movement[(int)Step.Left]) x--;
            if (movement[(int)Step.Right]) x++;
            if (movement[(int)Step.Up]) y--;
            if (movement[(int)Step.Down]) y++;

            if (x != 0 && y != 0) loc = Hero.Location + new Vector(x,y)/Math.Sqrt(2);
            else loc = Hero.Location + new Vector(x,y);

            if (loc.X - 20 < 0) loc = new Vector(20,loc.Y);
            if (loc.Y - 20 < 0) loc = new Vector(loc.X, 20);
            if (loc.X + 20 > space.Width) loc = new Vector(space.Width - 20, loc.Y);          
            if (loc.Y + 20 > space.Height) loc = new Vector(loc.X, space.Height - 20);
           
            Hero = new Hero(loc);
        }

        internal void RotateHero(Point e)
        {
            var length = Math.Sqrt(Math.Pow(e.X - Hero.Location.X, 2) + Math.Pow(e.Y - Hero.Location.Y, 2));
            var angle = Math.Acos((e.X - Hero.Location.X) / length);
            if (e.Y <= Hero.Location.Y) angle = -angle;
            Hero = new Hero(angle, Hero.Location);
        }

        //public void MoveMonster(Size space, Step movement)
        //{

        //}

        public void Reset() => Hero = InitialHero;
    }
}