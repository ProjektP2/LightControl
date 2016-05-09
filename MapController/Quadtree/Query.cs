using System;
using LightControl;
using System.Collections.Generic;
using Triangulering;
using System.Diagnostics;

namespace TreeStructure
{
    public abstract class Query : IBoundable
    {
        public Bounds MapBound { get; set; }
        public QuadTree Tree { get; set; }

        /*public Query(int width, int height, Bounds map, QuadTree tree)
        {
            MapBound = map;
            Tree = tree;
        }*/
        public abstract Bounds GetBound(Coords entityPosition, int witdh, int height);
        public abstract void SearchTree(Coords entityPosition, ref List<LightingUnit> list);
        public abstract void CalculateBoundCoords(Coords Position, out Coords TopLeft, out Coords BottomRight);
        public abstract Bounds Bound { get; set; }
    }

    public class StartTreeSearch
    {
        private List<LightingUnit> _unitList;
        private QuadTree _tree;
        public StartTreeSearch(QuadTree tree)
        {
            _tree = tree;
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
