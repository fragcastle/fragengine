using System.Collections.Generic;
using FragEngine.Layers;

namespace FragEngine.Data
{
    public class Level
    {
        private List<Layer> _layers = new List<Layer>(); 

        public Level()
        {
            Layers = new List<Layer>();
        }

        public List<Layer> Layers
        {
            get
            {
                _layers.Sort((a, b) => a.Order < b.Order ? -1 : 1);
                return _layers;
            }
            set
            {
                _layers = value;
            }
        }

        public string Name { get; set; }
    }
}
