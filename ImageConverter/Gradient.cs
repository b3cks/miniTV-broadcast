using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarRover
{
    class Gradient
    {
        private double angle;
        private double mag;

        public Gradient(double angle, double mag)
        {
            this.angle = angle;
            this.mag = mag;
        }
        
        public double Angle
        {
            set { angle = value; }
            get { return angle; }
        }

        public double Mag
        {
            set { mag = value; }
            get { return mag; }
        }

        
    }
}
