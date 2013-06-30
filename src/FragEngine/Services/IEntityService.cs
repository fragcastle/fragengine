using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Entities;

namespace FragEngine.Services
{
    public interface IEntityService
    {
        TEntitytype SpawnEntity<TEntitytype>( Action<TEntitytype> configuration = null ) where TEntitytype : EntityBase, new();
    }
}
