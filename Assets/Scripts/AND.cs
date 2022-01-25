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

public class AND : GATEbase
{
    public override string Name { get; set; } = "AND";
    public override List<bool> ApplyInputs(List<bool> inputs)
    {
        if(inputs.Count > 2) throw new Exception("incorrect amount of inputs for this gate! Expected Input: 2, actual inputs: " + inputs.Count);
        //print("runned");
        return new List<bool> { inputs[0] && inputs[1] };  //returning the AND output using C#'s build in && function
    }
}
