using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class AStarAlgorithm
    {
        private static List<MapNode> closedListNode = new List<MapNode>();
        private static List<MapNode> openListNode = new List<MapNode>();
        private static List<MapNode> AiPathNode = new List<MapNode>();
        private static List<MapNode> currentNodeNeightbors = new List<MapNode>();
        private static MapNode[][] mapNode;

        // Should call this
        public static void setMapNode(MapNode[][] _mapNode) {
            mapNode = _mapNode;
            Debug.Log("Init Set Map Node : " + mapNode.Length + " - " + mapNode[0].Length);

        }

        public static List<MapNode> computePath(MapNode startNode, MapNode goalNode) {
            resetAllNodeCost();
            AiPathNode.Clear();
            openListNode.Clear();
            closedListNode.Clear();
            currentNodeNeightbors.Clear();
            float g = 0.0f;
            float h = getHcostBetweenTwoNodes(startNode, goalNode);
            Debug.Log("Start node : " + startNode.getIndexX() + "," + startNode.getIndexY());
            Debug.Log("Goal node : " + goalNode.getIndexX() + "," + goalNode.getIndexY());
            startNode.updateCost(g, h);
            openListNode.Add(startNode);
            MapNode currentNode = null;
            for(int i = 0; i < mapNode.Length * mapNode[0].Length; i++)
            {
                currentNode = getOptimalNodeFromOpenList();
                closedListNode.Add(currentNode);

                if (currentNode == goalNode)
                {
                    Debug.Log("Find A Way");
                    BuildPathNode(goalNode);
                    return AiPathNode;
                    //break;
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
                            if(!openListNode.Contains(currentNodeNeightbors[a]))
                                openListNode.Add(currentNodeNeightbors[a]);
                        }
                    }
                }
            }
            Debug.Log("Fail to find A Way");
        return AiPathNode;
        }
        private static MapNode getOptimalNodeFromOpenList() {
            MapNode optimalNode = openListNode[0];
            for (int a = 0; a < openListNode.Count; a++) {
                if (openListNode[a].getFvalue() <= optimalNode.getFvalue()) optimalNode = openListNode[a];
            }
            openListNode.Remove(optimalNode);
            return optimalNode;
        }

        private static List<MapNode> findNodesNeighbor(MapNode node)
        {
            List <MapNode> neighbors = new List<MapNode>();
            neighbors.Clear();
            int indexX = node.getIndexX();
            int indexY = node.getIndexY();
            if (indexX - 1 > 0)
            {
                if (mapNode[indexY][indexX - 1].isAvailablePath())
                    neighbors.Add(mapNode[indexY][indexX - 1]);
            }
            if (indexX + 1 < mapNode.Length)
            {
                if (mapNode[indexY][indexX + 1].isAvailablePath())
                    neighbors.Add(mapNode[indexY][indexX + 1]);
            }
            if (indexY - 1 > 0)
            {
                if (mapNode[indexY - 1][indexX].isAvailablePath())
                    neighbors.Add(mapNode[indexY - 1][indexX]);
            }
            if (indexY + 1 <= mapNode.Length)
            {
                if (mapNode[indexY + 1][indexX].isAvailablePath())
                    neighbors.Add(mapNode[indexY + 1][indexX]);
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
            for (int y = 0; y < mapNode.Length; y++)
            {
                for (int x = 0; x < mapNode[0].Length; x++)
                {
                    mapNode[y][x].resetCost(); ;
                }
            }
        }

    }

    
}
