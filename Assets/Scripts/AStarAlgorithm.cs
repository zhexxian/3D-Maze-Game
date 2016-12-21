using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class AStarAlgorithm
    {
        private static List<MapNode> closedListNode = new List<MapNode>();
        private static List<MapNode> openListNode = new List<MapNode>();
        private static List<MapNode> AiPathNode = new List<MapNode>();
        private static MapNode[][] mapNode;

        // Should call this
        public static void setMapNode(MapNode[][] mapNode) {
            AStarAlgorithm.mapNode = mapNode;
        }

        public static List<MapNode> computePath(MapNode startNode, MapNode goalNode) {
            resetAllNodeCost();
            AiPathNode.Clear();
            openListNode.Clear();
            closedListNode.Clear();
            bool search = true;
            float g = 0.0f;
            float h = getHcostBetweenTwoNodes(startNode, goalNode);
            startNode.updateCost(g, h);
            openListNode.Add(startNode);
            MapNode currentNode = null;
            List<MapNode> currentNodeNeightbors = new List<MapNode>();
            while (search)
            {
                currentNode = getOptimalNodeFromOpenList();
                closedListNode.Add(currentNode);

                if (currentNode == goalNode)
                {
                    BuildPathNode(goalNode);
                    break;
                }
                currentNodeNeightbors = findNodesNeighbor(currentNode);
                for (int a = 0; a < currentNodeNeightbors.Count; a++)
                {
                    if (!closedListNode.Contains(currentNodeNeightbors[a]))
                    {
                        g = currentNode.getGvalue() + 1;
                        h = getHcostBetweenTwoNodes(currentNodeNeightbors[a], goalNode);
                        bool isBetter = currentNodeNeightbors[a].updateCost(g, h);
                        if (isBetter || currentNodeNeightbors[a].getParent() != null)
                        {
                            currentNodeNeightbors[a].setParent(currentNode);
                            openListNode.Add(currentNodeNeightbors[a]);
                        }
                    }
                }
            }

            return AiPathNode;
        }
        private static MapNode getOptimalNodeFromOpenList() {
            MapNode optimalNode = null;

            return optimalNode;
        }

        private static List<MapNode> findNodesNeighbor(MapNode node)
        {
            List<MapNode> neighbors = new List<MapNode>();
            neighbors.Clear();
            int indexX = node.getIndexX();
            int indexY = node.getIndexY();
            if (indexX - 1 >= 0)
            {
                if (mapNode[indexX - 1][indexY].isAvailablePath())
                    neighbors.Add(mapNode[indexX - 1][indexY]);
            }
            if (indexX + 1 <= mapNode.GetLength(0))
            {
                if (mapNode[indexX + 1][indexY].isAvailablePath())
                    neighbors.Add(mapNode[indexX + 1][indexY]);
            }
            if (indexY - 1 >= 0)
            {
                if (mapNode[indexX][indexY - 1].isAvailablePath())
                    neighbors.Add(mapNode[indexX][indexY - 1]);
            }
            if (indexY + 1 <= mapNode.GetLength(0))
            {
                if (mapNode[indexX][indexY + 1].isAvailablePath())
                    neighbors.Add(mapNode[indexX][indexY + 1]);
            }

            return neighbors;
        }

        private static void BuildPathNode(MapNode node)
        {
            if (node == null) return;
            BuildPathNode(node.getParent());
            AiPathNode.Add(node);
        }

        private static float getHcostBetweenTwoNodes(MapNode start, MapNode goal)
        {
            float dx = start.getIndexX() - goal.getIndexX();
            float dy = start.getIndexY() - goal.getIndexY();
            if (dx < 0) dx = -dx;
            if (dy < 0) dy = -dy;
            return dx + dy;
        }

        private static void resetAllNodeCost()
        {
            for (int x = 0; x < MazeDatabase.GetMaze[0].GetLength(0); x++)
            {
                for (int y = 0; y < MazeDatabase.GetMaze[0].GetLength(1); y++)
                {
                    mapNode[x][y].resetCost(); ;
                }
            }
        }

    }

    
}
