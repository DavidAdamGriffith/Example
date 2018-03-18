/*

Description:        Helper class, emulates a selection of a button when navigating between menus. Attach to a button object that needs this functionality and call Click() to emulate
                    
                    Example of use, when asked to 'Exit', user says 'No', clicking 'No' will then make the 'Play' button be clicked

David Griffith 2016
 
 */

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UI_CodeClick : MonoBehaviour
{
	// Use this for initialization
	public void Click()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.pointerClickHandler);
    }
}
