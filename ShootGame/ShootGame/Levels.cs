using System;
using System.Collections.Generic;
using static System.Windows.Forms.Form;

namespace ShootGame
{
    public class Levels
    {
        public static readonly Vector StartLoc = new Vector(512, 374);
        private static readonly Hero Hero = new Hero(location: StartLoc, health: 100, experiens: 0, direction: 0);

        public static IEnumerable<Level> CreateLevels()
        {
            yield return new Level("lvl1", Hero, 10);

            yield return new Level("lvl2", Hero, 20);

            yield return new Level("lvl3", Hero, 100);

            yield return new Level("lvl4", Hero, 100);

            yield return new Level("lvl5", Hero, 100);
        }

        public static List<Level> MyLevels = new List<Level>
        {
            new Level("lvl1", Hero, 2),
            new Level("lvl2", Hero, 20),
            new Level("lvl3", Hero, 100),
            new Level("lvl4", Hero, 100),
            new Level("lvl5", Hero, 100)
        };
    }
}