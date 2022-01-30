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

public class HALFADDER : GATEbase
{
    public override string Name { get; set; } = "NAND";
    public override List<bool> ApplyInputs(List<bool> inputs)
    {
        if (inputs.Count > 2) throw new Exception("incorrect amount of inputs for this gate! Expected Input: 2, actual inputs: " + inputs.Count);
        //print("runned");
        //return new List<bool> { !(inputs[0] && inputs[1]) };
        List<bool> outputs;
        if (inputs[0])
        {
            if (inputs[1]) outputs = new List<bool>() { true, false };
            else outputs = new List<bool>() { false, true };
        }
        else
        {
            if (inputs[1]) outputs = new List<bool>() { false, true };
            else outputs = new List<bool>() { false, false };
        }
        return outputs;
    }
}
