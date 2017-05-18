using System.Collections.Generic;

namespace ShootGame
{
    public class Object
    {
        public readonly Vector Location;
        public readonly double Direction;

        public readonly HashSet<Object> Objects = new HashSet<Object>(); 

        public Object(Vector location, double direction)
        {
            Location = location;
            Direction = direction;
        }

        public void CheckAcross()
        {
            foreach (var obj in Objects)
            {
                
            }
        }
    }
}