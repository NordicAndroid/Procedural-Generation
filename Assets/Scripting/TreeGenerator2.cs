using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

public class TreeGenerator2 : MonoBehaviour {

    private string tree;
    [SerializeField] private string axiom;
    [SerializeField] private int iterations;
    [SerializeField] private float length;
    [SerializeField] private float angleMin;
    [SerializeField] private float angleMax;
    [SerializeField] private float angleYMin;
    [SerializeField] private float angleYMax;
    [SerializeField] private float branchRadius;
    Stack<TransformInfoHelper> stack = new Stack<TransformInfoHelper>();

    Stack<int> splineIndexStack = new Stack<int>();
    private List<List<Vector3>> LineList = new List<List<Vector3>>();

    [SerializeField] private Material treeMaterial;

    void Start()
    {
        tree = axiom;
        Debug.Log("Starting tree " + tree);
        ExpandTreeString();
        CreateMesh();
         
    }

    private void OnDrawGizmos()
    {
        foreach (List<Vector3> line in LineList)
        {
            Gizmos.DrawLine(line[0],line[1]);
        }
    }
    void ExpandTreeString()
    {
        string expandedTree;

        for (int i = 0; i < iterations; i++)
        {
            expandedTree = "";
            foreach (char j in tree)
            {
                switch (j)
                {
                    case 'F':
                        if(Random.Range(0f,100f) < 50f)
                        {
                            expandedTree += "F";
                        }
                        else
                        {
                            expandedTree += "FF";
                        }
                        break;
                    case 'B':
                        if(Random.Range(0f,100f) < 50f)
                        {
                            expandedTree += "[llFB][rFB]";
                        }
                        else
                        {
                            expandedTree += "[lFB][rrFB]";
                        }
                        
                        break;
                    default:
                        expandedTree += j.ToString();
                        break;
                }
            }

            tree = expandedTree;
            Debug.Log("Tree at iteration "+ i + " is " + tree);
        }
    }

    void CreateMesh()
    {
        GameObject treeObject = new GameObject("Tree");
        var meshFilter = treeObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        var meshRenderer = treeObject.AddComponent<MeshRenderer>();
        meshRenderer.material = treeMaterial;

        var container = treeObject.AddComponent<SplineContainer>();
        container.RemoveSplineAt(0);
        var extrude = treeObject.AddComponent<SplineExtrude>();
        extrude.Container = container;
        extrude.Radius = branchRadius;

        var currentSpline = container.AddSpline();
        var splineIndex = container.Splines.FindIndex(currentSpline);

        currentSpline.Add(new BezierKnot(transform.position), TangentMode.AutoSmooth);

        var meshCollider = treeObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.mesh;


        foreach(char j in tree)
        {
            switch (j)
            {
                case 'F':
                    transform.Translate(Vector3.up*length);
                    currentSpline.Add(new BezierKnot(transform.position), TangentMode.AutoSmooth);
                break;
                case 'B':
                break;
                case '[':
                    stack.Push(new TransformInfoHelper()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    } );
                    splineIndexStack.Push(splineIndex);
                    int splineCount = currentSpline.Count;
                    int prevSplineIndex = splineIndex;
                    currentSpline = container.AddSpline();
                    splineIndex = container.Splines.FindIndex(currentSpline);
                    currentSpline.Add(new BezierKnot(transform.position), TangentMode.AutoSmooth);
                    container.LinkKnots(new SplineKnotIndex(prevSplineIndex,splineCount-1),new SplineKnotIndex(splineIndex,0));
                break;
                case ']':
                    TransformInfoHelper helper = stack.Pop();
                    transform.position = helper.position;
                    transform.rotation = helper.rotation;
                    splineIndex = splineIndexStack.Pop();
                    currentSpline = container.Splines[splineIndex];
                break;
                case 'l':
                    transform.Rotate(Vector3.back,Random.Range(angleMin,angleMax));
                    transform.Rotate(Vector3.up,Random.Range(angleYMin,angleYMax));
                break;
                case 'r':
                    transform.Rotate(Vector3.forward,Random.Range(angleMin,angleMax));
                    transform.Rotate(Vector3.up,Random.Range(angleYMin,angleYMax));
                break;
                
                
            }
        }
    }



}
    public static class TreeGeneratorExtention
    {
        public static int FindIndex(this IReadOnlyList<Spline> splines , Spline spline)
        {
            for (int i = 0; i < splines.Count; i++)
            {
                if(splines[i] == spline)
                {
                    return i;
                }
            }
            return -1;
        }
    }