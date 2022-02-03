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

public class WireConnector : MonoBehaviour
{
    public int index; //stores the index of its input.output node.  For example, records if this is the 4th input of the gate
    public GATEbase connectedGate;

    public bool inputOrOutput;
    public bool onOrOff;
    public LineRenderer WirePrefab;
    public List<LineRenderer> connectedWires;
    public static bool connectingWire = false;
    public static LineRenderer currentWire;
    public static WireConnector startWC;
    public WireConnector startPoint;
    private Sprite unHighlightedMode;
    private SpriteRenderer SR;
    public List<WireConnector> connections;

    public Sprite highlightedMode;
    public Sprite inputSprite;
    public Sprite inputHighlightedMode;

    [SerializeField] AudioSource click;
    [SerializeField] AudioSource snip;

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        unHighlightedMode = SR.sprite;
    }

    private void OnMouseOver()
    {
        //change sprite for highlighting
        SR.sprite = highlightedMode;

        //IF RIGHT CLICK, DESTROY CONNECTIONS
        if (Input.GetMouseButtonDown(1))
        {
            RemoveConnections();
        }
    }

    private void OnMouseExit()
    {
        SR.sprite = unHighlightedMode;
    }

    private void OnMouseDown()
    {
        if (connectingWire)
        {

            if (connections.Count > 0 && inputOrOutput) return;  //input nodes can only have one connection
            if (startWC.inputOrOutput == inputOrOutput) return;  //inputs can not connect to inputs and outputs can not connect to outputs
            connectingWire = false;
            currentWire.SetPosition(currentWire.positionCount - 1, transform.position);
            connections.Add(startWC);
            startWC.connections.Add(this);

            //once the wire is complete, make an electrical wire that has the same points
            Vector3[] wirePositions = new Vector3[currentWire.positionCount];
            currentWire.GetPositions(wirePositions);
            for (int i = 0; i < wirePositions.Length; i++)
            {
                wirePositions[i] = wirePositions[i].Change(0, 0, -1);  //moves wire Z down one to put it in front of other wire
            }
            currentWire.transform.GetChild(0).GetComponent<LineRenderer>().positionCount = wirePositions.Length;
            currentWire.transform.GetChild(0).GetComponent<LineRenderer>().SetPositions(wirePositions);

            //catagorizes input and output connections, for easier reading of code
            WireConnector inputWC;
            WireConnector outputWC;

            //CONNECTS THE OUTPUT OF ONE GATE TO THE INPUT OF THE CONNECTED GATE
            if (inputOrOutput)  //input and output nodes defined for organizational purposes
            {
                inputWC = this;
                outputWC = startWC;
            }
            else {
                inputWC = startWC;
                outputWC = this;
            }
            //gets the gate connected to the outputWC the wire is connected to, and based on the index out the outputWC,
            //adds a function that calls the setinput of the inputWC, which takes in the state of the output and puts it into the inputWC-indexed input
            outputWC.connectedGate.OutgoingSignals[outputWC.index].Add(currentWire, (bool state) => inputWC.connectedGate.SetInput(inputWC.index, state));
            outputWC.connectedGate.RunNode();

            outputWC.connectedWires.Add(currentWire);
            inputWC.connectedWires.Add(currentWire);  //inputs need to keep track of wires for the purpose of moving the wire when moving the gate
            outputWC.SetElectricWires(outputWC.connectedGate.outputStates[outputWC.index]);  //Turns the electrical wires on or off depending on the current output


            click.Play();  //play click sound

            MainMeniAI.MM.CreateCopyOfLevel();
        }
        else
        {
            startPoint = this;
            connectingWire = true;
            currentWire = Instantiate(WirePrefab, transform);
            currentWire.SetPosition(0, transform.position);
            currentWire.gameObject.SetActive(true);
            startWC = this;
        }
    }

    public void SetElectricWires(bool state)
    {
        //print("wire altered " + state);
        try
        {
            foreach (LineRenderer wire in connectedWires)
            {
                wire.transform.GetChild(0).gameObject.SetActive(state);
            }
        }
        catch (Exception)
        {
            print("error in: " + gameObject.name);
            print(connectedGate.gameObject.name);
        }
        
    }

    public void RemoveConnections()
    {
        List<WireConnector> connectedWCs = new List<WireConnector>(connections);  //storing a list of connections so that they can run their nodes.

        //if this is the input WC, we only need to remove one connection
        if (inputOrOutput)
        {
            if(connections.Count > 0)
            {
                connections[0].connectedWires.Remove(connectedWires[0]);
                connections[0].connectedGate.OutgoingSignals[connections[0].index].Remove(connectedWires[0]);
                connectedGate.SetInput(index, false);
            }
        }
        else //otherwise, iterate through the list of connections
        {
            foreach (WireConnector wc in connections)
            {
                wc.connections.Remove(this);
                foreach (LineRenderer wire in connectedWires)
                {
                    wc.connectedWires.Remove(wire);
                    connectedGate.OutgoingSignals[index].Remove(wire);
                }
            }
        }

        //physically destroying the wires
        foreach (LineRenderer wire in connectedWires)
        {
            Destroy(wire.gameObject);
        }

        //resetting list
        connections = new List<WireConnector>();
        connectedWires = new List<LineRenderer>();

        //running the nodes again incase of changes states
        if (!inputOrOutput) foreach (var wc in connectedWCs)
            {
                wc.connectedGate.SetInput(wc.index, false);
                wc.connectedGate.RunNode();
            }
        else connectedGate.RunNode();

        snip.Play();
    }
}
