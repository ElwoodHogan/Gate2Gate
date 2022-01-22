using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine.EventSystems;
using static FrontMan;

public abstract class GATEbase : MonoBehaviour
{
    public List<WireConnector> inputWCs;
    public List<WireConnector> outputWCs;

    private void Start()
    {
        //THIS SETS EACH WIRE CONNECTER TO BE IETHER AN INPUT OR AN OUTPUT, WHICH DEFINES WEATHER IT CAN HAVE MORE THAT 1 WIRE CONNECTION
        foreach(WireConnector WC in inputWCs)
        {
            WC.inputOrOutput = true;
        }   
        foreach (WireConnector WC in outputWCs)
        {
            WC.inputOrOutput = false;
        }
    }
    public bool RunNode()
    {
        List<bool> inputs = new List<bool>();
        foreach(WireConnector wire in inputWCs)
        {
            inputs.Add(wire.onOrOff);
        }
        return ApplyInputs(inputs);
    }
    public abstract bool ApplyInputs(List<bool> inputs);  //Runs the gate based on the list of inputs

    private void Update()
    {
        bool outP = RunNode();
        foreach (WireConnector WC in outputWCs)
        {
            WC.onOrOff = outP;
        }
    }
}
