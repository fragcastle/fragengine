using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FragEd.Controllers {
    public interface IController<TENTITY> {
        void Create();

        void Read();

        void Update();

        void Delete();
    }
}
