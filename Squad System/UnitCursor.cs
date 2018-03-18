/*

Description:        Helper class to act as a cursor when moving between Squads and moving Units

David Griffith 2017
 
 */

using UnityEngine;
using System.Collections;

public class UnitCursor : MonoBehaviour
{
    private int currentUnitIndex = 0;

    private Squad currentSquad = null;
    public Squad CurrentSquad
    {
        get
        {
            return currentSquad;
        }

        set
        {
            currentSquad = value;
        }
    }



    //Sets the unit by squad and unit index
    public void SetUnit(int unitIndex, Squad squad)
    {
        currentSquad = squad;
        currentUnitIndex = unitIndex;

        //Change the cursor position to the new unitposition
        SetCursorPosition();

        name = "Cursor: " + currentSquad.name + " Unit: " + currentUnitIndex;
    }

    //Move the unit's position the cursor is currently managing, if the unit is set
    public void MoveUnit()
    {
        if (currentSquad != null)
        {
            currentSquad.formation = SquadFormation.Formation.Custom;

            currentSquad.SetCustomUnitPosition(currentUnitIndex, transform.position);
        }
    }

    //Sets the cursor's position to match the unit's position, if the unit is set
    public void SetCursorPosition()
    {
        if (currentSquad != null)
        {
            transform.position = currentSquad.UnitPositions[currentUnitIndex];
        }
    }
}
