using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAI : MonoBehaviour
{
    public void NextSlide()
    {
        gameObject.SetActive(false);
        if(transform.GetSiblingIndex()+1 != transform.parent.childCount)
        transform.parent.GetChild(transform.GetSiblingIndex() + 1).gameObject.SetActive(true);
    }

    public void PrevSlide()
    {
        if (transform.GetSiblingIndex() - 1 < 0) return;
        gameObject.SetActive(false);
        transform.parent.GetChild(transform.GetSiblingIndex() - 1).gameObject.SetActive(true);
    }
}
