using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts;

public class AiScript : MonoBehaviour {

    public int monster_id;
    public float radius = 0.5f;

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

    private Vector2 nextTargetCoordinat;

    private MapNode startMapNode;       // Start Current Posisition of AI
    private MapNode nextTargetMapNode;  // Goal Posisition That AI should Go
    private List<MapNode> mWalkingCoordinateNode; // For Storing Which Node of Map that should passed
    private List<MapNode> closedListNode;
    private List<MapNode> openListNode;

    enum AiBehaviour{chasing,patroling,idle};
    private AiBehaviour mBehaviour;
    private bool haveReadTheMap;
    private GameObject mainPlayer;

    // Use this for initialization
    void Start()
    {
        mainPlayer  = GameObject.Find("MainPlayer");
        mBehaviour  = AiBehaviour.idle;
        controller  = GetComponent<CharacterController>();
        mAnimator   = GetComponent<Animator>();
        mAnimation  = GetComponent<Animation>();
        haveReadTheMap  = false;
        closedListNode  = new List<MapNode>();
        openListNode    = new List<MapNode>();
        mAnimation.wrapMode = WrapMode.Loop;
        mWalkingCoordinateNode = new List<MapNode>();
    }

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
        
        if (nextTargetCoordinat.x - radius > controller.transform.position.x) // go to right
            return 1;
        if (nextTargetCoordinat.x + radius < controller.transform.position.x) // go to left
            return 3;
        if (nextTargetCoordinat.y - radius > controller.transform.position.z) // go to up
            return 2;
        if (nextTargetCoordinat.y + radius < controller.transform.position.z) // go to down
            return 0;

        //movingState = 0;
        //updateAnimation();
        return faceDirection;
    }

    public bool isSeenPlayer() {
        // if location player and ai within in xx range and yy range
        
        setCurrentLocationAsStartNode();
        float playerX = mainPlayer.transform.position.x;
        float playerZ = mainPlayer.transform.position.z;
        int indexX = (int)Math.Round((playerX - indexMap * MazeDatabase.GetMaze[indexMap].GetLength(0)), MidpointRounding.AwayFromZero);
        int indexY = (int)Math.Round(playerZ, MidpointRounding.AwayFromZero);
        return false;
        //return (indexX >= startMapNode.getIndexX() - seenRange &&
        //    indexX <= startMapNode.getIndexX() + seenRange &&
        //    indexY >= startMapNode.getIndexY() - seenRange &&
        //    indexY <= startMapNode.getIndexY() + seenRange);
    }

    // A* Path
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
        if (indexX - 1 >= 0 )
        {
            if (mapNode[indexX - 1][indexY].isAvailablePath())
                neighbors.Add(mapNode[indexX - 1][indexY]);
        }
        if (indexX + 1 <= mapNode.GetLength(0))
        {
            if(mapNode[indexX + 1][indexY].isAvailablePath())
                neighbors.Add(mapNode[indexX + 1][indexY]);
        }
        if (indexY - 1 >= 0 )
        {
            if(mapNode[indexX][indexY - 1].isAvailablePath())
                neighbors.Add(mapNode[indexX][indexY - 1]);
        }
        if (indexY + 1 <= mapNode.GetLength(0))
        {
            if (mapNode[indexX][indexY + 1].isAvailablePath())
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

            if (currentNode == nextTargetMapNode)
            {
                BuildPathNode(nextTargetMapNode);
                return;
            }
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
            if (walkingNodeIndex == mWalkingCoordinateNode.Count)
            {

            }
            else if(walkingNodeIndex < mWalkingCoordinateNode.Count) {
                nextTargetCoordinat = mWalkingCoordinateNode[walkingNodeIndex + 1].getPosition();
                bool arrive = nextTargetCoordinat.x - radius < controller.transform.position.x && nextTargetCoordinat.x + radius > controller.transform.position.x;
                arrive = arrive && nextTargetCoordinat.y - radius < controller.transform.position.z && nextTargetCoordinat.y + radius > controller.transform.position.z;
                if (!arrive)
                {
                    int direction = getDiretionToGo();
                    int rotateDegree = (faceDirection - direction) * 90;
                    controller.transform.Rotate(Vector3.up, rotateDegree);
                    transform.Translate(0.0f, 0, getSpeedFactor());
                    faceDirection = direction;
                }
                else {
                    walkingNodeIndex++;
                    setCurrentLocationAsStartNode();
                }

                

            }

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
        int indexX = (int)Math.Round((transform.position.x- indexMap * MazeDatabase.GetMaze[indexMap].GetLength(0)), MidpointRounding.AwayFromZero);
        int indexY = (int)Math.Round(transform.position.z, MidpointRounding.AwayFromZero);
        startMapNode = mapNode[indexX][indexY];
    }

    public void setMainPlayerLocationAsTargetNode() {
        float playerX = GameObject.Find("MainPlayer").transform.position.x;
        float playerZ = GameObject.Find("MainPlayer").transform.position.z;
        int indexX = (int)Math.Round((playerX - indexMap * MazeDatabase.GetMaze[indexMap].GetLength(0)), MidpointRounding.AwayFromZero);
        int indexY = (int)Math.Round(playerZ, MidpointRounding.AwayFromZero);
        nextTargetMapNode = mapNode[indexX][indexY];
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
                transform.position = new Vector3(xAI + indexMap * MazeDatabase.GetMaze[indexMap].GetLength(0), 0, yAI);
                return ;
            }
        } 
    }
    void testingMovementManual() {
        if(Input.GetKey("i"))transform.Translate(0.0f, 0, getSpeedFactor());
        if (Input.GetKey("k")) transform.Translate(0.0f, 0, -getSpeedFactor());
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
                testingMovementManual();
                // Do the patroling and idle
                //if (mBehaviour == AiBehaviour.idle)
                //{
                //    if (idleTime >= maxIdleTime)
                //    {
                //        // change behaviour become patroling
                //        mBehaviour = AiBehaviour.patroling;
                //        speedFactor = walkSpeedFactor;
                //        idleTime = 0.0f;
                //        randomNextCordinate();
                //        setCurrentLocationAsStartNode();
                //        computePath();
                //        updateAnimation();

                //    }
                //    else {
                //        // do the idle
                //        idleTime += Time.deltaTime;
                //    }
                //}
                //else if (mBehaviour == AiBehaviour.patroling) {
                //    if (startMapNode == nextTargetMapNode)
                //    {
                //        // reaching the target location, change behaviour become idle
                //        mBehaviour = AiBehaviour.idle;
                //        idleTime = 0.0f;
                //        updateAnimation();
                //    }
                //    else {
                //        moving();
                //    }
                //} else if (mBehaviour == AiBehaviour.chasing) {
                //    if (startMapNode == nextTargetMapNode)
                //    {
                //        // reaching the target location, change behaviour
                //        mBehaviour = AiBehaviour.idle;
                //        speedFactor = walkSpeedFactor;
                //        idleTime = 0.0f;
                //        updateAnimation();
                //    }
                //    else
                //    {
                //        moving();
                //    }
                //}

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
