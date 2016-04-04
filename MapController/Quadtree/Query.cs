using System;
using LightControl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulering;

namespace TreeStructure
{
    abstract class Query
    {
        public abstract Bounds GetBound(Coords entityPosition, int witdh, int height);
        public abstract void SearchTree(Coords entityPosition, ref List<LightingUnit> list);
    }

    class StartTreeSearch
    {
        private List<LightingUnit> _unitList;
        public List<LightingUnit> SearchQuery(Coords position, params Query[] searches)
        {
            _unitList = new List<LightingUnit>();
            foreach (var item in searches)
            {
                item.SearchTree(position, ref _unitList);
            }
            return _unitList;
        }
    }

    class RadiusSearchQuery : Query
    {
        int _radius;
        int _width, _height;
        private Bounds _circleBound;
        private List<LightingUnit> _lightUnitList;
        private Bounds _mapBound;
        private QuadTree _tree;
        public RadiusSearchQuery(int Radius, Bounds Mapbound, QuadTree tree)
        {
            _radius = Radius;
            _width = _height = _radius;
            _mapBound = Mapbound;
            _tree = tree;
        }
        public override Bounds GetBound(Coords entityPosition, int width, int height)
        {
            Bounds Bound = new Bounds(entityPosition.x, entityPosition.y, width, height);
            return Bound;
        }

        public override void SearchTree(Coords entityPosition, ref List<LightingUnit> list)
        {
            _circleBound = GetBound(entityPosition, _width, _height);
            _tree.GetLightUnitInBound(ref list, _circleBound);
        }
    }

    class VectorSearchQuery : Query
    {
        public VectorSearchQuery(Bounds MapBound, QuadTree tree)
        {
        }
        public override Bounds GetBound(Coords entityPosition, int width, int height)
        {
            Bounds Bound = new Bounds(entityPosition.x, entityPosition.y, width, height);
            return Bound;
        }

        public override void SearchTree(Coords entityPosition, ref List<LightingUnit> list)
        {
            throw new NotImplementedException();
        }
    }
}
