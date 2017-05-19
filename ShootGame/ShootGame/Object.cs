using System.Collections.Generic;
using System.Drawing;
using static ShootGame.Hero;

namespace ShootGame
{
    public class Object
    {
        public readonly Vector Location;
        public readonly double Direction;
        public readonly Bitmap ObjectIMG;

        public readonly HashSet<Object> ObjectsSet = new HashSet<Object>(); 

        public Object(Vector location, double direction, Bitmap image)
        {
            Location = location;
            Direction = direction;
            ObjectIMG = image;
        }

        //public void CheckAcross()
        //{
        //    foreach (var obj in Objects)
        //    {

        //    }
        //}

        public static readonly Dictionary<string, List<Object>> Objects = new Dictionary<string, List<Object>>
        {
            ["lvl1"] =
            {
                new Object(new Vector(100, 300), 0, GetImage("obj_1")),
                new Object(new Vector(800, 800), 0, GetImage("obj_2")),
                new Object(new Vector(400, 400), 0, GetImage("obj_2")),
                new Object(new Vector(800, 400), 0, GetImage("obj_2")),
                new Object(new Vector(400, 800), 0, GetImage("obj_2")),
            },
            ["lvl2"] =
            {
                new Object(new Vector(100, 300), 0, GetImage("obj_1")),
                new Object(new Vector(800, 800), 0, GetImage("obj_2")),
                new Object(new Vector(400, 400), 0, GetImage("obj_2")),
                new Object(new Vector(800, 400), 0, GetImage("obj_2")),
                new Object(new Vector(400, 800), 0, GetImage("obj_2")),
            },
            ["lvl3"] =
            {
                new Object(new Vector(100, 300), 0, GetImage("obj_1")),
                new Object(new Vector(800, 800), 0, GetImage("obj_2")),
                new Object(new Vector(400, 400), 0, GetImage("obj_2")),
                new Object(new Vector(800, 400), 0, GetImage("obj_2")),
                new Object(new Vector(400, 800), 0, GetImage("obj_2")),
            },
            ["lvl4"] =
            {
                new Object(new Vector(100, 300), 0, GetImage("obj_1")),
                new Object(new Vector(800, 800), 0, GetImage("obj_2")),
                new Object(new Vector(400, 400), 0, GetImage("obj_2")),
                new Object(new Vector(800, 400), 0, GetImage("obj_2")),
                new Object(new Vector(400, 800), 0, GetImage("obj_2")),
            },
            ["lvl5"] =
            {
                new Object(new Vector(100, 300), 0, GetImage("obj_1")),
                new Object(new Vector(800, 800), 0, GetImage("obj_2")),
                new Object(new Vector(400, 400), 0, GetImage("obj_2")),
                new Object(new Vector(800, 400), 0, GetImage("obj_2")),
                new Object(new Vector(400, 800), 0, GetImage("obj_2")),
            }
        };
    }


}

//enum Objects
//{
//    a,
//    b,
//    c,
//    d
//}