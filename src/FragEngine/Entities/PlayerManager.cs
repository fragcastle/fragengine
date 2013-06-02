using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Entities
{
    public static class PlayerManager
    {
        public static Dictionary<PlayerIndex, Player> Players { get; set; }

        private static ContentManager _content = null;

        static PlayerManager()
        {
            Players = new Dictionary<PlayerIndex, Player>();
        }

        public static Player PlayerOne
        {
            get
            {
                return Players.ContainsKey(PlayerIndex.One) ? Players[ PlayerIndex.One ] : null;
            }
        }

        public static void AddPlayer( Player player )
        {
            Players.Add( player.Index, player );
        }

        public static void RemovePlayer( PlayerIndex index )
        {
            Players.Remove( index );
        }

        public static void Draw( SpriteBatch spriteBatch )
        {
            foreach(Player player in Players.Values )
            {
                player.Draw( spriteBatch );
            }
        }

        public static void Update( GameTime gameTime )
        {
            foreach ( Player player in Players.Values )
            {
                player.Update( gameTime );
            }
        }

        public static Player RandomPlayer()
        {
            var rand = new Random();
            return Players.ElementAt( rand.Next( 0, Players.Count ) ).Value;
        }
    }
}
