using FragEngine.Maps;
using Microsoft.Xna.Framework;
using Should;
using Xunit;

namespace FragEngine.Tests.Maps
{
    public class MapDataDictionaryTests
    {
        public class GetCellLocation : MapDataDictionaryTests
        {
            [Fact]
            public void Should_Be_Able_To_Access_2D_Points_In_1D_Array()
            {
                var map = new MapDataDictionary
                {
                    Data = new int[]
                    {
                        -1, -1, -1, -1,
                        -1,  0,  0, -1,
                        -1,  0,  0, -1,
                        -1, -1, -1, -1,
                    },
                    Width = 4
                };

                var vector = map.GetCellLocation(1);
                vector.ShouldEqual(new Vector2(1, 0));

                var vector2 = map.GetCellLocation(10);
                vector2.ShouldEqual(new Vector2(2, 2));
            }
        }
    }
}
