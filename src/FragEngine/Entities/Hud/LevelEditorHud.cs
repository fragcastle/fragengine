using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace FragEngine.Entities.Hud
{
    public class LevelEditorHud : HudBase
    {

        private const int _xOffset = 10;
        private const int _yOffset = 10;

        protected Vector2 HudOffset = new Vector2( _xOffset, _yOffset );

        public LevelEditorHud(Rectangle windowSize) : base(windowSize)
        {
        }
    }
}
