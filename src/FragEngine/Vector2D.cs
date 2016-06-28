using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Point = System.Drawing.Point;

namespace FragEngine
{


    public class Vector2DConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            if (sourceType == typeof(Microsoft.Xna.Framework.Vector2))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var v = ((string)value).Split(',');
                return new Vector2D(float.Parse(v[0]), float.Parse(v[1]));
            }
            if (value is Microsoft.Xna.Framework.Vector2)
            {
                return new Vector2D(((Microsoft.Xna.Framework.Vector2)value).X, ((Microsoft.Xna.Framework.Vector2)value).Y);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return ((Vector2D)value).X + "," + ((Vector2D)value).Y;
            }
            if (destinationType == typeof(Microsoft.Xna.Framework.Vector2))
            {
                return new Vector2 { X = ((Vector2D)value).X, Y = ((Vector2D)value).Y };
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [DataContract]
    [TypeConverter(typeof(Vector2DConverter))]
    public struct Vector2D
    {
        [DataMember]
        public float X;
        [DataMember]
        public float Y;

        public Vector2D(float x = 0f, float y = 0f)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Microsoft.Xna.Framework.Vector2(Vector2D vec)
        {
            return new Vector2(vec.X, vec.Y);
        }
    }
}
