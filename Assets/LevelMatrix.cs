using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using System.Linq;
using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class LevelMatrix : MonoBehaviour
{
    [SerializeField] public Transform content;
    [SerializeField] GameObject LMCI;
    [SerializeField] public Sprite Check;
    [SerializeField] public Sprite Cross;
    public void GenerateMatrix(bool[,] matrix, int outputs)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2((matrix.GetLength(0) + outputs) * 50, (matrix.GetLength(1)+1) * 50);//the height is increased by one for the purposes of labels
        for (int i = 0; i < matrix.GetLength(0) + outputs; i++)
        {
            GameObject lmci = Instantiate(LMCI, content);
            lmci.transform.SetAsFirstSibling();
            TextMeshProUGUI text = lmci.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            if (i < (matrix.GetLength(0) - outputs))  text.text = GetColumnName(i);
            else text.text = GetColumnNameReverse(i-outputs-1);
        }

        //labels the last to columns, as they are what shows the player weather they have succeeded or not
        int count = 1;
        for (int i = outputs-1; i >= 0; i--)
        {
            content.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Out " + (count);
            count++;
        }

        for (int i = content.childCount - 1; i >= 0 ; i--)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                GameObject matrixItem = Instantiate(LMCI, content.GetChild(i));
                if (i > outputs-1)
                {
                    matrixItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = matrix[(content.childCount - 1)-i,j]?"1":"0";  //gets the objects TMPro and converts the bool to a string
                }
                matrixItem.transform.position = content.GetChild(i).position.Change(25, -(j * 50)-25, 0);
            }
        }
    }

    bool hidden = false;
    [Button]
    public void HideUnhide()
    {
        if (!hidden) transform.DOMove(transform.position.Change(0, -GetComponent<RectTransform>().sizeDelta.y, 0), .3f);
        else transform.DOMove(transform.position.Change(0, GetComponent<RectTransform>().sizeDelta.y, 0), .3f);
        hidden = !hidden;
    }
















    private static string ReverseStringWithInbuiltMethod(string stringInput)
    {
        // using Linq  

        return new string(stringInput.ToCharArray().Reverse().ToArray());

    }

    static string GetColumnName(int index)
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        var value = "";

        if (index >= letters.Length)
            value += letters[index / letters.Length - 1];

        value += letters[index % letters.Length];

        return value;
    }

    static string GetColumnNameReverse(int index)
    {
        string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        letters = ReverseStringWithInbuiltMethod(letters);

        var value = "";

        if (index >= letters.Length)
            value += letters[index / letters.Length - 1];
        value += letters[index % letters.Length];

        return value;
    }
}
