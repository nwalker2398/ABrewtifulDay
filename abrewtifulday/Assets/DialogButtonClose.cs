using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogButtonClose : MonoBehaviour
{
    public void CloseDialogBox()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("DialogBox"))
            o.SetActive(false);
    }
}
