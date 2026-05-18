using UnityEngine;

public class TreeGeneration 
{
    private string axiom = "FB";
    private int iterations = 3;
    private GameObject plantPrefab;
    private Material treeMaterial;

    public TreeGeneration( GameObject prefab, Material mat)
    {
        this.plantPrefab = prefab;
        this.treeMaterial = mat;
    }

    // GenerationController should call this method to create a tree
    public Plant SpawnPlant(Vector3 position)
    {
        // The L-string is expanded here
        string expandedString = GenerateLSystemString();

        GameObject newPlantObj = UnityEngine.Object.Instantiate(plantPrefab, position, Quaternion.identity);

        Plant plantScript = newPlantObj.GetComponent<Plant>();
        plantScript.Initialize(expandedString, 1f, 0.1f, treeMaterial);

        //newPlantObj.transform.position = position;
        return plantScript;
    }

    private string GenerateLSystemString()
    {
        string currentTree = axiom;
        for (int i = 0; i < iterations; i++)
        {
            string nextIteration = "";
            foreach (char c in currentTree)
            {
                if (c == 'F') nextIteration += (Random.value < 0.5f) ? "F" : "FF";
                else if (c == 'B') nextIteration += (Random.value < 0.5f) ? "[llFB][rFB]" : "[lFB][rrFB]";
                else nextIteration += c;
            }
            currentTree = nextIteration;
        }
        return currentTree;
    }
}