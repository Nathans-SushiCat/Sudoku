using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    public static ChangeText CurrentlyClickedText = null;
    bool Clicked;
    public bool Locked = false;
    private void OnMouseDown()
    {
        if(ChangeText.CurrentlyClickedText != null)
        {
            ChangeText.CurrentlyClickedText.Clicked = false;
        }
        Clicked = true;
        ChangeText.CurrentlyClickedText = this;
    }
    private void Update()
    {
        if(Locked)
            gameObject.GetComponent<TMP_Text>().color = Color.black;
        else
            gameObject.GetComponent<TMP_Text>().color = Color.gray;

        if (Clicked && !Locked)
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Alpha0))
            {
                gameObject.GetComponent<TMP_Text>().text = "";

                ChangeText.CurrentlyClickedText = null;
                Clicked = false;
            }
            for (int i = 1; i < 10; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
                {
                    gameObject.GetComponent<TMP_Text>().text = ""+i;
                    ChangeText.CurrentlyClickedText = null;
                    Clicked = false;
                }
            }
        }
    }

}
