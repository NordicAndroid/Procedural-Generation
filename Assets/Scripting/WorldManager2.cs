using UnityEngine;

public class WorldManager2 : MonoBehaviour
{
    [Header("World Settings")]
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public float depth = 20f; 

    private Terrain terrain;
    private Noise noise;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        noise = new FractalNoise(width, height, 0, 0, scale);
        noise.CalculateTexture();
        
        GenerateTerrain();
        ApplyTextures();
    }

    void GenerateTerrain()
    {
        TerrainData terrainData = terrain.terrainData;
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);

        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = noise.texture.GetPixel(x, y).r;
            }
        }
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

                if (heightSample < 0.25f) 
                    weights[0] = 1.0f;
                else if (heightSample < 0.55f) 
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
}