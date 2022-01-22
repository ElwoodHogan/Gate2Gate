using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputNode : MonoBehaviour
{
    public bool onOrOff;
    private Sprite offSprite;
    public Sprite onSprite;
    public WireConnector outputWC;


    private void Awake()
    {
        offSprite = GetComponent<SpriteRenderer>().sprite;
    }
    private void OnMouseDown()
    {
        onOrOff = !onOrOff;
        if (onOrOff) GetComponent<SpriteRenderer>().sprite = onSprite;
        else GetComponent<SpriteRenderer>().sprite = offSprite;
    }

    private void Update()
    {
        outputWC.SetState(onOrOff);
    }
}
