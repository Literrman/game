﻿using System;
using System.Collections.Generic;
using static System.Windows.Forms.Form;
using static ShootGame.Hero;

namespace ShootGame
{
    public class Levels
    {
        public static readonly Vector StartLoc = new Vector(512, 374);
        private static readonly Hero Hero = new Hero(location: StartLoc, health: 100, experiens: 0, direction: 0);
        private static int lvl;

        public static readonly List<Level> MyLevels = new List<Level>
        {
            new Level("lvl1", Hero, 50),
            new Level("lvl2", Hero, 100),
            new Level("lvl3", Hero, 1000),
            new Level("lvl4", Hero, 2),
            new Level("lvl5", Hero, 2)
        };

        public static Level NextLvl() => lvl + 1 == MyLevels.Count ? MyLevels[lvl] : MyLevels[lvl++];

        
    }
}