using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using FragEngine.Entities;
using FragEngine.IO;
using FragEngine.Layers;
using FragEngine.View;
using FragEngine.Collisions;
using FragEngine.Services;

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
            temp = DiskStorage.LoadFromDisk<Level>( temp.FilePath );

            return temp;
        }

        public static Level Load( FileInfo filePath )
        {
            var level = DiskStorage.LoadFromDisk<Level>( filePath.FullName );

            level.FilePath = filePath.FullName;

            // since we're loading a level we need to update the collision layer
            // get the collision service
            var collision = ServiceLocator.Get<ICollisionService>();
            if (level.CollisionLayer != null) {
                collision.SetCollisionMap(new CollisionMap(level));
            } else {
                collision.SetCollisionMap(null);
            }

            return level;
        }

        public Level()
        {
            // levels always have a collision layer
            MapLayers = new List<MapLayer>();

            Entities = new List<GameObject>();

            CollisionLayer = new CollisionLayer();
        }

        [DataMember]
        public List<GameObject> Entities { get; set; }

        [DataMember]
        public List<MapLayer> MapLayers { get; set; }

        [DataMember]
        public CollisionLayer CollisionLayer { get; set; }

        [DataMember]
        public string Name { get; set; }

        public void Save()
        {
            DiskStorage.SaveToDisk( FilePath, this );
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
