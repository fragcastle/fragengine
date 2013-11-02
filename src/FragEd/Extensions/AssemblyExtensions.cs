using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FragEngine.Entities;

namespace FragEd {
    public static class AssemblyExtensions {
        public static IEnumerable<Type> GetEntities( this Assembly asm )
        {
            var types = ( from type in asm.GetTypes()
                          where type.IsPublic && type.IsSubclassOf( typeof( GameObject ) ) && !type.IsAbstract
                          select type ).ToList();

            return types;
        }
    }
}
