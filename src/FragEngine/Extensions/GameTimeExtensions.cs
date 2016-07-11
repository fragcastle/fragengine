using Microsoft.Xna.Framework;

namespace FragEngine
{
    public static class GameTimeExtensions
    {

        /// <summary>
        /// Gets the fraction of seconds that have elapsed since the last Update() call.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public static float GetGameTick( this GameTime gameTime )
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
