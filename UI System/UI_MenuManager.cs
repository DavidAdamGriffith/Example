/*

Description:        Helper class to select a menu through code

David Griffith 2016
 
 */

using UnityEngine;
using System.Collections;

public class UI_MenuManager : MonoBehaviour
{
    public GameObject selectedMenu;

    public void SetSelectedMenu(GameObject menu)
    {
        selectedMenu = menu;
    }
}
