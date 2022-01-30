using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class lightbulb : GATEbase
{
    public override string Name { get; set; } = "LIGHTBULB";

    public bool onOrOff;
    private Sprite offSprite;
    public Sprite onSprite;

    public TextMeshPro labelTMP;

    private void Awake()
    {
        offSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public override void RunNode()
    {
        outputStates = ApplyInputs(inputStates);  //RUNS THE NODES FUNCTION.  I.E. FOR THE AND NODE, CHECKS ITS TWO INPUTS AND MAKES A BOOL[] HOLDING A SINGLE BOOL OUTPUT
    }
    public override List<bool> ApplyInputs(List<bool> inputs)
    {
        if (inputs[0]) GetComponent<SpriteRenderer>().sprite = onSprite;
        else GetComponent<SpriteRenderer>().sprite = offSprite;
        onOrOff = inputs[0];
        return null;
    }
}
