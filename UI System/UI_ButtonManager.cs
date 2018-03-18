/*

Description:        Overrides Unity's default selection behavior. Used for main menu buttons, ensure that the colors remain consistent when selected/de-selected
                    Most notably overrides behavior when highlighted, but not pressed

David Griffith 2016
 
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_ButtonManager : MonoBehaviour
{
    public Color defaultColor;
    public Color selectedColor;

    private Button[] buttons;


    void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    public void SetButtonColors(GameObject target)
    {
        ColorBlock colors;

        Button button;

        for (int i = 0; i < buttons.Length; i++)
        {
            button = buttons[i].GetComponent<Button>();
            colors = button.colors;
            colors.normalColor = defaultColor;
            colors.highlightedColor = defaultColor;
            button.colors = colors;
        }

        button = target.GetComponent<Button>();
        colors = button.colors;
        colors.normalColor = selectedColor;
        colors.highlightedColor = selectedColor;
        button.colors = colors;
    }
}
