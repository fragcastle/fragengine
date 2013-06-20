using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FragEd.Controllers;
using FragEngine;
using FragEngine.Data;
using FragEngine.Entities;

namespace FragEd.Data
{
    public class Project
    {

        public static EventHandler OnLoad;

        public static EventHandler OnLoadAssembly;
        public static EventHandler OnLoadLevel;
        public static EventHandler OnLoadContentDirectory;
        public static EventHandler OnCompleteLoadContentDirectory;

        public Project( ProjectConfiguration configuration = null )
        {
            Entities = new List<Type>();
            Levels = new List<Level>();
            ContentDirectories = new List<DirectoryInfo>();

            Load( configuration );
        }

        public List<Type> Entities { get; set; }

        public List<Level> Levels { get; set; }

        public List<DirectoryInfo> ContentDirectories { get; set; }

        public ProjectConfiguration GetConfiguration()
        {
            var configuration = new ProjectConfiguration
                {
                    GameAssemblies = Entities.Select( e => Path.GetFullPath( e.Assembly.Location ) ).Distinct(),
                    GameContent = ContentDirectories.Select( dir => dir.FullName ),
                    GameLevels = Levels.Select( l => l.FilePath )
                };

            return configuration;
        }

        private void Load( ProjectConfiguration configuration )
        {
            if( OnLoad != null )
            {
                OnLoad( this, EventArgs.Empty );
            }

            if( configuration != null )
            {
                LoadEntitiesFromAssemblies( configuration.GameAssemblies );

                // must load content _before_ we attempt to load the levels!
                LoadContentDirectories( configuration.GameContent );
                
                LoadLevels( configuration.GameLevels );
            }

        }

        private void LoadContentDirectories(IEnumerable<string> gameContent)
        {
            foreach( var contentDir in gameContent )
            {
                var dir = new DirectoryInfo( contentDir );

                ContentDirectories.Add(dir);

                if( OnLoadContentDirectory != null )
                {
                    OnLoadContentDirectory( dir, EventArgs.Empty );
                }
            }

            if( OnCompleteLoadContentDirectory != null ) {
                OnCompleteLoadContentDirectory( this, EventArgs.Empty );
            }
        }

        private void LoadLevels( IEnumerable<string> gameLevels )
        {
            foreach( var levelFile in gameLevels )
            {
                var level = Level.Load( new FileInfo(levelFile) );
                Levels.Add( level );

                if( OnLoadLevel != null )
                {
                    OnLoadLevel( level, EventArgs.Empty );
                }
            }
        }

        private void LoadEntitiesFromAssemblies( IEnumerable<string> gameAssemblies )
        {
            if( gameAssemblies == null ) return;
            foreach( var assemblyPath in gameAssemblies )
            {
                var assembly = AssemblyResolutionManager.Current.Add( assemblyPath );
                var types = assembly.GetEntities();

                Entities.AddRange( types );

                if( OnLoadAssembly != null )
                {
                    OnLoadAssembly( assembly, EventArgs.Empty );
                }
            }
        }
    }
}
