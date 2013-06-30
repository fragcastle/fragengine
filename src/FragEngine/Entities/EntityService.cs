using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Services;

namespace FragEngine.Entities
{
    public class EntityService : IEntityService
    {
        public TEntitytype SpawnEntity<TEntitytype>( Action<TEntitytype> configuration = null ) where TEntitytype : EntityBase, new()
        {
            var entity = new TEntitytype();

            if( configuration != null )
            {
                configuration( entity );
            }

            return entity;
        }
    }
}
