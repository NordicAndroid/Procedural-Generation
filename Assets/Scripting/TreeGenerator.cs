using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(SplineContainer))]
[RequireComponent(typeof(SplineExtrude))]
public class TreeGenerator : MonoBehaviour {
    public string axiom = "F"; 
    public int iterations = 4;
    public float angle = 25f;
    public float branchLength = 1f;
    public float branchRadius = 0.05f;
    public Material treeMaterial; 

    public List<string> fRules = new List<string> {
        "F[+&F][-^F][/F][\\F]",
        "F[+&F][\\-^F]",
        "F[/+F][\\-F]",
        "F[&F][^F]"
    };

    private Stack<TransformInfo> transformStack = new Stack<TransformInfo>();
    private SplineContainer splineContainer;
    private SplineExtrude splineExtrude;

    private Vector3 currentPosition;
    private Quaternion currentRotation;
    private int currentSplineIndex = 0;

    void Start() {
        if (treeMaterial == null) return;
        SetupComponents();
        GenerateTree();
    }

    void SetupComponents() {
        splineContainer = GetComponent<SplineContainer>();
        splineExtrude = GetComponent<SplineExtrude>();
        GetComponent<MeshRenderer>().material = treeMaterial;

        splineExtrude.Container = splineContainer;
        splineExtrude.Radius = branchRadius;
        splineExtrude.SegmentsPerUnit = 20;

        while (splineContainer.Splines.Count > 0) {
            splineContainer.RemoveSpline(splineContainer[0]);
        }
        
        splineContainer.AddSpline();
    }

    void GenerateTree() {
        string currentPath = axiom;
        for (int i = 0; i < iterations; i++) {
            currentPath = ExpandRules(currentPath);
        }
        InterpretInstructions(currentPath);
    }

    string ExpandRules(string input) {
        string output = "";
        foreach (char c in input) {
            if (c == 'F') output += fRules[Random.Range(0, fRules.Count)];
            else output += c;
        }
        return output;
    }

    void InterpretInstructions(string instructions) {
        currentPosition = Vector3.zero;
        currentRotation = Quaternion.identity;
        currentSplineIndex = 0;

        AddKnot(currentPosition, currentSplineIndex);

        foreach (char c in instructions) {
            switch (c) {
                case 'F':
                    currentPosition += currentRotation * Vector3.up * branchLength;
                    AddKnot(currentPosition, currentSplineIndex);
                    break;
                case '+': currentRotation *= Quaternion.Euler(0, angle, 0); break;
                case '-': currentRotation *= Quaternion.Euler(0, -angle, 0); break;
                case '&': currentRotation *= Quaternion.Euler(angle, 0, 0); break;
                case '^': currentRotation *= Quaternion.Euler(-angle, 0, 0); break;
                case '\\': currentRotation *= Quaternion.Euler(0, 0, angle); break;
                case '/': currentRotation *= Quaternion.Euler(0, 0, -angle); break;
                case '[':
                    transformStack.Push(new TransformInfo() {
                        position = currentPosition,
                        rotation = currentRotation,
                        splineIndex = currentSplineIndex
                    });
                    
                    splineContainer.AddSpline();
                    currentSplineIndex = splineContainer.Splines.Count - 1;
                    AddKnot(currentPosition, currentSplineIndex);
                    break;
                case ']':
                    TransformInfo ti = transformStack.Pop();
                    currentPosition = ti.position;
                    currentRotation = ti.rotation;
                    currentSplineIndex = ti.splineIndex;
                    break;
            }
        }
        splineExtrude.Rebuild();
    }

    void AddKnot(Vector3 localPos, int index) {
        if (index < 0 || index >= splineContainer.Splines.Count) return;
        BezierKnot knot = new BezierKnot(localPos);
        splineContainer[index].Add(knot);
    }
}

public struct TransformInfo {
    public Vector3 position;
    public Quaternion rotation;
    public int splineIndex;
}