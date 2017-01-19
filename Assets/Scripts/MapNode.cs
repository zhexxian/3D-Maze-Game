using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class MapNode
    {
        private int indexX;
        private int indexY;
        private MapNode mParent;
        private bool availablePath;
        private Vector2 posisition;
        private float fValue, gValue, hValue;

        public MapNode(int indexX, int indexY, int indexMap)
        {
            this.indexX = indexX;
            this.indexY = indexY;
            this.mParent = null;
            this.availablePath = (MazeDatabase.GetMaze[indexMap][indexY, indexX] != MazeGenerator.MAZEWALL);
            this.posisition = new Vector2(indexX + indexMap * MazeDatabase.GetMaze[indexMap].GetLength(0), indexY);
            resetCost();
        }
        public void resetCost()
        {
            this.fValue = this.gValue = this.hValue = -1.0f;
            mParent = null;
        }
        public void setParent(MapNode mParent)
        {
            this.mParent = mParent;
        }
        public MapNode getParent()
        {
            return mParent;
        }
        public bool isAvailablePath()
        {
            return availablePath;
        }
        public Vector2 getPosition()
        {
            return posisition;
        }
        public float getFvalue()
        {
            return fValue;
        }
        public float getGvalue()
        {
            return gValue;
        }
        public float getHvalue()
        {
            return hValue;
        }
        public int getIndexX()
        {
            return indexX;
        }
        public int getIndexY()
        {
            return indexY;
        }
        public bool updateCost(float gValue, float hValue)
        {
            bool betterValue = false;
            if (this.gValue < 0.0) { betterValue = true; this.gValue = gValue; }
            if (this.hValue < 0.0) { betterValue = true; this.hValue = hValue; }
            if (betterValue)
            {
                fValue = gValue + hValue;
            }
            else
            {
                if (fValue > gValue + hValue)
                {
                    betterValue = true;
                    this.gValue = gValue;
                    this.hValue = hValue;
                    fValue = gValue + hValue;
                }
            }
            return betterValue;
        }
    }
}
