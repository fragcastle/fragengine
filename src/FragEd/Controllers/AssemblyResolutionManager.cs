using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FragEd.Controllers
{
    public class AssemblyResolutionManager
    {

        private List<Assembly> _assemblies;

        public static readonly AssemblyResolutionManager Current = new AssemblyResolutionManager();

        public AssemblyResolutionManager()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;

            _assemblies = new List<Assembly>();
        }

        public Assembly Add( string path )
        {
            var asm = Assembly.LoadFrom( path );
            _assemblies.Add(asm);

            return asm;
        }

        private Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach(var asm in _assemblies)
            {
                if( asm.FullName.StartsWith(args.Name))
                {
                    return asm;
                }
            }
            return null;
        }
    }
}
