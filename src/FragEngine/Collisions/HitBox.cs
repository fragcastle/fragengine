﻿using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace FragEngine.Collisions
{
    [DataContract]
    public struct HitBox
    {
        public HitBox(float width, float height)
        {
            Width = width;
            Height = height;
        }

        [DataMember]
        public float Height { get; set; }

        [DataMember]
        public float Width { get; set; }

        public static HitBox operator +( HitBox box, Vector2 vector )
        {
            return new HitBox { Height = box.Height + vector.Y, Width = box.Width + vector.X };
        }

        public static HitBox operator +( HitBox box, Rectangle rectangle )
        {
            return new HitBox { Height = box.Height + rectangle.Height, Width = box.Width + rectangle.Width };
        }

        public static HitBox operator -( HitBox box, Vector2 vector )
        {
            return new HitBox { Height = box.Height - vector.Y, Width = box.Width - vector.X };
        }

        public static HitBox operator -( HitBox box, Rectangle rectangle )
        {
            return new HitBox { Height = box.Height - rectangle.Height, Width = box.Width - rectangle.Width };
        }

        public static implicit operator Vector2( HitBox box )
        {
            return new Vector2( box.Width, box.Height );
        }

        public static implicit operator Rectangle( HitBox box )
        {
            return new Rectangle( 0, 0, (int)box.Width, (int)box.Height );
        }

        public static explicit operator HitBox( Vector2 vector )
        {
            return new HitBox { Width = vector.X, Height = vector.Y };
        }

        public static explicit operator HitBox( Rectangle rectangle )
        {
            return new HitBox { Width = rectangle.Width, Height = rectangle.Height };
        }
    }
}
