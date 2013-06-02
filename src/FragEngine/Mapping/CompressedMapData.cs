using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace FragEngine.Mapping
{
    public class CompressedMapData
    {

        public int this[Vector2 pos]
        {
            get { return this.GetTile( pos ); }
            set
            {
                var index = this.GetCellIndex( pos );
                Data[index] = value;
            }
        }

        public int this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        public int[] Data { get; set; }

        public int Width { get; set; }

        /// <summary>
        /// Converts a cell index in the <see cref="Data"/> array to a uncorrected vector.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector2 GetCellLocation(int index)
        {
            return new Vector2(
				(index%Width),
                (int)Math.Floor( (double)(index / Width) )
			);
        }

        /// <summary>
        /// Converts a given uncorrected vector to a index in the <see cref="Data"/> array
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public int GetCellIndex(int x, int y )
        {
            return GetCellIndex( new Vector2( x, y ) );
        }
        public int GetCellIndex(Vector2 position)
        {
            return (int)(( ( position.Y ) * Width ) + ( position.X ));
        }

        /// <summary>
        /// Converts a given uncorrected vector to a tile value in the <see cref="Data"/> array
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public int GetTile(Vector2 position)
        {
            return Data[ GetCellIndex( position ) ];
        }

        /// <summary>
        /// Iterates through the <see cref="Data"/> array, calling the passed action with the
        /// uncorrected map-location and the tile value.
        /// </summary>
        /// <param name="each"></param>
        public void EachCell(Action<Vector2, int> each)
        {
            int index = 0;
            foreach (var cell in Data)
            {
                each( GetCellLocation( index ), Data[ index ] );
                index++;
            }
        }

        /// <summary>
        /// Iterates thought the <see cref="Data"/> array, called the passed function with the
        /// uncorrected map-location and the tile value. Any integer returned will be set into
        /// the Data array.
        /// </summary>
        /// <param name="each"></param>
        public void EachCell(Func<Vector2, int, int> each)
        {
            int index = 0;
            foreach( var cell in Data )
            {
                Data[index] = each( GetCellLocation( index ), Data[ index ] );
                index++;
            }
        }

        public bool IsEmpty()
        {
            return Data.Length > 0;
        }
    }

    public static class CompressedMapDataExtensions
    {
        public static string ToJson( this CompressedMapData mapData )
        {
            var writer = new StringWriter();
            var jsonWriter = new JsonTextWriter( writer );

            jsonWriter.WriteStartObject();

            jsonWriter.WritePropertyName("Data");

            jsonWriter.WriteStartArray();
            mapData.Data.ToList().ForEach( jsonWriter.WriteValue );
            jsonWriter.WriteEndArray();

            jsonWriter.WritePropertyName("Width");
            jsonWriter.WriteValue(mapData.Width);

            jsonWriter.WriteEndObject();

            return writer.ToString();
        }
    }
}
