using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FragEd.Data
{
    [DataContract]
    public class ProjectConfiguration
    {

        public ProjectConfiguration()
        {
            GameAssemblies = new List<string>();
            GameLevels = new List<string>();
            GameContent = new List<string>();
        }

        [DataMember]
        public IEnumerable<string> GameAssemblies { get; set; }

        [DataMember]
        public IEnumerable<string> GameLevels { get; set; }

        [DataMember]
        public IEnumerable<string> GameContent { get; set; }
    }
}
