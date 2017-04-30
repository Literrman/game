using System;
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

        public bool IsCompleted => Hero.Location.Length < 20; /////////////////////////////////////////////////////////////////////////////////

        //public bool IsDead => (Hero.Location - Monster.Location).Length < 20;
        public Image GetImage(string e) => (Image)ResourceManager.GetObject(e);

        public void Move(Size spaceSize, Step step)
        {
            if (step == Step.None) return;
            var location = Vector.Zero;
            if (step == Step.Left || step == Step.Right)
                location += new Vector(Hero.Location + new Vector((double)step, 0).Normalize());
            if (step == Step.Down || step == Step.Up)
                location += new Vector(Hero.Location + new Vector(0, (double)step).Normalize());
            ///*var*/ location = step == Step.Left || step == Step.Right
            //    ? new Vector(Hero.Location + new Vector((double)step, 0).Normalize())
            Hero = new Hero(location);
        }

        //public void MoveMonster(Size spaceSize, Step step)
        //{

        //}

        public void Reset()
        {
            Hero = InitialHero;
        }        
    }
}