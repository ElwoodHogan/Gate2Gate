using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class OutputNode : GATEbase
{
    public override string Name { get; set; } = "OUTPUT";

    public string Label;
    public TextMeshProUGUI labelTMP;

    private Sprite offSprite;
    public Sprite onSprite;


    private void Awake()
    {
        offSprite = GetComponent<SpriteRenderer>().sprite;
    }
    private void OnMouseDown()
    {
        Toggle();
    }

    public void Toggle()
    {
        outputStates[0] = !outputStates[0];
        if (outputStates[0]) GetComponent<SpriteRenderer>().sprite = onSprite;
        else GetComponent<SpriteRenderer>().sprite = offSprite;

        //custom output function for this output node, which for right now is the only gate with no input
        foreach (KeyValuePair<LineRenderer, Action<bool>> outputs in OutgoingSignals[0])
            outputs.Value.Invoke(outputStates[0]);

        for (int i = 0; i < outputStates.Count; i++)
        {
            outputWCs[i].onOrOff = outputStates[i];
            outputWCs[i].SetElectricWires(outputStates[i]);
        }
    }

    public void Set(bool setState)
    {
        outputStates[0] = setState;
        if (outputStates[0]) GetComponent<SpriteRenderer>().sprite = onSprite;
        else GetComponent<SpriteRenderer>().sprite = offSprite;

        //custom output function for this output node, which for right now is the only gate with no input
        foreach (KeyValuePair<LineRenderer, Action<bool>> outputs in OutgoingSignals[0])
            outputs.Value.Invoke(outputStates[0]);

        for (int i = 0; i < outputStates.Count; i++)
        {
            outputWCs[i].onOrOff = outputStates[i];
            outputWCs[i].SetElectricWires(outputStates[i]);
        }
    }

    private void OnValidate()
    {
        labelTMP.text = Label;
    }

    public override List<bool> ApplyInputs(List<bool> inputs) { return null; }

    public override void RunNode()
    {
        base.OutputSignal();
    }

    public override void SetInput(int index, bool state)
    {
        print("output was called by wireconnector of index:" + index);
    }
}
