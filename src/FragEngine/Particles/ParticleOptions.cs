using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace FragEngine.Particles
{
    [DataContract]
    public class ParticleOptions
    {
        [DataMember]
        public Vector2 Velocity { get; set; }
        [DataMember]
        public Vector2 SetVelocity { get; set; }
        [DataMember]
        public Color[] Colors { get; set; }
        [DataMember]
        public Vector2 AbsoluteVelocity { get; set; }
    }
}
