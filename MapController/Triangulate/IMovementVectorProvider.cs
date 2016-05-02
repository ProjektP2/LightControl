using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;

namespace Triangulering
{
    public interface IMovementVectorProvider
    {
        Coords GetMovementVector(Occupant occupant);
    }
}
