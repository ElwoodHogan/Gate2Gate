using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class INVERSE : GATEbase
{
    public override string Name { get; set; } = "INVERSE";
    public override List<bool> ApplyInputs(List<bool> inputs)
    {
        if (inputs.Count > 2) throw new Exception("incorrect amount of inputs for this gate! Expected Input: 2, actual inputs: " + inputs.Count);
        return new List<bool> {!inputs[0]};
    }
}
