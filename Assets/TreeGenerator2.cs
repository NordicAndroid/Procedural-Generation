using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

public class TreeGenerator2 : MonoBehaviour {

    private string tree;

    [SerializeField] private string axiom;

    [SerializeField] private int iterations;
    [SerializeField] private float length;
    [SerializeField] private float angle;
    Stack<TransformInfoHelper> stack = new Stack<TransformInfoHelper>();
    TransformInfoHelper helper;
    private List<List<Vector3>> LineList = new List<List<Vector3>>();


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
                expandedTree += j switch
                {
                    'F' => "FF",
                    'B' => "[lFB][rFB]",
                    _ =>j.ToString()
                };
            }

            tree = expandedTree;
            Debug.Log("Tree at iteration "+ i + " is " + tree);
        }
    }

    void CreateMesh()
    {
        Vector3 initialPosition;

        foreach(char j in tree)
        {
            switch (j)
            {
                case 'F':
                    initialPosition = transform.position;
                    transform.Translate(Vector3.up*length);
                    LineList.Add(new List<Vector3>(){initialPosition,transform.position});
                    initialPosition = transform.position;
                break;
                case 'B':
                break;
                case '[':
                    stack.Push(new TransformInfoHelper()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    } );
                break;
                case ']':
                    TransformInfoHelper helper = stack.Pop();
                    transform.position = helper.position;
                    transform.rotation = helper.rotation;
                break;
                case 'l':
                    transform.Rotate(Vector3.back,angle);
                break;
                case 'r':
                    transform.Rotate(Vector3.forward,angle);
                break;
                
                
            }
        }
    }

}