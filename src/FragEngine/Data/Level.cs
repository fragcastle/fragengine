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
            Layers = new List<MapLayer>() { new CollisionLayer() };

            Entities = new List<EntityBase>();
        }

        [DataMember]
        public List<EntityBase> Entities { get; set; }

        [DataMember]
        public List<MapLayer> Layers { get; set; }

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

        public List<MapLayer> MapLayers()
        {
            return Layers.Where( l => ( l as CollisionLayer ) == null ).ToList();
        }

        public CollisionLayer CollisionLayer()
        {
            return (CollisionLayer)Layers.First( l => l is CollisionLayer );
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
