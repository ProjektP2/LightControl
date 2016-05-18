using System;
using LightControl;
using System.Collections.Generic;
using Triangulering;
using System.Diagnostics;
using System.Drawing;
using Quadtree;

namespace TreeStructure
{
    public abstract class Query
    {
        public IBoundable MapBound { get; set; }
        public ISearchable Tree { get; set; }
        
        public abstract Rectangle GetBound(Coords entityPosition, int witdh, int height);
        public abstract void SearchTree(Coords entityPosition, ref List<LightingUnit> list);
        public abstract Rectangle Bound { get; set; }
    }

    public class StartTreeSearch
    {
        private List<LightingUnit> _unitList;
        public StartTreeSearch()
        {
        }
        public List<LightingUnit> SearchQuery(Coords position, params Query[] searches)
        {
            _unitList = new List<LightingUnit>();
            foreach (Query search in searches)
            {
                search.SearchTree(position, ref _unitList);
            }
            return _unitList;
        }
    }
}
