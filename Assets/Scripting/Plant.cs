using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

public class Plant : MonoBehaviour 
{
    private string treeString;
    private float length = 0.6f;
    private float branchRadius = 0.1f;
    private float angleMin = 10f, angleMax = 30f, angleYMin = 60f, angleYMax = 100f;
    private Material treeMaterial;
    [SerializeField] private GameObject leafPrefab;

    private Stack<TransformInfoHelper> stack = new Stack<TransformInfoHelper>();
    private Stack<int> splineIndexStack = new Stack<int>();

    public void Initialize(string expandedString, float len, float radius, Material mat)
    {
        treeString = expandedString;
        length = len;
        branchRadius = radius;
        treeMaterial = mat;

        BuildMesh();
    }

    void BuildMesh()
    {
        // Add necessary components
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = treeMaterial;

        var container = gameObject.AddComponent<SplineContainer>();
        container.RemoveSplineAt(0);
        
        var extrude = gameObject.AddComponent<SplineExtrude>();
        extrude.Container = container;
        extrude.Radius = branchRadius;

        Vector3 drawingPos = Vector3.zero; 
        Quaternion drawingRot = Quaternion.identity;

        var currentSpline = container.AddSpline();
        var splineIndex = 0;
        currentSpline.Add(new BezierKnot(drawingPos), TangentMode.AutoSmooth);

        // Turn chars to physical form
        foreach(char j in treeString)
        {
            switch (j)
            {
                case 'F':
                    drawingPos += drawingRot * (Vector3.up * length);
                    currentSpline.Add(new BezierKnot(drawingPos), TangentMode.AutoSmooth);
                    break;

                case 'L':
                    for (int i = 0; i < 10; i++) {
                        GameObject leaf = Instantiate(leafPrefab, transform);
                        float randomBackOffset = Random.Range(0, length); 
                        Vector3 distributedPos = drawingPos - (drawingRot * (Vector3.up * randomBackOffset));
                        Vector3 jitter = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                        leaf.transform.localPosition = distributedPos + jitter;
                        
                        Quaternion leafRot = drawingRot * Quaternion.Euler(Random.Range(-35f, 35f), Random.Range(0, 360f), 0);
                        leaf.transform.localRotation = leafRot;
                    }
                    break;
                case '[':
                    stack.Push(new TransformInfoHelper() { position = drawingPos, rotation = drawingRot });
                    splineIndexStack.Push(splineIndex);
                    
                    int prevSplineIndex = splineIndex;
                    int prevSplineKnotCount = currentSpline.Count;

                    currentSpline = container.AddSpline();
                    splineIndex = container.Splines.Count - 1;
                    currentSpline.Add(new BezierKnot(drawingPos), TangentMode.AutoSmooth);
                    
                    // Connect branchs
                    container.LinkKnots(new SplineKnotIndex(prevSplineIndex, prevSplineKnotCount - 1), new SplineKnotIndex(splineIndex, 0));
                    break;
                case ']':
                    TransformInfoHelper helper = stack.Pop();
                    drawingPos = helper.position;
                    drawingRot = helper.rotation;
                    splineIndex = splineIndexStack.Pop();
                    currentSpline = container.Splines[splineIndex];
                    break;
                case 'l':
                    drawingRot *= Quaternion.Euler(0, 0, -Random.Range(angleMin, angleMax));
                    drawingRot *= Quaternion.Euler(0, Random.Range(angleYMin, angleYMax), 0);
                    break;
                case 'r':
                    drawingRot *= Quaternion.Euler(0, 0, Random.Range(angleMin, angleMax));
                    drawingRot *= Quaternion.Euler(0, Random.Range(angleYMin, angleYMax), 0);
                    break;
            }
        }

        // Add collision
        var meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.mesh;
    }
}