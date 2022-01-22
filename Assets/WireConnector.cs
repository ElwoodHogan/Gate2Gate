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

    public bool inputOrOutput;
    public bool onOrOff;
    public Sprite highlightedMode;
    public LineRenderer WirePrefab;
    public List<LineRenderer> connectedWires;
    public static bool connectingWire = false;
    public static LineRenderer currentWire;
    public static WireConnector startWC;
    public WireConnector startPoint;
    private Sprite unHighlightedMode;
    private SpriteRenderer SR;
    public List<WireConnector> connections;
    

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        unHighlightedMode = SR.sprite;
    }

    private void OnMouseOver()
    {
        SR.sprite = highlightedMode;
    }

    private void OnMouseExit()
    {
        SR.sprite = unHighlightedMode;
    }

    private void OnMouseDown()
    {
        if (connectingWire)
        {
            if (connections.Count > 0 && inputOrOutput) return;
            connectingWire = false;
            currentWire.SetPosition(currentWire.positionCount - 1, transform.position);
            connectedWires.Add(currentWire);
            connections.Add(startWC);
            startWC.connections.Add(this);
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

    public void SetState(bool OrO)
    {
        onOrOff = OrO;
        if (!inputOrOutput)
        {
            foreach (WireConnector WC in connections)
            {
                WC.SetState(OrO);
            }
        }
    }
}
