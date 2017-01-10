using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts;

public class AiScript : MonoBehaviour
{
    private MapNode[][] mapNode;
    private Animation mAnimation;
    private CharacterController controller;

    // Public for GUI parameter input
    public float radius = 0.01f;
    public float maxIdleTime = 3.0f;
    public float maxChasingTime = 5.0f;
    public float walkSpeedFactor = 2.0f;
    public float runSpeedFactor = 2.0f;
    public float catchRange = 0.72f;
    public float seenRange = 2.0f;

    private float speedFactor = 2.0f;
    private float idleTime = 0.0f;
    private float rechasingTime = 0.0f;
    private int indexMap = 1;  // 1-6 
    private int faceDirection = 2;  // 0 - camera direction   || 1 - right direction   || 2 - forward direction   || 3 - left direction
    private int walkingNodeIndex = 0;
   
    private MapNode startMapNode;       // Start Current Posisition of AI
    private MapNode nextTargetMapNode;  // Goal Posisition That AI should Go
    private Vector2 nextTargetCoordinat;
    private List<MapNode> mWalkingCoordinateNode; // For Storing Which Node of Map that should passed

    enum AiBehaviour { chasing, patroling, idle };
    private AiBehaviour mBehaviour;
    private bool haveReadTheMap;
    private GameObject mainPlayer;

    // Use this for initialization
    void Start()
    {
        mainPlayer = GameObject.Find("MainPlayer");
        mBehaviour = AiBehaviour.idle;
        controller = GetComponent<CharacterController>();
        mAnimation = GetComponent<Animation>();
        rechasingTime = maxChasingTime;
        haveReadTheMap = false;
        mAnimation.wrapMode = WrapMode.Loop;
        mWalkingCoordinateNode = new List<MapNode>();
        if (GlobalVariable.CurrentLevel == 0) {
            startIdle();
        }
    }

    public float getSpeedFactor()
    {
        return (speedFactor * Time.deltaTime);
    }

    public void randomNextCordinate()
    {
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

    public bool isSeenPlayer()
    {
        // if location player and ai within in xx range and yy range
        setCurrentLocationAsStartNode();
        float playerX = mainPlayer.transform.position.x;
        float playerZ = mainPlayer.transform.position.z;
        int indexX = (int)Math.Round((playerX - indexMap * MazeDatabase.GetMaze[indexMap].GetLength(0)), MidpointRounding.AwayFromZero);
        int indexY = (int)Math.Round(playerZ, MidpointRounding.AwayFromZero);
        return (indexX >= startMapNode.getIndexX() - seenRange &&
            indexX <= startMapNode.getIndexX() + seenRange &&
            indexY >= startMapNode.getIndexY() - seenRange &&
            indexY <= startMapNode.getIndexY() + seenRange &&
            MazeDatabase.GetMaze[indexMap][indexY, indexX] != MazeGenerator.MAZESTART);
    }

    public void moving()
    {
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
                    walkingNodeIndex++;
                    setCurrentLocationAsStartNode();
                }
            }

        }
    }

    public void setCurrentLocationAsStartNode()
    {
        
        int indexX = (int)Math.Round((transform.position.x - indexMap * mapNode.Length), MidpointRounding.AwayFromZero);
        int indexY = (int)Math.Round(transform.position.z, MidpointRounding.AwayFromZero);
        startMapNode = mapNode[indexY][indexX];
    }

    public void setMainPlayerLocationAsTargetNode()
    {
        float playerX = mainPlayer.transform.position.x;
        float playerZ = mainPlayer.transform.position.z;
        int indexX = (int)Math.Round((playerX - indexMap * MazeDatabase.GetMaze[indexMap].GetLength(0)), MidpointRounding.AwayFromZero);
        int indexY = (int)Math.Round(playerZ, MidpointRounding.AwayFromZero);
        nextTargetMapNode = mapNode[indexY][indexX];
    }

    void updateAnimation()
    {
        switch (mBehaviour)
        {
            case AiBehaviour.idle:
                //mAnimator.Play("Idle", -1, 0f); break;
                mAnimation.Play("idle"); break;
            case AiBehaviour.patroling:
                //mAnimator.Play("Walk", -1, 0f); break;
                mAnimation.Play("walk"); break;
            case AiBehaviour.chasing:
                //mAnimator.Play("Run", -1, 0f); break;
                //mAnimation.Play("run"); break;
                mAnimation.Play("walk"); break;
        }
    }

    void initMapNode()
    {
        mapNode = new MapNode[MazeDatabase.GetMaze[indexMap].GetLength(0)][];
        for (int y = 0; y < MazeDatabase.GetMaze[indexMap].GetLength(0); y++)
        {
            mapNode[y] = new MapNode[MazeDatabase.GetMaze[indexMap].GetLength(1)];
            for (int x = 0; x < MazeDatabase.GetMaze[indexMap].GetLength(1); x++)
            {
                mapNode[y][x] = new MapNode(x, y, indexMap);
            }
        }
        AStarAlgorithm.setMapNode(mapNode);
    }

    void startIdle()
    {
        //Debug.Log("Start Idle");
        mBehaviour = AiBehaviour.idle;
        speedFactor = walkSpeedFactor;
        idleTime = 0.0f;
        updateAnimation();
    }
    void startPatroling()
    {
        //Debug.Log("Start Patroling");
        mBehaviour = AiBehaviour.patroling;
        speedFactor = walkSpeedFactor;
        idleTime = 0.0f;
        setCurrentLocationAsStartNode();
        randomNextCordinate();
        walkingNodeIndex = 0;
        mWalkingCoordinateNode = AStarAlgorithm.computePath(startMapNode, nextTargetMapNode);
        updateAnimation();
    }
    void startChasing()
    {
        //Debug.Log("Start Chasing");
        mBehaviour = AiBehaviour.chasing;
        speedFactor = runSpeedFactor;
        idleTime = 0.0f;
        rechasingTime = maxChasingTime;
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
        while (true)
        {
            xAI = UnityEngine.Random.Range(0, MazeDatabase.GetMaze[indexMap].GetLength(0));
            yAI = UnityEngine.Random.Range(0, MazeDatabase.GetMaze[indexMap].GetLength(1));
            if (MazeDatabase.GetMaze[indexMap][yAI, xAI] == MazeGenerator.MAZEPATH)
            {
                transform.position = new Vector3(xAI + indexMap * MazeDatabase.GetMaze[indexMap].GetLength(0), 0, yAI);
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVariable.onPauseGame || PlayerMovementControl.isPlayerFinish) return;
        if (!haveReadTheMap)
        {   // Try to read the map
            if (MazeDatabase.GetMaze[1] != null)
            {
                haveReadTheMap = true;
                initMapNode();
				if (GlobalVariable.CurrentLevel == 0) {
					transform.position = new Vector3(9, 0, 1);
				}
				else
				{
					placeAIInStartPosition();
				}
            }
        }
        else
        {
            if (GlobalVariable.CurrentLevel == 0) return;
            if (indexMap != GlobalVariable.getIndexMap()) {
                // Realocate Ai to the new location in the new map side
                indexMap = GlobalVariable.getIndexMap();
                initMapNode();
                placeAIInStartPosition();
                startIdle();
            }
            
            if (checkCatchPlayer())
            {
                startIdle();
                resetPlayer();
            }
            if (isSeenPlayer())
            {
                switch (mBehaviour)
                {
                    case AiBehaviour.chasing:
                        if (rechasingTime <= 0 || startMapNode == nextTargetMapNode)
                        {
                            startChasing();
                        }
                        else
                        {
                            moving();
                            rechasingTime -= Time.deltaTime;
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
                    case AiBehaviour.idle:
                        if (idleTime >= maxIdleTime) startPatroling();
                        else idleTime += Time.deltaTime; // Iddle 
                        break;
                    case AiBehaviour.patroling:
                    case AiBehaviour.chasing:
                        if (startMapNode == nextTargetMapNode) startIdle();
                        else moving();
                        break;
                }
            }
        }



    }

    void regeneratedGem(int number)
    {
        if (number > 0 && GlobalVariable.nonActiveGem.Count >= number) {
            for (int x = 0; x < number; x++) {
                String name = GlobalVariable.nonActiveGem[0];
                name = name.Substring(4);
                int index = Int32.Parse(name);
                Debug.Log(index);
                GlobalVariable.nonActiveGem.RemoveAt(0);
                GlobalVariable.mGem[index].SetActive(true);
            }
        }

    }

    void resetPlayer()
    {
        // Decrease Gem into half
        int delta = GlobalVariable.CurrGemNumber > 1 ? (int)Math.Round((double)(GlobalVariable.CurrGemNumber / 2), MidpointRounding.AwayFromZero) : 0;
        delta = GlobalVariable.CurrGemNumber - delta;
        GlobalVariable.CurrGemNumber = GlobalVariable.CurrGemNumber > 1 ? (int)Math.Round((double)(GlobalVariable.CurrGemNumber / 2), MidpointRounding.AwayFromZero) : 0;
        PlayerMovementControl.resetPlayer = true;
        // generate gem
        regeneratedGem(delta);
    }

    bool checkCatchPlayer()
    {
        float playerX = mainPlayer.transform.position.x;
        float playerZ = mainPlayer.transform.position.z;
        float distanceX = playerX - transform.position.x;
        float distanceZ = playerZ - transform.position.z;
        distanceX = distanceX < 0 ? -distanceX : distanceX;
        distanceZ = distanceZ < 0 ? -distanceZ : distanceZ;
        int indexX = (int)Math.Round((playerX - indexMap * MazeDatabase.GetMaze[indexMap].GetLength(0)), MidpointRounding.AwayFromZero);
        int indexY = (int)Math.Round(playerZ, MidpointRounding.AwayFromZero);
        return (distanceX < catchRange && distanceZ < catchRange && MazeDatabase.GetMaze[indexMap][indexY, indexX] != MazeGenerator.MAZESTART);
    }
}
