/*

Description:        Editor for UnitCursor:
                    - Highlights the currently selected unit
                    - Important! Works around Unity's default behavior to ensure other units in the squad still display in the Editor when one is selected

David Griffith 2017
 
 */

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitCursor))]
public class UnitCursorEditor : Editor
{
    private UnitCursor unitCursor;

    void OnEnable()
    {
        unitCursor = (UnitCursor)target;

        unitCursor.SetCursorPosition();
    }

    void OnDisable()
    {
        //unitCursor.Reset();
    }

    public override void OnInspectorGUI()
    {
    }

    public void OnSceneGUI()
    {
        if (unitCursor.CurrentSquad != null)
        {
            Squad squad = unitCursor.CurrentSquad;

            unitCursor.MoveUnit();

            SquadEditorGUI.ShowSquadBoundaries(squad.SquadTrigger);
            SquadEditorGUI.ShowSpawnMarkers(squad.SquadTrigger);
            SquadEditorGUI.ShowSquadTriggerMarkers(squad.SquadTrigger.SquadManager);

            Repaint();
            HandleUtility.Repaint();
        }
    }
}