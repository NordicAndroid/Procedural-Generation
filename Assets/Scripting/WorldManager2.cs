using UnityEngine;
using System;

public class WorldManager2 : MonoBehaviour
{
    [Header("World Settings")]
    public int? seed;
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public float depth = 20f; 
    public GameObject aPlantPrefab;
    public Material aTreeMat;

    private Terrain terrain;
    private Noise noise;
    private TreeGeneration treeFactory;
    public int treeCountToSpawn = 500;

    void Start()
    {
        terrain = GetComponent<Terrain>();

        if (seed != null){
            noise = new FractalNoise(width, height, seed);
        }
        else{
            noise = new FractalNoise(width, height);
            Debug.Log("Not providing seed at initial generation");
        }
        noise.updateScale(scale);
        noise.CalculateTexture();
        
        GenerateTerrain();
        ApplyTextures();

        treeFactory = new TreeGeneration( aPlantPrefab, aTreeMat);
        SpawnVegetation();
    }
    public void Recalculate()
    {
        DespawnPlants();
        if (seed != null){
            noise = new FractalNoise(width, height, seed);
        }
        else{
            noise = new FractalNoise(width, height);
            Debug.Log("Not providing seed");
        }
        noise.updateScale(scale);
        noise.CalculateTexture();
        GenerateTerrain();
        ApplyTextures();
        treeFactory = new TreeGeneration(aPlantPrefab, aTreeMat);
        SpawnVegetation();
    }
    public void updateSeed(string seedString){
        try{
            seed = int.Parse(seedString);
        }
        catch(Exception e){
            seed = null;
        }
    }

    void GenerateTerrain()
    {
        TerrainData terrainData = terrain.terrainData;
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);

        float[,] heights = new float[width, height];

        heights = noise.heights;
        terrainData.SetHeights(0, 0, heights);
    }

    void ApplyTextures()
    {
        TerrainData terrainData = terrain.terrainData;
        int aWidth = terrainData.alphamapWidth;
        int aHeight = terrainData.alphamapHeight;

        float[,,] splatmapData = new float[aWidth, aHeight, terrainData.terrainLayers.Length];

        for (int y = 0; y < aHeight; y++)
        {
            for (int x = 0; x < aWidth; x++)
            {
                float xPercent = (float)x / aWidth;
                float yPercent = (float)y / aHeight;
                
                float heightSample = noise.texture.GetPixelBilinear(xPercent, yPercent).r;

                float[] weights = new float[terrainData.terrainLayers.Length];

                if (heightSample < 0.20f) 
                    weights[0] = 1.0f;
                else if (heightSample < 0.30f) 
                    weights[1] = 1.0f;
                else 
                    weights[2] = 1.0f;

                for (int i = 0; i < terrainData.terrainLayers.Length; i++)
                {
                    splatmapData[x, y, i] = weights[i];
                }
            }
        }
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }

void SpawnVegetation()
    {
        
        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < treeCountToSpawn; i++)
        {
            //UnityEngine.Random coordinates
            float randomX = UnityEngine.Random.Range(0, width);
            float randomZ = UnityEngine.Random.Range(0, height);

            Vector3 spawnPos = new Vector3(randomX, 0, randomZ);
            
            //get the height value from the terrain
            float worldHeight = terrain.SampleHeight(spawnPos);

            float normalizedHeight = worldHeight / depth;

            //plant on the black area
            if (normalizedHeight >= 0.20f && normalizedHeight < 0.30f)
            {
                float xPercent = randomX / width;
                float yPercent = randomZ / height;

                //get the steepness and eliminate some points
                float steepness = terrainData.GetSteepness(xPercent, yPercent);
                if (steepness < 25f)
                {
                    spawnPos.y = worldHeight;
                    treeFactory.SpawnPlant(spawnPos);
                }
            }
        }
    }
    void DespawnPlants()
    {
        GameObject[] trees = new GameObject[treeCountToSpawn];
        trees = GameObject.FindGameObjectsWithTag("Tree");
        foreach (GameObject oneObject in trees)
            Destroy(oneObject);

    }
}