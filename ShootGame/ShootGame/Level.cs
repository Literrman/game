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
            if (step == Step.Left && Hero.Location.X != 0)
                {
                location += new Vector(Hero.Location + new Vector((double)step, 0).Normalize());
                Hero = new Hero(location);
                Hero.Direction -= Math.PI / 2;
                return;
                }
            if (step == Step.Right && Hero.Location.X != spaceSize.Width)
                {
                location += new Vector(Hero.Location + new Vector((double)step, 0).Normalize());
                Hero = new Hero(location);
                Hero.Direction += Math.PI / 2;
                return;
            }
            if (step == Step.Down && Hero.Location.Y != spaceSize.Height)
                {
                location += new Vector(Hero.Location + new Vector(0, (double)step).Normalize());
                Hero = new Hero(location);
                Hero.Direction += Math.PI;
                return;
            }
            if (step == Step.Up && Hero.Location.Y != 0)
                {
                location += new Vector(Hero.Location + new Vector(0, (double)step).Normalize());
                Hero = new Hero(location);
                Hero.Direction += 2*Math.PI;
                return;
                //не работает поворот вверх
            }
            if (step == Step.DiagoanlRight && Hero.Location.X != spaceSize.Width && Hero.Location.Y != 0)
                {
                location += new Vector(Hero.Location + new Vector((-1) * (double)step, (double)step).Normalize());
                Hero = new Hero(location);
                Hero.Direction += Math.PI/4;
                return;
            }
            if (step == Step.DiagonalLeft && Hero.Location.X != 0 && Hero.Location.Y != 0)
                {
                location += new Vector(Hero.Location + new Vector((-1) * (double)step, (-1) * (double)step).Normalize());
                Hero = new Hero(location);
                Hero.Direction -= Math.PI/4;
                return;
            }
            /*
             * Довольно странное дело, я вроде бы и проверяю условия дохождения и вроде они работают, но почему тогда движения совершаются?
             * */
            return;
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