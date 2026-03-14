using UnityEngine;
using System.Collections.Generic;

public class TreeGenerator : MonoBehaviour {
    public string axiom = "F"; 
    public int iterations = 4;
    public float angle = 25f;
    public float branchLength = 2f;

    public List<string> fRules = new List<string> {
        "F[+&F][-^F][/F][\\F]",
        "F[+&F][\\-^F]",
        "F[/+F][\\-F]",
        "F[&F][^F]"
    };

    private Stack<TransformInfo> transformStack = new Stack<TransformInfo>();
    
    [SerializeField] private GameObject branchPrefab;

    void Start() {
        GenerateTree();
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
            if (c == 'F') {
                int randomIndex = Random.Range(0, fRules.Count);
                output += fRules[randomIndex];
            } else {
                output += c;
            }
        }
        return output;
    }

    void InterpretInstructions(string instructions) {
        foreach (char c in instructions) {
            switch (c) {
                case 'F':
                    Vector3 initialPos = transform.position;
                    transform.Translate(Vector3.up * branchLength);
                    Instantiate(branchPrefab, initialPos, transform.rotation);
                    break;
                case '+': transform.Rotate(Vector3.up * angle); break;      
                case '-': transform.Rotate(Vector3.up * -angle); break;     
                case '&': transform.Rotate(Vector3.right * angle); break;   
                case '^': transform.Rotate(Vector3.right * -angle); break;  
                case '\\': transform.Rotate(Vector3.forward * angle); break; 
                case '/': transform.Rotate(Vector3.forward * -angle); break; 
                case '[': 
                    transformStack.Push(new TransformInfo() {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;
                case ']': 
                    TransformInfo ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;
            }
        }
    }
}

public struct TransformInfo {
    public Vector3 position;
    public Quaternion rotation;
}