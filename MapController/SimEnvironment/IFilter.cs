using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapController.SimEnvironment
{
    interface IFilter<T>
    {
        bool Predicate(T L);
    }
}
