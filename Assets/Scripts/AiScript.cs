﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts;

public class AiScript : MonoBehaviour {

    public int monster_id;
    public float radius = 0.01f;

    private MapNode[][] mapNode;
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
        mAnimation  = GetComponent<Animation>();
        haveReadTheMap  = false;
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
            if (MazeDatabase.GetMaze[indexMap][y, x] == MazeGenerator.MAZEPATH)
            {
                nextTargetMapNode = mapNode[y][x];
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
        return faceDirection;
    }

    public bool isSeenPlayer() {
        // if location player and ai within in xx range and yy range
        
        //setCurrentLocationAsStartNode();
        //float playerX = mainPlayer.transform.position.x;
        //float playerZ = mainPlayer.transform.position.z;
        //int indexX = (int)Math.Round((playerX - indexMap * MazeDatabase.GetMaze[indexMap].GetLength(0)), MidpointRounding.AwayFromZero);
        //int indexY = (int)Math.Round(playerZ, MidpointRounding.AwayFromZero);
        return false;
        //return (indexX >= startMapNode.getIndexX() - seenRange &&
        //    indexX <= startMapNode.getIndexX() + seenRange &&
        //    indexY >= startMapNode.getIndexY() - seenRange &&
        //    indexY <= startMapNode.getIndexY() + seenRange);
    }

    public void moving() {
        if (mWalkingCoordinateNode.Count > 0)
        {
            if (walkingNodeIndex < mWalkingCoordinateNode.Count)
            {
                nextTargetCoordinat = mWalkingCoordinateNode[walkingNodeIndex + 1].getPosition();
                bool arriveX = nextTargetCoordinat.x - radius < controller.transform.position.x && nextTargetCoordinat.x + radius > controller.transform.position.x;
                bool arriveY = nextTargetCoordinat.y - radius < controller.transform.position.z && nextTargetCoordinat.y + radius > controller.transform.position.z;
                bool arrive = arriveX && arriveY;
                if (!arrive)
                {
                    int direction = getDiretionToGo();
                    int rotateDegree = (faceDirection - direction) * 90;
                    controller.transform.Rotate(Vector3.up, rotateDegree);
                    transform.Translate(0.0f, 0.0f, getSpeedFactor());
                    faceDirection = direction;
                }
                else
                {
                    float dX = nextTargetCoordinat.x - controller.transform.position.x;
                    float dY = nextTargetCoordinat.y - controller.transform.position.z;
                    float minX = nextTargetCoordinat.x - 0.1f;
                    float maxX = nextTargetCoordinat.x + 0.1f;
                    float minY = nextTargetCoordinat.y - 0.1f;
                    float maxY = nextTargetCoordinat.y + 0.1f;
                    //Debug.Log("Next node");
                    //Debug.Log("X1 = " + controller.transform.position.x + " , Y1 = " + controller.transform.position.z);
                    walkingNodeIndex++;
                    setCurrentLocationAsStartNode();
                }
            }

        }
    }
    
    public void setCurrentLocationAsStartNode() {
        //Debug.Log("MapNode Length = " + mapNode.Length);
        int indexX = (int)Math.Round((transform.position.x - indexMap * mapNode.Length), MidpointRounding.AwayFromZero);
        int indexY = (int)Math.Round( transform.position.z, MidpointRounding.AwayFromZero);
        startMapNode = mapNode[indexY][indexX];
    }

    public void setMainPlayerLocationAsTargetNode() {
        float playerX = mainPlayer.transform.position.x;
        float playerZ = mainPlayer.transform.position.z;
        int indexX = (int)Math.Round((playerX - indexMap * MazeDatabase.GetMaze[indexMap].GetLength(0)), MidpointRounding.AwayFromZero);
        int indexY = (int)Math.Round( playerZ, MidpointRounding.AwayFromZero);
        nextTargetMapNode = mapNode[indexY][indexX];
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
        for (int y = 0; y < MazeDatabase.GetMaze[indexMap].GetLength(0); y++){
            mapNode[y] = new MapNode[MazeDatabase.GetMaze[indexMap].GetLength(1)];
            for (int x = 0; x < MazeDatabase.GetMaze[indexMap].GetLength(1); x++){
                mapNode[y][x] = new MapNode(x,y,indexMap);
            }
        }
        AStarAlgorithm.setMapNode(mapNode);
    }

    void startIdle() {
        //Debug.Log("Start Idle");
        mBehaviour  = AiBehaviour.idle;
        speedFactor = walkSpeedFactor;
        idleTime    = 0.0f;
        updateAnimation();
    }
    void startPatroling() {
        //Debug.Log("Start Patroling");
        mBehaviour  = AiBehaviour.patroling;
        speedFactor = walkSpeedFactor;
        idleTime = 0.0f;
        setCurrentLocationAsStartNode();
        randomNextCordinate();
        walkingNodeIndex = 0;
        mWalkingCoordinateNode = AStarAlgorithm.computePath(startMapNode, nextTargetMapNode);
        updateAnimation();
    }
    void startChasing() {
        //Debug.Log("Start Chasing");
        mBehaviour  = AiBehaviour.chasing;
        speedFactor = runSpeedFactor;
        idleTime    = 0.0f;
        setCurrentLocationAsStartNode();
        setMainPlayerLocationAsTargetNode();
        walkingNodeIndex = 0;
        mWalkingCoordinateNode = AStarAlgorithm.computePath(startMapNode, nextTargetMapNode);
        updateAnimation();
    }
    void placeAIInStartPosition()
    {
        // read name of AI to decide index map => 1 - 6
        int xAI = 0;
        int yAI = 0;
        while (true) {
            xAI = UnityEngine.Random.Range(0, MazeDatabase.GetMaze[indexMap].GetLength(0));
            yAI = UnityEngine.Random.Range(0, MazeDatabase.GetMaze[indexMap].GetLength(1));
            if (MazeDatabase.GetMaze[indexMap][yAI, xAI] == MazeGenerator.MAZEPATH)
            {
                transform.position = new Vector3(xAI + indexMap * MazeDatabase.GetMaze[indexMap].GetLength(0), 0, yAI);
                return ;
            }
        } 
    }
    void testingMovementManual() {
        if(Input.GetKey("i"))transform.Translate(0.0f, 0, getSpeedFactor());
        if (Input.GetKey("k")) transform.Translate(0.0f, 0, -getSpeedFactor());
        if (Input.GetKey("r")) resetPlayer();
    }
    // Update is called once per frame
    void Update()
    {
        if (GlobalVariable.onPauseGame) return;
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
            testingMovementManual();
            if (isSeenPlayer())
            {
                switch (mBehaviour) {
                    case AiBehaviour.chasing:
                        if (startMapNode == nextTargetMapNode)
                        {
                            // reaching the target location, change behaviour
                            startIdle();
                            // reset the player and decrease the player gem?
                            resetPlayer();
                        }
                        else
                        {
                            moving();
                        }
                        break;
                    default:
                        startChasing();
                        break;
                }
               
            }
            else
            {
                // Do the patroling and idle
                switch (mBehaviour)
                {
                    case AiBehaviour.idle       :
                        if (idleTime >= maxIdleTime) startPatroling();
                        else idleTime += Time.deltaTime; // Iddle 
                        break;
                    case AiBehaviour.patroling  :
                    case AiBehaviour.chasing    :
                        if (startMapNode == nextTargetMapNode)startIdle();
                        else moving();
                        break;
                }
            }
        }
        


    }

    void resetPlayer()
    {
        // Decrease Gem into half
        GlobalVariable.CurrGemNumber = GlobalVariable.CurrGemNumber > 1?(int)Math.Round((double)(GlobalVariable.CurrGemNumber/2), MidpointRounding.AwayFromZero):0;
        PlayerMovementControl.resetPlayer = true;
    }
}
