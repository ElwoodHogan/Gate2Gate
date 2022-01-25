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

public abstract class GATEbase : SerializedMonoBehaviour
{
    public bool indestructable = false;

    public List<WireConnector> inputWCs;  //ORDER MATTERS!
    public List<WireConnector> outputWCs;

    public Dictionary<int, Dictionary<LineRenderer, Action<bool>>> OutgoingSignals = new Dictionary<int, Dictionary<LineRenderer, Action<bool>>>();  //THE BASE LIST IS THE INDEX OF THE OUTPUTS, THE INTERIOR LIST IS THE LIST OUT INPUTS BEING SENT SIGNALS FROM THIS OUTPUT
    //List<Action<bool>> incomingSignals;

    public List<bool> inputStates = new List<bool>();
    public List<bool> outputStates = new List<bool>();

    public abstract string Name { get; set; }

    private void Start()
    {
        OutgoingSignals = new Dictionary<int, Dictionary<LineRenderer, Action<bool>>>();
        if (inputWCs.Count > 0)
        {
            for (int i = 0; i < inputWCs.Count; i++)
            {
                inputWCs[i].inputOrOutput = true;
                inputWCs[i].index = i;
                inputWCs[i].connectedGate = this;
                inputStates.Add(false);
            }
        }
        if(outputWCs.Count > 0)
        {
            for (int i = 0; i < outputWCs.Count; i++)
            {
                outputWCs[i].inputOrOutput = false;
                outputWCs[i].index = i;
                outputWCs[i].connectedGate = this;
                OutgoingSignals.Add(i, new Dictionary<LineRenderer, Action<bool>>());
                outputStates.Add(false);
            }
        }
    }

    public virtual void SetInput(int index, bool state)
    {
        if (index > inputStates.Count - 1)  throw new Exception($"index too high for number of inputs!  max index:{inputStates.Count - 1}, actual:{index}, for the {gameObject.name}");
        if (inputStates[index] == state) return;
        inputStates[index] = state;
        RunNode();
    }
    public virtual void RunNode()
    {
        outputStates = ApplyInputs(inputStates);  //RUNS THE NODES FUNCTION.  I.E. FOR THE AND NODE, CHECKS ITS TWO INPUTS AND MAKES A BOOL[] HOLDING A SINGLE BOOL OUTPUT
        OutputSignal();
        for (int i = 0; i < outputStates.Count; i++)
        {
            outputWCs[i].onOrOff = outputStates[i];
            outputWCs[i].SetElectricWires(outputStates[i]);
        }
    }

    public void OutputSignal()
    {
        //THEN, FOR EACH OUTPUT, SENDS AN OUTPUT SIGNAL TO EACH CONNECTED NODE, WHICH ALSO TRIGGERS THEIR SETINPUT FUNCTIONS
        if (outputStates.Count > 0)
        {
            for (int i = 0; i < outputStates.Count; i++)
                foreach (KeyValuePair<LineRenderer, Action<bool>> outputs in OutgoingSignals[i])
                    outputs.Value.Invoke(outputStates[i]);
        }
    }
    public abstract List<bool> ApplyInputs(List<bool> inputs);  //Runs the gate based on the list of inputs

    private void OnMouseDrag()
    {
        transform.position += FM.mousePosDelta;
        foreach (var wc in inputWCs)
        {
            foreach (var wire in wc.connectedWires)
            {
                //since i dont know weather the player started drawing the wire at the input or the output, the connected WC could be attached to the last or first position of the wire
                //so i gotta do some convaluted nonsense to figure out which it is
                Vector3[] wirePositions = new Vector3[wire.positionCount];
                wire.GetPositions(wirePositions);
                int index = Array.IndexOf(wirePositions, wirePositions.ToList().OrderBy(pos => Vector3.Distance(pos, wc.transform.position)).First());
                wire.SetPosition(index, wc.transform.position + FM.mousePosDelta);
                wire.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(index, (wc.transform.position + FM.mousePosDelta).Change(0, 0, -1));
            }
        }
        foreach (var wc in outputWCs)
        {
            foreach (var wire in wc.connectedWires)
            {
                //since i dont know weather the player started drawing the wire at the input or the output, the connected WC could be attached to the last or first position of the wire
                //so i gotta do some convaluted nonsense to figure out which it is
                Vector3[] wirePositions = new Vector3[wire.positionCount];
                wire.GetPositions(wirePositions);
                int index = Array.IndexOf(wirePositions, wirePositions.ToList().OrderBy(pos => Vector3.Distance(pos, wc.transform.position)).First());
                wire.SetPosition(index, wc.transform.position + FM.mousePosDelta);
                wire.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(index, (wc.transform.position + FM.mousePosDelta).Change(0,0,-1));
            }
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (indestructable) return;
            foreach(WireConnector wc in inputWCs)
            {
                wc.RemoveConnections();
            }
            foreach (WireConnector wc in outputWCs)
            {
                wc.RemoveConnections();
            }
            Destroy(gameObject);
        }
    }
}
