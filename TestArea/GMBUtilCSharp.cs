using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestArea
{
    static class GMBDmath
    {
        public static float GMBModf(float x, float y)
        {
            if (x < 0)
            {
                while (x < 0)
                {
                    x = y + x;
                }
                return x;
            }
            
            return x % y;
        }
    }
}
