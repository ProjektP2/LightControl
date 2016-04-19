using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightControl;
using Triangulering;

namespace TreeStructure
{
    public class QuadTreeNode
    {
        public LightingUnit LightUnit;
        public QuadTreeNode(LightingUnit lightObject)
        {
            this.LightUnit = lightObject;
        }
    }
}
