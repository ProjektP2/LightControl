using System;
using System.Collections.Generic;
using Triangulering;
using LightControl;

namespace TreeStructure
{
    class QuadTree
    {
        static int MaxNodes = 4;
        static int MaxObjects = 2;
        static int MaxNodeLevel = 6;

        // *** OBS Bruges Kun Til Debug OBS ***
        private int interations = 0;
        private bool Searched = false;

        private QuadTree Parent = null;
        private QuadTree[] nodes = new QuadTree[MaxNodes];

        private List<LightingUnit> LightingUnits = new List<LightingUnit>();
        private List<QuadTreeNode> QuadNodesList = new List<QuadTreeNode>();
        private int currentLevel = 0;

        // *** OBS Bounds Class skal laves *** 
        Bounds bound;
        private Coords nodeCenter;
        private bool LeafNode = false;

        private enum State
        {
            TopLeft, TopRight, BottomLeft, BottomRight
        }

        public QuadTree(Bounds bound, bool leaf, QuadTree parent)
        {
            this.bound = bound;
            this.LeafNode = leaf;
            this.Parent = parent;
        }

        public void InsertNode(QuadTreeNode node)
        {
            int index = -1;
            if (nodes[0] != null)
            {
                index = GetNodeIndex(node);
                //Console.WriteLine("index " + index);
                if (index != -1)
                {
                    nodes[index].InsertNode(node);
                    return;
                }
            }
            QuadNodesList.Add(node);
            LeafNode = true;
            int numLightsInList = QuadNodesList.Count;
            if (numLightsInList > MaxObjects && MaxNodeLevel > currentLevel)
            {
                if (nodes[0] == null)
                    Split();

                int i = 0;
                index = -1;
                while (i < QuadNodesList.Count)
                {
                    index = GetNodeIndex(QuadNodesList[i]);
                    //Console.WriteLine("index " + index);
                    if (index != -1)
                    {
                        nodes[index].InsertNode(QuadNodesList[i]);
                        QuadNodesList.RemoveAt(i);
                    }
                    else
                        i++;
                }
                LeafNode = false;
            }
        }

        private void Split()
        {
            int subWidth = bound.Width / 2;
            int subHeight = bound.Height / 2;
            int x = (int) bound.x;
            int y = (int) bound.y;

            int increasedNodeLevel = ++currentLevel;

            Bounds boundTL = new Bounds(x, y, subWidth, subHeight);
            Bounds boundTR = new Bounds(x + subWidth, y, subWidth, subHeight);
            Bounds boundBL = new Bounds(x, y +  subHeight, subWidth, subHeight);
            Bounds boundBR = new Bounds(x + subWidth, y + subHeight, subWidth, subHeight);

            nodes[0] = new QuadTree(boundTL, true, null);
            nodes[1] = new QuadTree(boundTR, true, null);
            nodes[2] = new QuadTree(boundBL, true, null);
            nodes[3] = new QuadTree(boundBR, true, null);
            
        }

        private int GetNodeIndex(QuadTreeNode node)
        {
            double quadWidthMid = bound.x + bound.Width / 2;
            double quadHeightMid = bound.y + bound.Height / 2;

            return GetObjectState(quadWidthMid, quadHeightMid, node);
        }

        private int GetObjectState(double xMid, double yMid, QuadTreeNode node)
        {
            int index = -1;
            bool top, bottom, left, right;
            double y, x;
            x = node.LightUnit.x; y = node.LightUnit.y;
            top = y < yMid;
            bottom = y > yMid;
            left = x < xMid;
            right = x > xMid;
            if (top && left) index = (int)State.TopLeft;
            else if (top && right) index = (int)State.TopRight;
            else if (bottom && left) index = (int)State.BottomLeft;
            else if (bottom && right) index = (int)State.BottomRight;

            return index;
        }

        public List<LightingUnit> RadiusSearchQuery(Coords entityPosition, double Radius)
        {
            Bounds circleBound = GetCircleBound(entityPosition, Radius);
            GetLightUnitInBound(ref LightingUnits, circleBound);
            return LightingUnits;
        }
        private Bounds GetCircleBound(Coords entityPosition, double Radius)
        {
            int width = (int)Radius;
            int height = (int)Radius;
            Bounds circleBound = new Bounds(entityPosition.x, entityPosition.y, width, height);
            return circleBound;
        }
        private void GetLightUnitInBound(ref List<LightingUnit> list, Bounds circleBound)
        {
            if (nodes[0] != null && bound.Intersects(circleBound))
            {
                foreach (var item in nodes)
                {
                    if (item != null && item.bound.Intersects(circleBound))
                    {
                        if(item.QuadNodesList.Count == 0)
                           item.GetLightUnitInBound(ref list, circleBound);
                        else
                            foreach (var listItem in item.QuadNodesList)
                            {
                                if (list.Contains(listItem.LightUnit) == false)
                                {
                                    list.Add(listItem.LightUnit);
                                }
                            }
                    }
                }
            }
        }

        private void PrintBounds(Bounds one)
        {
            Console.WriteLine($"one {one.x} {one.y} {one.Width} {one.Height} {one.BottomRightX} {one.BottomRightY} {one.TopLeftX} {one.TopLeftY}");
        }

        public void Print()
        {
            /*foreach (var item in QuadNodesList)
            {
                //Console.WriteLine(item.LightUnit.x + " " + item.LightUnit.y);
                Console.WriteLine(bound.x + " " + bound.y);
            }
            if (nodes[0] != null)
                nodes[0].Print();
            if (nodes[1] != null)
                nodes[1].Print();
            if (nodes[2] != null)
                nodes[2].Print();
            if (nodes[3] != null)
                nodes[3].Print(); */
            Console.WriteLine(nodes[1].bound.y + " "+ nodes[1].bound.x);
        }

    }
}
