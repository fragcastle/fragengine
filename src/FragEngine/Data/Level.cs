using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using FragEngine.Entities;
using FragEngine.IO;
using FragEngine.Layers;
using FragEngine.View;

namespace FragEngine.Data
{

    [DataContract]
    public class Level
    {

        public EventHandler OnDataChanged;
        public EventHandler OnPersisted;
        private string _filePath;

        public static Level Load( string levelName )
        {
            var temp = new Level() { Name = levelName };
            temp = Persistant.Load<Level>( temp.FilePath );

            return temp;
        }

        public static Level Load( FileInfo filePath )
        {
            var level = Persistant.Load<Level>( filePath.FullName );

            level.FilePath = filePath.FullName;

            return level;
        }

        public Level()
        {
            // levels always have a collision layer
            MapLayers = new List<MapLayer>();

            Entities = new List<Entity>();

            CollisionLayer = new CollisionLayer();
        }

        [DataMember]
        public List<Entity> Entities { get; set; }

        [DataMember]
        public List<MapLayer> MapLayers { get; set; }

        [DataMember]
        public CollisionLayer CollisionLayer { get; set; }

        [DataMember]
        public string Name { get; set; }

        public void Save()
        {
            Persistant.Persist( FilePath, this );
            if( OnPersisted != null )
            {
                OnPersisted( this, EventArgs.Empty );
            }
        }

        [IgnoreDataMember]
        public string FilePath
        {
            get { return String.IsNullOrWhiteSpace(_filePath) ? @"Data\" + Name + ".json" : _filePath; }
            set { _filePath = value; }
        }

        public void SetDirty()
        {
            if( OnDataChanged != null )
            {
                OnDataChanged( this, EventArgs.Empty );
            }
        }
    }
}
