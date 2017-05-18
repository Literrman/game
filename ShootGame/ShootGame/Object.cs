using System.Collections.Generic;
using System.Drawing;

namespace ShootGame
{
    public class Object
    {
        public readonly Vector Location;
        public readonly double Direction;
        public readonly Bitmap ObjectIMG;

        public readonly HashSet<Object> Objects = new HashSet<Object>(); 

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
    }
}

enum Objects
{
    a,
    b,
    c,
    d
}