﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;
    public float PerlinNoise;
    public GameObject[] Obstacles;

    private static Random.State seedGenerator;
    // seed to generate seed 
    private static int seedGeneratorSeed = 1;
    private static bool seedGeneratorInitialised = false;

    // Use this for initialization
    void Start ()
    {
        Random.InitState(GenerateSeed());
        int rand = Random.Range(10, 20);

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        CreateShape();
        UpdateMesh();
        CreateObstacles(rand);
        GetComponent<MeshCollider>().sharedMesh = mesh;
	}

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * PerlinNoise, z * PerlinNoise) * 2f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
                
            }
            vert++;
        }
        
       
    }
	
    void UpdateMesh()
    {
       
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public static int GenerateSeed()
    {
        // remember the old seed
        var temp = Random.state;

        // initialise generator if not generated
        if (!seedGeneratorInitialised)
        {
            Random.InitState(seedGeneratorSeed);
            seedGenerator = Random.state;
            seedGeneratorInitialised = true;
        }

        // set generator state to seed generator
        Random.state = seedGenerator;
        // generate new seed
        var newSeed = Random.Range(int.MinValue, int.MaxValue);
        // remember generator state
        seedGenerator = Random.state;

        // set the original state back so that normal random generation can continue where it left off
        Random.state = temp;

        //Debug.Log(" " + newSeed);

        return newSeed;
    }

    // Creating obstacles on random parts of the tile
    void CreateObstacles(int numOfObstacles)
    {

        for (int i = 0; i < numOfObstacles; ++i)
        {
            Vector3 randPos = new Vector3(Random.Range(mesh.bounds.min.x, mesh.bounds.max.x), mesh.bounds.max.y /*+ 1.0f*/, Random.Range(mesh.bounds.min.z, mesh.bounds.max.z));

            //Debug.Log("randomPos: " + randPos);

            int obstacleType = Random.Range(0, Obstacles.Length);

            switch (obstacleType)
            {
                case 0:
                    GameObject obstacle = Instantiate(Obstacles[0]);
                    obstacle.transform.position = randPos;
                    break;
                case 1:
                    obstacle = Instantiate(Obstacles[1]);
                    obstacle.transform.position = randPos;
                    break;
            }
        }


    }
}
