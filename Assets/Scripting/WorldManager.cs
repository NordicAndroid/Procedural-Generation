using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WorldManager : MonoBehaviour
{
    [Header("Noise Settings")]
    public int width = 100;
    public int height = 100;
    public float scale = 20f;
    public float xOrigin = 0f;
    public float yOrigin = 0f;

    [Header("Arazi Ayarları")]
    public float heightMultiplier = 10f;
    public AnimationCurve heightCurve;
    public Gradient terrainGradient;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;
    private PerlinNoise perlin;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        GenerateWorld();
    }

    public void GenerateWorld()
    {
        perlin = new PerlinNoise(width, height);
        perlin.CalculateTexture();

        CreateMeshShape();
        UpdateMesh();
    }

    void CreateMeshShape()
    {
        vertices = new Vector3[(width + 1) * (height + 1)];
        colors = new Color[vertices.Length];

        for (int i = 0, y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
                float rawSample = GetNoiseSample(x, y);
                
                float evaluatedHeight = heightCurve.Evaluate(rawSample) * heightMultiplier;

                vertices[i] = new Vector3(x, evaluatedHeight, y);
                
                colors[i] = terrainGradient.Evaluate(rawSample);
                
                i++;
            }
        }

        triangles = new int[width * height * 6];
        int vert = 0;
        int tris = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + width + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + width + 1;
                triangles[tris + 5] = vert + width + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    float GetNoiseSample(int x, int y)
    {
        int clampedX = Mathf.Clamp(x, 0, width - 1);
        int clampedY = Mathf.Clamp(y, 0, height - 1);
        
        return perlin.texture.GetPixel(clampedX, clampedY).r;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }
}