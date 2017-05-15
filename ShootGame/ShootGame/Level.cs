﻿using System;
using System.Collections;
using System.Drawing;
using System.Linq.Expressions;
using static ShootGame.Properties.Resources;


namespace ShootGame
{
    public class Level
    {    
        public readonly string Name;
        public readonly Hero InitialHero;
        public Hero Hero;
        private const int StepLen = 3;

        public Level(string name, Hero hero)
        {
            Name = name;
            Hero = hero;
            InitialHero = hero;
        }

        //public bool IsCompleted => Enemy.Count == 0;
        public bool IsDead => Hero.Health <= 0;

        public Image GetImage(string e) => (Image)ResourceManager.GetObject(e);

        public void MoveHero(Size space, bool[] movement)
        {
            var x = 0;
            var y = 0;

            if (movement[(int)Step.Left]) x -= StepLen;
            if (movement[(int)Step.Right]) x += StepLen;
            if (movement[(int)Step.Up]) y -= StepLen;
            if (movement[(int)Step.Down]) y += StepLen;

            var loc = Hero.Location + new Vector(x, y) / (x != 0 && y != 0 ? Math.Sqrt(2) : 1);

            if (loc.X - 20 < 0) loc = new Vector(20, loc.Y);
            if (loc.Y - 20 < 0) loc = new Vector(loc.X, 20);
            if (loc.X + 20 > space.Width) loc = new Vector(space.Width - 20, loc.Y);          
            if (loc.Y + 20 > space.Height) loc = new Vector(loc.X, space.Height - 20);
           
            Hero.Location = loc;
        }

        public void RotateHero(Point e)
        {
            var length = Math.Sqrt(Math.Pow(e.X - Hero.Location.X, 2) + Math.Pow(e.Y - Hero.Location.Y, 2));
            var angle = Math.Acos((e.X - Hero.Location.X) / length);
            if (e.Y <= Hero.Location.Y) angle = -angle;
            Hero.Direction = angle;
        }
        public void Reset() => Hero = InitialHero;
    }
}