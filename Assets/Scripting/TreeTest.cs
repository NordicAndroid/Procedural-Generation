using UnityEngine;
using System.Collections.Generic;

public class TreeTest : MonoBehaviour
{
    [SerializeField] private GameObject plantPrefab;
    [SerializeField] private Material testMaterial;

    //Each desired test can be selected here
    public void Start()
    {
        //RunStringLogicTest();
        //RunBracketLogicTest();
        //RunVisualComparisonTest();
        RunPerformanceTest();
    }

    //output of the specific iteration is compared with the expected result
    public void RunStringLogicTest()
    {
        Debug.Log("--- Stage 1: String Logic Test ---");
        Random.State originalState = Random.state;

        Random.InitState(42);
        TreeGeneration treeGen = new TreeGeneration(plantPrefab, testMaterial);

        Debug.Log("- Comparing the output with the expected result: -");
        string testString = treeGen.GenerateLSystemString();
        string result = "FF[llFF[llFF[lFBL][rrFBL]L][rFF[llFBL][rFBL]L]L][rFF[lF[lFBL][rrFBL]L][rrFF[llFBL][rFBL]L]L]";

        if(testString == result)
            Debug.Log(" The output is the same with the desired output. ");
        else
            Debug.Log(" Wrong result, result is: " + testString);

        Random.state = originalState;
    }

    //every ’[’ symbol has to be paired with a ’]’ symbol
    public void RunBracketLogicTest()
    {
        string debugString = "--- BracketLogicTest ---\n";

        TreeGeneration treeGen = new TreeGeneration(plantPrefab, testMaterial);
        int sampleCount = 10;
        bool allPassed = true;

        for (int s = 0; s < sampleCount; s++)
        {
            string testString = treeGen.GenerateLSystemString();
            
            int openBrackets = 0;

            foreach (char c in testString)
            {
                if (c == '[')
                    openBrackets++;
                if (c == ']')
                    openBrackets--;
                if(openBrackets<0)
                    break;
            }

            if (openBrackets != 0)
            {
                debugString += "Error in string: " + testString + "\n";
                allPassed = false;
            }
        }

        if (allPassed)
        {
            debugString +="Bracket Balance Test: Checked "+ sampleCount +" randomized expansions. All brackets perfectly paired.";
        }
        else
        {
            debugString +="Bracket Balance Test: Checked "+ sampleCount +" randomized expansions. Could not pass.";
        }
        Debug.Log(debugString);
    }

    //physical structure is inspected using Unity’s scene view
    public void RunVisualComparisonTest()
    {
        Debug.Log("--- Stage 2: Visual Comparison Test ---");
        
        Random.State originalState = Random.state;

        Random.InitState(42);
        TreeGeneration gen = new TreeGeneration(plantPrefab, testMaterial);
        string string1 = gen.GenerateLSystemString();
        Debug.Log("First string: " + string1);
        int openBrackets = 0;
        foreach (char c in string1)
        {
            if (c == '[')
                openBrackets++;
        }
        Debug.Log("First string branch count: " + openBrackets);
        
        Random.InitState(42);
        string string2 = gen.GenerateLSystemString();
        Debug.Log("Second string: " + string2);

        Random.InitState(1337);
        string string3 = gen.GenerateLSystemString();
        Debug.Log("Third string: " + string3);

        if (string1 == string2)
        {
            Debug.Log("First two tree has same string.");
        }
        else
        {
            Debug.LogError("Produced different strings for first two tree");
        }

        if (string1 != string3)
        {
            //Debug.Log("Different seeds successfully produced distinct structures.");
        }
        else
        {
            Debug.LogWarning("Different seeds produced the exact same string.");
        }

        Random.state = originalState;
        
        Plant treeA = gen.SpawnPlant(new Vector3(-5, 0, 0),string1);
        treeA.name = "Tree_Base_Seed_A";

        Plant treeA_Clone = gen.SpawnPlant(new Vector3(0, 0, 0),string2);
        treeA_Clone.name = "Tree_Clone_Seed_A";

        Plant treeB = gen.SpawnPlant(new Vector3(5, 0, 0),string3);
        treeB.name = "Tree_Different_Seed_B";

        Debug.Log("3 Test trees instantiated at positions (-5, 0, 0), (0, 0, 0), and (5, 0, 0). Inspect vertices, splines, and leaf distribution in Scene View.");
    }

    //Cpu time is measured according to the iteration
    public void RunPerformanceTest()
    {
        Debug.Log("--- Stage 3: Performance and Scalability Test ---");
        TreeGeneration testGen = new TreeGeneration(plantPrefab, testMaterial);
        testGen.GenerateLSystemString();
        testGen.SpawnPlant(new Vector3(-10, 0, 0));
        
        for (int iter = 1; iter <= 6; iter++)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            
            testGen.SetIteration(iter);
            
            stopwatch.Start();
            string expandedResult = testGen.GenerateLSystemString();
            testGen.SpawnPlant(new Vector3(iter*5, 0, 0),expandedResult);
            stopwatch.Stop();

            int leafCommandCount = 0;
            int branchCommandCount = 0;
            foreach (char c in expandedResult)
            {
                if (c == 'L') 
                    leafCommandCount++;
                if (c == '[')
                    branchCommandCount++;
            }

            int estimatedTotalLeaves = leafCommandCount * 10; 

            Debug.Log($"[METRIC] Iteration Depth: {iter} | " +
                      $"String Length: {expandedResult.Length} chars | " +
                      $"Leaf Commands ('L'): {leafCommandCount} (~{estimatedTotalLeaves} instantiated leaf meshes) | " +
                      $"Branch Commands ('['): {branchCommandCount} | " +
                      $"CPU Generation Time: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}