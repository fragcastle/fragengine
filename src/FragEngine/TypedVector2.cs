using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragEngine
{
    public class Vector2<T>
    {
        public T X { get; set; }
        public T Y { get; set; }

        public Vector2()
        {
            
        }

        public Vector2(T x, T y)
        {
            X = x;
            Y = y;
        }
    }
}
