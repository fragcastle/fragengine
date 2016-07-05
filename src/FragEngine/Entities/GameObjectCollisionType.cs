using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragEngine.Entities
{
    public enum GameObjectCollisionStyle : int
    {
        Never,
        Lite,
        Passive,
        Active,
        Fixed
    }
}
