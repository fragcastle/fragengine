using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FragEd.Controllers;
using FragEngine.Data;

namespace FragEd.Data {
    public class ProjectChangeEventArgs : EventArgs
    {
        public string PropertyName { get; set; }
    }

    public class Project {

        public static EventHandler OnLoad;

        public static EventHandler OnLoadAssembly;
        public static EventHandler OnLoadLevel;
        public static EventHandler OnLoadContentDirectory;
        public static EventHandler OnCompleteLoadContentDirectory;
        public static event EventHandler<ProjectChangeEventArgs> OnChange;

        private List<Type> _entities = new List<Type>();
        private List<Level> _levels = new List<Level>();
        private List<DirectoryInfo> _contentDirectories = new List<DirectoryInfo>();

        public Project( ProjectConfiguration configuration = null ) {
            Load( configuration );
        }

        public List<Type> Entities {
            get { return _entities; }
            set { _entities = value; RaiseOnChangeEvent( "Entities" ); }
        }

        public List<Level> Levels {
            get { return _levels; }
            set { _levels = value; RaiseOnChangeEvent( "Levels" ); }
        }

        public List<DirectoryInfo> ContentDirectories {
            get { return _contentDirectories; }
            set { _contentDirectories = value; RaiseOnChangeEvent( "ContentDirectories" ); }
        }

        public ProjectConfiguration GetConfiguration() {
            var configuration = new ProjectConfiguration {
                GameAssemblies = Entities.Select( e => Path.GetFullPath( e.Assembly.Location ) ).Distinct(),
                GameContent = ContentDirectories.Select( dir => dir.FullName ),
                GameLevels = Levels.Select( l => l.FilePath )
            };

            return configuration;
        }

        private void Load( ProjectConfiguration configuration ) {
            if( configuration != null ) {
                LoadEntitiesFromAssemblies( configuration.GameAssemblies );

                // must load content _before_ we attempt to load the levels!
                LoadContentDirectories( configuration.GameContent );

                LoadLevels( configuration.GameLevels );
            }
        }

        private void LoadContentDirectories( IEnumerable<string> gameContent ) {
            var directories = new List<DirectoryInfo>();
            foreach( var contentDir in gameContent ) {
                var dir = new DirectoryInfo( contentDir );

                directories.Add( dir );
            }

            ContentDirectories = directories;
        }

        private void LoadLevels( IEnumerable<string> gameLevels ) {
            var levels = new List<Level>();
            foreach( var levelFile in gameLevels ) {
                var level = Level.Load( new FileInfo( levelFile ) );
                levels.Add( level );
            }
            Levels = levels;
        }

        private void LoadEntitiesFromAssemblies( IEnumerable<string> gameAssemblies ) {
            if( gameAssemblies == null )
                return;

            var entities = new List<Type>();
            foreach( var assemblyPath in gameAssemblies ) {
                var assembly = AssemblyResolutionManager.Current.Add( assemblyPath );
                var types = assembly.GetEntities();

                entities.AddRange( types );
            }

            Entities = entities;
        }

        private void RaiseOnChangeEvent( string propertyName ) {
            var handler = Project.OnChange;
            if( handler != null )
                handler( this, new ProjectChangeEventArgs { PropertyName = propertyName } );
        }
    }
}
