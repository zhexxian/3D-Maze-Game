using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapNode {
    private int indexX;
    private int indexY;
    private MapNode mParent;
    private bool availablePath;
    private Vector2 posisition;
    private float fValue,gValue,hValue;

    public MapNode(int indexX,int indexY,int indexMap) {
        this.indexX = indexX;
        this.indexY = indexY;
        this.mParent = null;
        this.availablePath = (MazeDatabase.GetMaze[indexMap][indexX, indexY] != MazeGenerator.MAZEPATH);
        this.posisition = new Vector2(indexX + indexMap * AiScript.widthBeetweenMap, indexY);
        resetCost();
    }
    public void resetCost() {
        this.fValue = this.gValue = this.hValue = -1.0f;
    }
    public void setParent(MapNode mParent) {
        this.mParent = mParent;
    }
    public MapNode getParent() {
        return mParent;
    }
    public bool isAvailablePath() {
        return availablePath;
    }
    public Vector2 getPosition() {
        return posisition;
    }
    public float getFvalue() {
        return fValue;
    }
    public float getGvalue() {
        return gValue;
    }
    public int getIndexX() {
        return indexX;
    }
    public int getIndexY(){
        return indexY;
    }
    public bool updateCost(float gValue , float hValue) {
        bool betterValue = false;
        if (this.gValue < 0.0) { betterValue = true; this.gValue = gValue; }
        if (this.hValue < 0.0) { betterValue = true; this.hValue = hValue; }
        if (betterValue)
        {
            fValue = gValue + hValue;
        }
        else {
            if (fValue > gValue + hValue) {
                betterValue = true;
                this.gValue = gValue;
                this.hValue = hValue;
                fValue = gValue + hValue;
            }
        }
        return betterValue;
    }
}

public class AiScript : MonoBehaviour {

    public static int widthBeetweenMap = 30;

    private MapNode[][] mapNode;
    private Animator mAnimator;
    private Animation mAnimation;
    private CharacterController controller;
    // Public for GUI parameter input
    public float maxIdleTime        = 3.0f;
    public float seenRange          = 2.0f;
    public float walkSpeedFactor    = 2.0f;
    public float runSpeedFactor     = 4.0f;

    private float speedFactor       = 2.0f;
    private float idleTime          = 0.0f;
    private int indexMap            = 1;  // 1-6 
    private int movingState         = 0;  // 0 - iddle   || 1 - walking   || 2 - running
    private int faceDirection       = 2;  // 0 - camera direction   || 1 - right direction   || 2 - forward direction   || 3 - left direction
    private int walkingNodeIndex    = 0;

    private Vector2 startCoordinat;
    private Vector2 nextTargetKoordinat;

    private MapNode startMapNode;
    private MapNode nextTargetMapNode;
    private List<Vector2> mWalkingCoordinatePath;
    private List<MapNode> mWalkingCoordinateNode; // open map node list
    private List<MapNode> closedListNode;
    private List<MapNode> openListNode;

    enum AiBehaviour{chasing,patroling,idle};
    private AiBehaviour mBehaviour;
    private bool haveReadTheMap;

    public float getSpeedFactor()
    {
        return (speedFactor * Time.deltaTime); 
    }

    public void randomNextCordinate() {
        //nextTargetKoordinat = new Vector2(Random.Range(30f,40f), Random.Range(-5f, 5f));
        int x = 0;
        int y = 0;
        while (true)
        {
            x = UnityEngine.Random.Range(0, MazeDatabase.GetMaze[indexMap].GetLength(0));
            y = UnityEngine.Random.Range(0, MazeDatabase.GetMaze[indexMap].GetLength(1));
            if (MazeDatabase.GetMaze[indexMap][x, y] == MazeGenerator.MAZEPATH)
            {
                nextTargetMapNode = mapNode[x][y];
                return;
            }
        }
    }

    public int getDiretionToGo()
    {
        float radius = 0.5f;
        if (nextTargetKoordinat.x - radius > controller.transform.position.x) // go to right
            return 1;
        if (nextTargetKoordinat.x + radius < controller.transform.position.x) // go to left
            return 3;
        if (nextTargetKoordinat.y - radius > controller.transform.position.z) // go to up
            return 2;
        if (nextTargetKoordinat.y + radius < controller.transform.position.z) // go to down
            return 0;

        //movingState = 0;
        //updateAnimation();
        return faceDirection;
    }

    public bool isSeenPlayer() {
        // if location player and ai within in xx range and yy range
        setCurrentLocationAsStartNode();
        float playerX = GameObject.Find("MainPlayer").transform.position.x;
        float playerZ = GameObject.Find("MainPlayer").transform.position.z;
        int indexX = (int)Math.Round((playerX - indexMap * AiScript.widthBeetweenMap), MidpointRounding.AwayFromZero);
        int indexY = (int)Math.Round(playerZ, MidpointRounding.AwayFromZero);
        return (indexX >= startMapNode.getIndexX() - seenRange &&
            indexX <= startMapNode.getIndexX() + seenRange &&
            indexY >= startMapNode.getIndexY() - seenRange &&
            indexY <= startMapNode.getIndexY() + seenRange);
    }

    public void resetAllNodeCost() {
        for (int x = 0; x < MazeDatabase.GetMaze[indexMap].GetLength(0); x++)
        {
            for (int y = 0; y < MazeDatabase.GetMaze[indexMap].GetLength(1); y++)
            {
                mapNode[x][y].resetCost(); ;
            }
        }
    }

    public float getHcostBetweenTwoNodes( MapNode start, MapNode goal ){
        float dx = start.getIndexX() - goal.getIndexX();
        float dy = start.getIndexY() - goal.getIndexY();
        if ( dx< 0 ) dx = -dx;
        if (dy < 0 ) dy = -dy;
        return dx + dy;
    }

    public MapNode getNodeFromOpenList() {
        MapNode minimum = null;
        foreach (MapNode node in openListNode) {
            if (minimum == null || node.getFvalue() < minimum.getFvalue()) {
                minimum = node;
            }
        }
        openListNode.Remove(minimum);
        return minimum;
    }

    public List<MapNode> findNodesNeighbor(MapNode node) {
        List<MapNode> neighbors = new List<MapNode>();
        neighbors.Clear();
        int indexX = node.getIndexX();
        int indexY = node.getIndexY();
        if (indexX - 1 >= 0 && mapNode[indexX - 1][indexY].isAvailablePath())
        {
            neighbors.Add(mapNode[indexX - 1][indexY]);
        }
        if (indexX + 1 <= mapNode.GetLength(1) && mapNode[indexX + 1][indexY].isAvailablePath())
        {
            neighbors.Add(mapNode[indexX + 1][indexY]);
        }
        if (indexY - 1 >= 0 && mapNode[indexX][indexY - 1].isAvailablePath())
        {
            neighbors.Add(mapNode[indexX][indexY - 1]);
        }
        if (indexY + 1 <= mapNode.GetLength(1) && mapNode[indexX][indexY + 1].isAvailablePath())
        {
            neighbors.Add(mapNode[indexX][indexY + 1]);
        }

        return neighbors;
    }

    public void BuildPathNode(MapNode node)
    {
        if (node == null) return;
        BuildPathNode(node.getParent());
        mWalkingCoordinateNode.Add(node);
    }

    public void computePath() {
        openListNode.Clear();
        closedListNode.Clear();
        resetAllNodeCost();
        bool search = true;
        float g, h;
        g = 0.0f;
        h = getHcostBetweenTwoNodes(startMapNode, nextTargetMapNode);
        startMapNode.updateCost(g, h);
        openListNode.Add(startMapNode);
        MapNode currentNode = null;
        List<MapNode> currentNodeNeightbors = new List<MapNode>();
        while (search)
        {
            currentNode = getNodeFromOpenList();
            closedListNode.Add(currentNode);

            if (currentNode == nextTargetMapNode) return;
            currentNodeNeightbors = findNodesNeighbor(currentNode);
            for (int a = 0; a < currentNodeNeightbors.Count; a++)
            {
                if (!closedListNode.Contains(currentNodeNeightbors[a]))
                {
                    g = currentNode.getGvalue() + 1;
                    h = getHcostBetweenTwoNodes(currentNodeNeightbors[a], nextTargetMapNode);
                    bool isBetter = currentNodeNeightbors[a].updateCost(g, h);
                    if (isBetter || currentNodeNeightbors[a].getParent() != null)
                    {
                        currentNodeNeightbors[a].setParent(currentNode);
                        openListNode.Add(currentNodeNeightbors[a]);
                    }
                }
            }
        }


    }

    public void moving() {
        if (mWalkingCoordinateNode.Count > 0)
        {
            //Real targetX = mPathPoints[mCurrentPathPointIndex+1].x;
            //Real targetZ = mPathPoints[mCurrentPathPointIndex+1].z;
            //Real rboX = mRobotNode->getPosition().x;
            //Real rboZ = mRobotNode->getPosition().z;
            //if(rboX == targetX && rboZ == targetZ){
            //robotIsMoving = false;
            //}
            //else{
            //Real movementX = 0.1;
            //Real movementZ = 0.1;
            //if(rboX > targetX ) movementX = movementX * -1;
            //if(rboZ > targetZ ) movementZ = movementZ * -1;
            //rboX = rboX + movementX;
            //rboZ = rboZ + movementZ;

        }
        //simple moving
        //float movementX = getSpeedFactor();
        //float movementZ = getSpeedFactor();
        //float playerX = nextTargetMapNode.getPosition().x;
        //float playerZ = nextTargetMapNode.getPosition().y;
        //if (playerX <= transform.position.x) movementX *= -1;
        //if (playerZ <= transform.position.z) movementZ *= -1;
        //transform.Translate(movementX, 0, movementZ);
        //setCurrentLocationAsStartNode();
    }
    
    public void setCurrentLocationAsStartNode() {
        int indexX = (int)Math.Round((transform.position.x- indexMap * AiScript.widthBeetweenMap), MidpointRounding.AwayFromZero);
        int indexY = (int)Math.Round(transform.position.z, MidpointRounding.AwayFromZero);
        startMapNode = mapNode[indexX][indexY];
    }

    public void setMainPlayerLocationAsTargetNode() {
        float playerX = GameObject.Find("MainPlayer").transform.position.x;
        float playerZ = GameObject.Find("MainPlayer").transform.position.z;
        int indexX = (int)Math.Round((playerX - indexMap * AiScript.widthBeetweenMap), MidpointRounding.AwayFromZero);
        int indexY = (int)Math.Round(playerZ, MidpointRounding.AwayFromZero);
        nextTargetMapNode = mapNode[indexX][indexY];
    }

    // Use this for initialization
    void Start()
    {
        mBehaviour  = AiBehaviour.idle;
        controller  = GetComponent<CharacterController>();
        mAnimator   = GetComponent<Animator>();
        mAnimation  = GetComponent<Animation>();
        haveReadTheMap = false;
        mAnimation.wrapMode = WrapMode.Loop;
        
        mWalkingCoordinatePath = new List<Vector2>();
        mWalkingCoordinateNode = new List<MapNode>();
        closedListNode = new List<MapNode>();
        openListNode    = new List<MapNode>();
    }

    void updateAnimation()
    {
        switch (mBehaviour)
        {
            case AiBehaviour.idle:
                //mAnimator.Play("Idle", -1, 0f); break;
                mAnimation.Play("idle");break;
            case AiBehaviour.patroling:
                //mAnimator.Play("Walk", -1, 0f); break;
                mAnimation.Play("walk"); break;
            case AiBehaviour.chasing:
                //mAnimator.Play("Run", -1, 0f); break;
                mAnimation.Play("run"); break;
        }
    }

    void initMapNode()
    {
        mapNode = new MapNode[MazeDatabase.GetMaze[indexMap].GetLength(0)][];
        for (int x = 0; x < MazeDatabase.GetMaze[indexMap].GetLength(0); x++){
            mapNode[x] = new MapNode[MazeDatabase.GetMaze[indexMap].GetLength(1)];
            for (int y = 0; y < MazeDatabase.GetMaze[indexMap].GetLength(1); y++){
                mapNode[x][y] = new MapNode(x,y,indexMap);
            }
        }
    }

    void placeAIInStartPosition()
    {
        // read name of AI to decide index map => 1 - 6
        int xAI = 0;
        int yAI = 0;
        while (true) {
            xAI = UnityEngine.Random.Range(0, MazeDatabase.GetMaze[indexMap].GetLength(0));
            yAI = UnityEngine.Random.Range(0, MazeDatabase.GetMaze[indexMap].GetLength(1));
            if (MazeDatabase.GetMaze[indexMap][xAI, yAI] == MazeGenerator.MAZEPATH)
            {
                transform.position = new Vector3(xAI + indexMap * widthBeetweenMap, 0, yAI);
                return ;
            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (!haveReadTheMap)
        {   // Try to read the map
            if (MazeDatabase.GetMaze[indexMap] != null)
            {
                haveReadTheMap = true;
                initMapNode();
                placeAIInStartPosition();
            }
        }
        else
        {
            if (isSeenPlayer())
            {
                if (mBehaviour == AiBehaviour.chasing)
                {
                    if (startMapNode == nextTargetMapNode)
                    {
                        // reaching the target location, change behaviour
                        mBehaviour  = AiBehaviour.idle;
                        speedFactor = walkSpeedFactor;
                        idleTime    = 0.0f;
                        updateAnimation();
                        // reset the player and decrease the player gem?
                    }
                    else {
                        moving();
                    }
                    
                }
                else {
                    // Start running and compute A* path finding
                    mBehaviour  = AiBehaviour.chasing;
                    speedFactor = runSpeedFactor;
                    idleTime    = 0.0f;
                    setCurrentLocationAsStartNode();
                    setMainPlayerLocationAsTargetNode();
                    computePath();
                    updateAnimation();
                }
            }
            else
            {
                // Do the patroling and idle
                if (mBehaviour == AiBehaviour.idle)
                {
                    if (idleTime >= maxIdleTime)
                    {
                        // change behaviour become patroling
                        mBehaviour  = AiBehaviour.patroling;
                        speedFactor = walkSpeedFactor;
                        idleTime    = 0.0f;
                        randomNextCordinate();
                        setCurrentLocationAsStartNode();
                        computePath();
                        updateAnimation();
                        
                    }
                    else {
                        // do the idle
                        idleTime += Time.deltaTime;
                    }
                }
                else if (mBehaviour == AiBehaviour.patroling) {
                    if (startMapNode == nextTargetMapNode)
                    {
                        // reaching the target location, change behaviour become idle
                        mBehaviour = AiBehaviour.idle;
                        idleTime = 0.0f;
                        updateAnimation();
                    }
                    else {
                        moving();
                    }
                }

                //if (movingState == 1)
                //{
                //    // Patroling random available path
                //    int direction = getDiretionToGo();
                //    int rotateDegree = (faceDirection - direction) * 90;
                //    controller.transform.Rotate(Vector3.up, rotateDegree);
                //    transform.Translate(0.0f, 0, getSpeedFactor());
                //    faceDirection = direction;
                //}
            }
        }
        


    }
}
