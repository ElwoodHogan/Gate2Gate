using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarAI : MonoBehaviour
{
    public static ToolbarAI toolbar;
    public int gateToAddIndex = 2;
    public Transform Content;

    private void Awake()
    {
        toolbar = this;
    }
    public void AddNextGate(int level)
    {
        Content.GetChild(level + 1).gameObject.SetActive(true);
    }
}
