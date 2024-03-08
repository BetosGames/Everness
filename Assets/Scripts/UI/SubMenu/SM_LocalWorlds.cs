using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FileManager;

public class SM_LocalWorlds : MonoBehaviour
{
    public GameObject UIButtonSave_GO;
    public UIScroll worldScroll;

    public void OnEnable()
    {
        worldScroll.DeleteAllContent();

        Save[] saves = ReadSaveFiles();

        if (saves != null && saves.Length != 0)
        {
            foreach (Save save in saves)
            {
                GameObject newSaveButton = Instantiate(UIButtonSave_GO, worldScroll.GetContentBox());
                newSaveButton.GetComponent<UIButtonSave>().Setup(save);
            }
        }
    }
}
