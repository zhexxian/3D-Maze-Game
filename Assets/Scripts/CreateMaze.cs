﻿using UnityEngine;
using System.Collections;

public class CreateMaze : MonoBehaviour
{
    public float wallHeight;
    public Material groundMaterial;
    public GameObject PrefabGemLand;
    public GameObject PrefabGemWater;
    public GameObject PrefabGemSky;
    public GameObject PrefabTeleportLand;
    public GameObject PrefabTeleportWater;
    public GameObject PrefabTeleportSky;
    public GameObject PrefabWallLand;
    public GameObject PrefabWallWater;
    public GameObject PrefabWallSky;
    public GameObject PrefabFloorLand;
    public GameObject PrefabFloorWater;
    public GameObject PrefabFloorSky;
    public GameObject PrefabFinishPointLand;
    public GameObject PrefabFinishPointWater;
    public GameObject PrefabFinishPointSky;
    public GameObject PrefabWallDefault;
    public GameObject PrefabFloorDefault;
    public GameObject PrefabGemDefault;
    public GameObject PrefabTeleportDefault;
    public GameObject PrefabFinishPointDefault;
    public AudioClip bgmLand;
    public AudioClip bgmWater;
    public AudioClip bgmSky;
    public GameObject PrefabAudioSource;
    public float yWallActive = 0;

    public float yWallLand;
    public float yWallWater;
    public float yWallSky;

    private GameObject audio;

    // Use this for initialization
    void Start()
    {
        audio = (GameObject)Instantiate(PrefabAudioSource);
        //initialize map
        int gemNumber = 0;

        const int mazesize = 10;
        //MazeDatabase.GenerateMaze (mazesize);
        //string[,] maze = MazeGenerator.CreateMaze(mazesize, 0, 0);

        const int NORTH = 1;
        const int WEST = 1;
        const int EAST = mazesize - 2;
        const int SOUTH = mazesize - 2;
        const int LOOPBEGIN = 1;
        const int LOOPEND = mazesize - 2;

        GameObject[] plane = new GameObject[7];
        GameObject[][,] cube = new GameObject[7][,];
        GameObject[][,] ground = new GameObject[7][,];
        BoxCollider[][,] cubeCollider = new BoxCollider[7][,];


        if (GlobalVariable.GetLevelData(GlobalVariable.CurrentLevel)[5] == "land")
        {
            PrefabWallDefault = PrefabWallLand;
            PrefabFloorDefault = PrefabFloorLand;
            PrefabGemDefault = PrefabGemLand;
            PrefabTeleportDefault = PrefabTeleportLand;
            PrefabFinishPointDefault = PrefabFinishPointLand;
            yWallActive = yWallLand;
        }
        else if (GlobalVariable.GetLevelData(GlobalVariable.CurrentLevel)[5] == "water")
        {
            PrefabWallDefault = PrefabWallWater;
            PrefabFloorDefault = PrefabFloorWater;
            PrefabGemDefault = PrefabGemWater;
            PrefabTeleportDefault = PrefabTeleportWater;
            PrefabFinishPointDefault = PrefabFinishPointWater;
            yWallActive = yWallWater;
        }
        else if (GlobalVariable.GetLevelData(GlobalVariable.CurrentLevel)[5] == "sky")
        {
            PrefabWallDefault = PrefabWallSky;
            PrefabFloorDefault = PrefabFloorSky;
            PrefabGemDefault = PrefabGemSky;
            PrefabTeleportDefault = PrefabTeleportSky;
            PrefabFinishPointDefault = PrefabFinishPointSky;
            yWallActive = yWallSky;
        }

        for (int a = 1; a <= 6; a++)
        {
            cube[a] = new GameObject[MazeDatabase.GetMaze[a].GetLength(0), MazeDatabase.GetMaze[a].GetLength(1)];
            ground[a] = new GameObject[MazeDatabase.GetMaze[a].GetLength(0), MazeDatabase.GetMaze[a].GetLength(1)];
            cubeCollider[a] = new BoxCollider[MazeDatabase.GetMaze[a].GetLength(0), MazeDatabase.GetMaze[a].GetLength(1)];
        }

        for (int a = 1; a <= 6; a++)
        {
            plane[a] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            plane[a].transform.position = new Vector3(a * MazeDatabase.GetMaze[a].GetLength(0) + 10, 0, 10);
            //maze[a].GetLength(0) --> horizontal; (1) --> vertical
            plane[a].transform.localScale = new Vector3(MazeDatabase.GetMaze[a].GetLength(0), 0.01f, MazeDatabase.GetMaze[a].GetLength(1));

            Material[] groundMaterialArr = new Material[1];
            groundMaterialArr[0] = groundMaterial;
            plane[a].GetComponent<MeshRenderer>().materials = groundMaterialArr;
            plane[a].GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(40, 40);

            for (int y = 0; y < MazeDatabase.GetMaze[a].GetLength(0); y++)
            {
                for (int x = 0; x < MazeDatabase.GetMaze[a].GetLength(1); x++)
                {
                    if (MazeDatabase.GetMaze[a][y, x] == MazeGenerator.MAZEWALL)
                    {
                        cube[a][y, x] = (GameObject)Instantiate(PrefabWallDefault);

                        cube[a][y, x].transform.position = new Vector3(x + a * MazeDatabase.GetMaze[a].GetLength(0), wallHeight / 2f + yWallActive, y);
                        //cube[a][y, x].transform.localScale = new Vector3(1, wallHeight, 1);
                        cube[a][y, x].transform.SetParent(plane[a].transform, true);

                        //Material[] wallMaterialArr = new Material[1];
                        //wallMaterialArr[0] = wallMaterial;
                        //cube[a][y, x].GetComponent<MeshRenderer>().materials = wallMaterialArr;

                        cubeCollider[a][y, x] = (BoxCollider)cube[a][y, x].AddComponent(typeof(BoxCollider));
                        cubeCollider[a][y, x].center = Vector3.zero;

                    }
                    if (MazeDatabase.GetMaze[a][y, x] == MazeGenerator.MAZEGEM)
                    {
                        GameObject go = (GameObject)Instantiate(PrefabGemDefault);
                        go.transform.localPosition = new Vector3(x + a * MazeDatabase.GetMaze[a].GetLength(0), 0.5f, y);
                        go.name = "gem_" + gemNumber;
                        gemNumber++;
                    }
                    if (MazeDatabase.GetMaze[a][y, x] == MazeGenerator.MAZEFINISH)
                    {
                        GameObject go = (GameObject)Instantiate(PrefabFinishPointDefault);
                        go.transform.localPosition = new Vector3(x + a * MazeDatabase.GetMaze[a].GetLength(0), 0.5f, y);
                        GlobalVariable.SetFinishNodeCoordinate(new int[2] { x + a * MazeDatabase.GetMaze[a].GetLength(0), y });
                    }
                    //if (MazeDatabase.GetMaze[a][y,x] == MazeGenerator.MAZEPATH)
                    //{
                    GameObject gofloor = (GameObject)Instantiate(PrefabFloorDefault);
                    gofloor.transform.localPosition = new Vector3(x + a * MazeDatabase.GetMaze[a].GetLength(0), 0, y);
                    //}
                }
            }
        }
        GlobalVariable.MaxGemNumber = gemNumber;
        GlobalVariable.mGem = new GameObject[gemNumber];
        GlobalVariable.RequiredGemNumber = gemNumber / 2;
        GlobalVariable.nonActiveGem.Clear();
        for (int g = 0; g < gemNumber; g++)
        {
            GlobalVariable.mGem[g] = GameObject.Find("gem_" + g);
        }
        //create maze  teleport spot

        if (GlobalVariable.CurrentLevel >= 0)
        {
            for (int a = 1; a <= 6; a++)
            {
                for (int z = 1; z < MazeDatabase.GetMaze[a].GetLength(0) - 1; z++)
                {
                    for (int x = 1; x < MazeDatabase.GetMaze[a].GetLength(1) - 1; x++)
                    {
                        int[] teleportPoint = MazeDatabase.GetTeleportPoint(a, z, x);
                        if (teleportPoint != null)
                        {
                            int des_a = teleportPoint[0];
                            int des_y = teleportPoint[1];
                            int des_x = teleportPoint[2];
                            GameObject go1 = (GameObject)Instantiate(PrefabTeleportDefault);
                            go1.transform.localPosition = new Vector3(a * MazeDatabase.GetMaze[a].GetLength(0) + x, 0.16f, z);
                        }
                    }
                }
            }
        }
        
        if (GlobalVariable.CurrentLevel<=1)
        {
            audio.GetComponent<AudioSource>().PlayOneShot(bgmLand);
        }
        if (GlobalVariable.CurrentLevel == 2)
        {
            audio.GetComponent<AudioSource>().PlayOneShot(bgmWater);
        }
        if (GlobalVariable.CurrentLevel == 3)
        {
            audio.GetComponent<AudioSource>().PlayOneShot(bgmSky);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!audio.GetComponent<AudioSource>().isPlaying)
        {
            if (GlobalVariable.CurrentLevel <= 1)
            {
                audio.GetComponent<AudioSource>().PlayOneShot(bgmLand);
            }
            if (GlobalVariable.CurrentLevel == 2)
            {
                audio.GetComponent<AudioSource>().PlayOneShot(bgmWater);
            }
            if (GlobalVariable.CurrentLevel == 3)
            {
                audio.GetComponent<AudioSource>().PlayOneShot(bgmSky);
            }
        }
    }
}
