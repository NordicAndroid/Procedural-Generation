using UnityEngine;

public class TreeGenerator : MonoBehaviour 
{
    public GameObject aPlantPrefab;
    public Material aTreeMat;
    private TreeGeneration treeFactory;

    void Start()
    {
        treeFactory = new TreeGeneration( aPlantPrefab, aTreeMat);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 randomPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            treeFactory.SpawnPlant(randomPos);
        }
    }
}