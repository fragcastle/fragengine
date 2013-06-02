using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FragEngine.Entities
{
    public abstract class Player: EntityBase
    {

        public PlayerIndex Index { get; set; }

        public Player() : base( Vector2.Zero, Vector2.Zero )
        {

        }
    }
}
