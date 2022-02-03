using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMeniAI : MonoBehaviour
{
    public Transform currentMenu = null;
    public List<Transform> menus = new List<Transform>();
    public Transform outside;  //this is where a menu goes when not selected
    public Transform middleOfScreen;

    public LevelAI currentLevel;
    public List<LevelAI> Levels = new List<LevelAI>();

    public Transform Toolbar;
    public Transform ToolbarOnScreenPos;
    public Transform ToolbarOffScreenPos;

    public static MainMeniAI MM;

    public Transform LevelParent;

    private void Awake()
    {
        MM = this;
    }

    private void Update()
    {
        //undo button, impliment in future
        //if (Input.GetKeyDown(KeyCode.Z)) RevertLevel();
    }
    public void SelectMenu(int index)
    {
        if (currentMenu)
        {
            currentMenu.DOMove(outside.position, .5f);
        }
        menus[index].DOMove(middleOfScreen.position, .8f);
        currentMenu = menus[index];
    }

    public void SelectLevel(int index)
    {
        if (currentLevel) currentLevel.gameObject.SetActive(false);
        currentLevel = Instantiate(Levels[index - 1], LevelParent);
        currentLevel.gameObject.SetActive(true);

        //moves the toolbar in and the menus out
        Toolbar.DOMove(ToolbarOnScreenPos.position, 1);
        currentMenu.DOMove(outside.position, 1);
        transform.DOMove(transform.position.Change(-1920 / 2, 0, 0), 1);

        CreateCopyOfLevel();
    }

    public void ExitLevel()
    {
        foreach (Transform child in LevelParent) Destroy(child.gameObject);
        Toolbar.DOMove(ToolbarOffScreenPos.position, .8f);
        currentMenu.DOMove(middleOfScreen.position, 1f);
        transform.DOMove(middleOfScreen.position, 1f);
    }

    public void  Exit()
    {
        Application.Quit();
    }

    public void CreateCopyOfLevel()
    {
       LevelAI newLevel = Instantiate(currentLevel, LevelParent);
        newLevel.gameObject.SetActive(false);
        currentLevel.transform.SetAsLastSibling();
    }

    public void RevertLevel()
    {
        if (LevelParent.childCount <= 2) return;
        DestroyImmediate(currentLevel.gameObject);
        DestroyImmediate(LevelParent.GetChild(LevelParent.childCount - 1).gameObject);
        Transform newCopy = Instantiate(LevelParent.GetChild(LevelParent.childCount - 1), LevelParent);
        LevelParent.GetChild(LevelParent.childCount - 1).gameObject.SetActive(true);
        currentLevel = LevelParent.GetChild(LevelParent.childCount - 1).GetComponent<LevelAI>();
    }
}
