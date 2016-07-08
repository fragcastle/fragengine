using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Entities
{
    public class Void : GameObject
    {
        public override void Draw(SpriteBatch batch)
        {
            // voids do not draw
        }
    }
}
