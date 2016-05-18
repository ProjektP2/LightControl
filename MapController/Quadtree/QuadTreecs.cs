using System;
using System.Collections.Generic;
using Triangulering;
using LightControl;
using System.Diagnostics;
using System.Drawing;
using Quadtree;

namespace TreeStructure
{
    public class QuadTree : ISearchable, IBoundable
    {
        static int MaxNodes = 4;
        static int MaxObjects = 2;
        static int MaxNodeLevel = 6;

        public QuadTree[] nodes = new QuadTree[MaxNodes];

        private List<LightingUnit> LightingUnits;
        public List<QuadTreeNode> QuadNodesList = new List<QuadTreeNode>();
        private int currentLevel = 0;

        public Rectangle Bound { get; set; }

        private enum State
        {
            TopLeft, TopRight, BottomLeft, BottomRight
        }

        public QuadTree(Rectangle bound)
        {
            this.Bound = bound;
        }

        public void CreateQuadTree(List<LightingUnit> list)
        {
            foreach (var item in list)
            {
                InsertNode(new QuadTreeNode(item));
            }
        }

        private void InsertNode(QuadTreeNode node)
        {
            int index = -1;
            if (nodes[0] != null)
            {
                index = GetNodeIndex(node);
                if (index != -1)
                {
                    nodes[index].InsertNode(node);
                    return;
                }
            }
            QuadNodesList.Add(node);
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
                    if (index != -1)
                    {
                        nodes[index].InsertNode(QuadNodesList[i]);
                        QuadNodesList.RemoveAt(i);
                    }
                    else
                        i++;
                }
            }
        }

        private void Split()
        {
            int subWidth = Bound.Width / 2;
            int subHeight = Bound.Height / 2;
            int x = (int)Bound.X;
            int y = (int) Bound.Y;

            int increasedNodeLevel = ++currentLevel;
            
            Rectangle boundTL = new Rectangle(new Point(x,y), new Size(subWidth, subHeight));
            Rectangle boundTR = new Rectangle(new Point(x + subWidth, y), new Size(subWidth, subHeight));
            Rectangle boundBL = new Rectangle(new Point(x, y + subHeight), new Size(subWidth, subHeight));
            Rectangle boundBR = new Rectangle(new Point(x + subWidth, y + subHeight), new Size(subWidth, subHeight));

            nodes[0] = new QuadTree(boundTL);
            nodes[1] = new QuadTree(boundTR);
            nodes[2] = new QuadTree(boundBL);
            nodes[3] = new QuadTree(boundBR);
            
        }

        private int GetNodeIndex(QuadTreeNode node)
        {
            double quadWidthMid = Bound.X + Bound.Width / 2;
            double quadHeightMid = Bound.Y + Bound.Height / 2;

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

        public void GetLightUnitInBound(ref List<LightingUnit> list, Rectangle Bound)
        {
            if (nodes[0] != null && this.Bound.IntersectsWith(Bound))
            {
                foreach (var item in nodes)
                {
                    if (item.Bound != null && item.Bound.IntersectsWith(Bound))
                    {
                        if (item.QuadNodesList.Count == 0)
                            item.GetLightUnitInBound(ref list, Bound);
                        else
                            AddNewUnitsToList(ref item.QuadNodesList, ref list);
                    }
                }
            }
        }
        private void AddNewUnitsToList(ref List<QuadTreeNode> list, ref List<LightingUnit> unitList)
        {
            foreach (QuadTreeNode listItem in list)
            {
                if (unitList.Contains(listItem.LightUnit) == false)
                {
                    unitList.Add(listItem.LightUnit);
                }
            }
        }
    }
}
