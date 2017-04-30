using System;
using System.Collections.Generic;
using static System.Windows.Forms.Form;

namespace ShootGame
{
    public class Levels
    {
        private static readonly Vector Location = new Vector(512, 374);
        private static readonly Hero Hero = new Hero(location: Location, health: 100, experiens: 0, direction: -0.5 * Math.PI);

        public static IEnumerable<Level> CreateLevels()
        {
            yield return new Level("background0", Hero);

            yield return new Level("background1", Hero);

            yield return new Level("background2", Hero);

            yield return new Level("background3", Hero);

            yield return new Level("background4", Hero);
        }
    }
}