using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfRobot
{
   public class Direction
    {
        public static void  GetAngle(out int newDirection,int angle, string keydown)
        {

            if (angle == (int)DirectionsEnums.N) newDirection = keydown == "L" ? 90 : 270;
            else if (angle == (int)DirectionsEnums.W) newDirection = keydown == "L" ? 180 : 0;
            else if (angle == (int)DirectionsEnums.S) newDirection = keydown == "L" ? 270 : 90;
            else if (angle == (int)DirectionsEnums.E) newDirection = keydown == "L" ? 0 : 180;
            else newDirection = 0;
             

        }
    }
}
