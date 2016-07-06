using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine
{
    public enum FontAlignment
    {
        LEFT,
        RIGHT,
        CENTER
    }

    public class Font
    {
        private const string Newline = "\n";

        public const int FirstChar = 32;
        public List<float> WidthMap { get; set; }
        public float Alpha { get; set; }
        public float LetterSpacing { get; set; }
        public float LineSpacing { get; set; }
        public float Height { get; set; }
        public List<float> Indices { get; set; }

        public Texture2D FontTexture { get; private set; }

        public Font(string texturePath) : this(ContentCacheManager.GetTextureFromResource(texturePath))
        {
            
        }
        public Font(Texture2D texture)
        {
            WidthMap = new List<float>();
            Indices = new List<float>();
            FontTexture = texture;

            Initialize();
        }

        private void Initialize()
        {
            Height = FontTexture.Height - 1;
            var rawData = new Color[FontTexture.Width * FontTexture.Height];
            FontTexture.GetData(rawData);
            var offset = FontTexture.Width*(FontTexture.Height - 1);
            rawData = rawData.Skip(offset).ToArray();
            var currentWidth = 0;
            var x = 0;
            for (x = 0; x < rawData.Length; x++)
            {
                if (rawData[x].A > 127)
                {
                    currentWidth++;
                }
                else if (rawData[x].A < 128 && currentWidth > 0)
                {
                    WidthMap.Add(currentWidth);
                    Indices.Add(x - currentWidth);
                    currentWidth = 0;
                }
            }
            WidthMap.Add(currentWidth);
            Indices.Add(x - currentWidth);
        }

        private float WidthForLine(string text)
        {
            var width = 0f;
            for (var i = 0; i < text.Length; i++)
            {
                width += WidthMap[text[i] - FirstChar] + LetterSpacing;
            }
            return width;
        }

        private float DrawChar(SpriteBatch spriteBatch, char c, Vector2 pos)
        {
            var viewPort = new Rectangle((int)Indices[c - FirstChar], 0, (int)WidthMap[c - FirstChar], (int)Height);
            var destination = new Rectangle(pos.ToPoint(), new Point(viewPort.Width, viewPort.Height));
            spriteBatch.Draw(FontTexture, destination, viewPort, new Color(Color.White, Alpha));
            return WidthMap[c] + LetterSpacing;
        }

        public void Draw(SpriteBatch spriteBatch, string text, Vector2 pos, FontAlignment align = FontAlignment.LEFT)
        {
            if (text.Contains(Newline))
            {
                var lines = text.Split(Newline[0]);
                var lineHeight = Height + LineSpacing;
                for (var i = 0; i < lines.Length; i++)
                {
                    Draw(spriteBatch, lines[i], pos.SetY(pos.Y + i * lineHeight), align);
                }
                return;
            }

            if (align == FontAlignment.RIGHT || align == FontAlignment.CENTER)
            {
                var width = WidthForLine(text);
                pos = pos.SetX(pos.X - (align == FontAlignment.CENTER ? width/2 : width));
            }

            spriteBatch.Begin();
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                pos = pos.SetX(pos.X + DrawChar(spriteBatch, c, pos));
            }
            spriteBatch.End();
        }

        public float WidthForString(string text)
        {
            // Multiline?
            if (text.Contains(Newline))
            {
                var lines = text.Split(Newline[0]);
                var width = 0f;
                for (var i = 0; i < lines.Length; i++)
                {
                    width = Math.Max(width, WidthForLine(lines[i]));
                }
                return width;
            }
            return WidthForLine(text);
        }

        public float HeightForString(string text)
        {
            return text.Split(Newline[0]).Length * (Height + LineSpacing);
        }

        public Vector2 DimensionsForString(char c)
        {
            return new Vector2(WidthForString("" + c), HeightForString("" + c));
        }

        public Vector2 DimensionsForString(string text)
        {
            return new Vector2(WidthForString(text), HeightForString(text));
        }
    }
}
