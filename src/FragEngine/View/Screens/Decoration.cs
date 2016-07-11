using Microsoft.Xna.Framework;

namespace FragEngine.View.Screens
{
    public abstract class Decoration
    {
        private static int _idx;

        protected Decoration()
        {
            _idx++;
            Name = "Decoration" + _idx;
        }
        public string Name { get; set; }
        public int ZIndex { get; set; }

        public abstract void Draw(GameTime gameTime);
    }
}
