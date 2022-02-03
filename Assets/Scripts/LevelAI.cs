using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using System.Linq;
using System;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class LevelAI : SerializedMonoBehaviour
{
    public int levelIndex;

    public List<OutputNode> inputs;
    public List<lightbulb> outputs;

    [SerializeField]
    LevelMatrix levelMatrix;

    [TableMatrix(HorizontalTitle = "Square Celled Matrix", SquareCells = false)]
    public bool[,] InputOutputMatrix;

    
    [Button]
    private void CreateData()
    {
        //this is used a lot so saving it for performance
        int ins = (int)Mathf.Pow(2, inputs.Count);

        //creates a matrix for the inspector.  this is how you set inputs and outputs
        InputOutputMatrix = new bool[inputs.Count + outputs.Count, ins];
        for (int i = 0; i < ins; i++)
        {
            string binary = Convert.ToString(i, 2);
            while (binary.Length < inputs.Count) binary = "0" + binary;
            //binary = ReverseStringWithInbuiltMethod(binary);
            for (int j = 0; j < inputs.Count; j++)
            {
                InputOutputMatrix[j, i] = Int2Bool(int.Parse(Char.ToString(binary[j])));
            }
        }

        
    }

    [Button]
    public void GenerateOnScreenMatrix()
    {
        //creating the level matrix that is shown to the player
        levelMatrix.GenerateMatrix(InputOutputMatrix, outputs.Count);
    }

    private void Start()
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            inputs[i].labelTMP.text = GetColumnName(i);
            inputs[i].Label = inputs[i].labelTMP.text;
            inputs[i].indestructable = true;
        }
        for (int i = 0; i < outputs.Count; i++)
        {
            outputs[i].labelTMP.text = GetColumnNameReverse(i);
            outputs[i].indestructable = true;
        }
    }

    [Button]
    public void Test()
    {
        //saves the states the outputs are in in order to set them back after testing
        List<bool> states = new List<bool>();
        foreach (OutputNode node in inputs)  states.Add(node.outputStates[0]);

        //this is used a lot so saving it for performance
        int ins = (int)Mathf.Pow(2, inputs.Count);

        bool win = true;//if this is still true at the end of this function, the player has beat the level;

        
        for (int i = 0; i < ins; i++)
        {
            for (int j = 0; j < inputs.Count; j++)
            {
                inputs[j].Set(InputOutputMatrix[j, i]);
            }
            for (int j = 0; j < outputs.Count; j++)
            {
                levelMatrix.content.GetChild((outputs.Count-1)-j).GetChild(i + 2).GetChild(0).GetComponent<Image>().sprite 
                    = outputs[(outputs.Count - 1) - j].onOrOff == InputOutputMatrix[inputs.Count + ((outputs.Count - 1) - j), i] ? levelMatrix.Check : levelMatrix.Cross;
                if (!outputs[j].onOrOff == InputOutputMatrix[inputs.Count + j, i]) win = false;
                //print(outputs[j].onOrOff == InputOutputMatrix[inputs.Count + j, i]);
            }
        }

        for (int i = 0; i < inputs.Count; i++)
        {
            inputs[i].Set(states[i]);
        }

        if (win)
        {
            //print("player beat the level");
            ToolbarAI.toolbar.AddNextGate(levelIndex);
            GameObject conf = Instantiate(FrontMan.FM.confetti, transform);
            conf.transform.position = Camera.main.transform.position.Change(0, 0, 20);
            conf.SetActive(true);
        }
    }

    [Button]
    public void Test2(int stage)
    {
        for (int j = 0; j < inputs.Count; j++)
        {
            inputs[j].Set(InputOutputMatrix[j, stage]);
        }
        print(InputOutputMatrix[2, stage]);
        for (int j = 0; j < outputs.Count; j++)
        {
            print(outputs[j].onOrOff);
            print(outputs[j].onOrOff == InputOutputMatrix[inputs.Count + j, stage]);
            
        }
    }










    public bool Int2Bool(int i)
    {
        return i == 0 ? false : true;
    }
    private static string ReverseStringWithInbuiltMethod(string stringInput)
    {
        // using Linq  

        return new string(stringInput.ToCharArray().Reverse().ToArray());

    }

    static string GetColumnName(int index)
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        var value = "";

        if (index >= letters.Length)
            value += letters[index / letters.Length - 1];

        value += letters[index % letters.Length];

        return value;
    }

    static string GetColumnNameReverse(int index)
    {
        string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        letters = ReverseStringWithInbuiltMethod(letters);

        var value = "";

        if (index >= letters.Length)
            value += letters[index / letters.Length - 1];

        value += letters[index % letters.Length];

        return value;
    }
}
