/*

Description:        Editor GUI for Squad's Editor:
                    - Allows visualization and visual positioning of Squads in Unity Editor
                    - Units are shown and selectable
                    - Units can be positioned freely
                    - Shows Trigger as boundary box

David Griffith 2017
 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public static class SquadEditorGUI
{
    #region GUI Methods
    //Projects the boundaries of the box collider tied to the squad trigger
    public static void ShowSquadBoundaries(SquadTrigger squadTrigger)
    {
        BoxCollider boxCollider = squadTrigger.GetComponent<BoxCollider>();

        Handles.color = Color.green;
        float lineSize = 1.0f;
        Vector3[] boundaryCorners = new Vector3[8];

        boundaryCorners[0] = boxCollider.bounds.min;                                                                    //Bottom Left
        boundaryCorners[1] = new Vector3(boxCollider.bounds.min.x, boxCollider.bounds.max.y, boxCollider.bounds.min.z); //Top Left
        boundaryCorners[2] = new Vector3(boxCollider.bounds.max.x, boxCollider.bounds.max.y, boxCollider.bounds.min.z); //Top Right
        boundaryCorners[3] = new Vector3(boxCollider.bounds.max.x, boxCollider.bounds.min.y, boxCollider.bounds.min.z); //Bottom Right

        boundaryCorners[4] = new Vector3(boxCollider.bounds.min.x, boxCollider.bounds.min.y, boxCollider.bounds.max.z); //Bottom Left
        boundaryCorners[5] = new Vector3(boxCollider.bounds.min.x, boxCollider.bounds.max.y, boxCollider.bounds.max.z); //Top Left
        boundaryCorners[6] = boxCollider.bounds.max;                                                                    //Top Right
        boundaryCorners[7] = new Vector3(boxCollider.bounds.max.x, boxCollider.bounds.min.y, boxCollider.bounds.max.z); //Bottom Right


        //Draw front square
        Handles.DrawDottedLine(boundaryCorners[0], boundaryCorners[1], lineSize);
        Handles.DrawDottedLine(boundaryCorners[1], boundaryCorners[2], lineSize);
        Handles.DrawDottedLine(boundaryCorners[2], boundaryCorners[3], lineSize);
        Handles.DrawDottedLine(boundaryCorners[3], boundaryCorners[0], lineSize);

        //Draw back square
        Handles.DrawDottedLine(boundaryCorners[4], boundaryCorners[5], lineSize);
        Handles.DrawDottedLine(boundaryCorners[5], boundaryCorners[6], lineSize);
        Handles.DrawDottedLine(boundaryCorners[6], boundaryCorners[7], lineSize);
        Handles.DrawDottedLine(boundaryCorners[7], boundaryCorners[4], lineSize);

        //Draw side lines
        Handles.DrawDottedLine(boundaryCorners[0], boundaryCorners[4], lineSize);
        Handles.DrawDottedLine(boundaryCorners[1], boundaryCorners[5], lineSize);
        Handles.DrawDottedLine(boundaryCorners[2], boundaryCorners[6], lineSize);
        Handles.DrawDottedLine(boundaryCorners[3], boundaryCorners[7], lineSize);
    }

    public static void ShowOtherSquadBoundaries(SquadTrigger referenceSquadTrigger)
    {
        foreach (SquadTrigger squadTrigger in referenceSquadTrigger.SquadManager.SquadTriggers)
        {
            if (squadTrigger.GetInstanceID() != referenceSquadTrigger.GetInstanceID())
            {
                ShowSquadBoundaries(squadTrigger);
            }
        }
    }

    //Projects spawn-markers (Handles) with selected settings
    public static void ShowSpawnMarkers(SquadTrigger squadTrigger)
    {
        foreach (Squad squad in squadTrigger.Squads)
        {
            if (squad.UnitPositions != null)
            {
                for (int count = 0; count < squad.UnitPositions.Length; count++)
                {
                    if (count == 0)
                    {
                        Handles.color = Color.red;

                        //Spawn button marker at location of squad
                        if (Handles.Button(squad.UnitPositions[count], Quaternion.identity, 0.25f, 0.25f, Handles.DotCap))
                        {
                            //Make squad active GameObject in Hierarchy
                            Selection.activeGameObject = squad.gameObject;
                        }
                    }
                    else
                    {
                        Handles.color = Color.yellow;

                        //Spawn simple marker at target Vector
                        if (Handles.Button(squad.UnitPositions[count], Quaternion.identity, 0.25f, 0.25f, Handles.DotCap))
                        {
                            UnitCursor cursor = squadTrigger.SquadManager.UnitCursor.GetComponent<UnitCursor>();

                            cursor.SetUnit(count, squad);

                            Selection.activeGameObject = cursor.gameObject;
                        }
                    }
                }
            }
        }
    }

    public static void ShowSpawnMarkers(SquadManager squadManager)
    {
        foreach (SquadTrigger squadTrigger in squadManager.SquadTriggers)
        {
            ShowSpawnMarkers(squadTrigger);
        }
    }

    //Show single marker for squad trigger's location, will change focus to when clicked
    public static void ShowSquadTriggerMarkers(SquadManager squadManager)
    {
        Handles.color = Color.green;

        foreach (SquadTrigger squadTrigger in squadManager.SquadTriggers)
        {
            //Spawn button marker at location of squad
            if (Handles.Button(squadTrigger.transform.position, Quaternion.identity, 0.20f, 0.20f, Handles.DotCap))
            {
                //Make squad active GameObject in Hierarchy
                Selection.activeGameObject = squadTrigger.gameObject;
            }
        }
    }

    //Draw line to squad trigger's position
    public static void ShowAlignedSquadTriggerAxis(SquadManager squadManager)
    {
        Handles.color = Color.white;

        //Spawn button marker at location of squad
        if (Handles.Button(squadManager.transform.position, Quaternion.identity, 0.10f, 0.10f, Handles.DotCap))
        {
            //Make squad active GameObject in Hierarchy
            Selection.activeGameObject = squadManager.gameObject;
        }

        Vector3[] squadTriggerPositions = new Vector3[squadManager.SquadTriggers.Count + 1];

        squadTriggerPositions[0] = squadManager.transform.position;

        for (int i = 0; i < squadManager.SquadTriggers.Count; i++)
        {
            squadTriggerPositions[i + 1] = squadManager.SquadTriggers[i].transform.position;
        }

        Handles.DrawPolyLine(squadTriggerPositions);   
    }
    #endregion
}