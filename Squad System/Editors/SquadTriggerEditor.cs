/*

Description:        Editor for SquadTrigger:
                    - Simple UI for adding new squads to a trigger (one trigger > many squads)
                    - Handles navifating, adding, removing

David Griffith 2017
 
 */

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SquadTrigger))]
public class SquadTriggerEditor : Editor
{
    private SquadTrigger squadTrigger;

    public void OnEnable()
    {
        squadTrigger = (SquadTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        if (serializedObject.targetObject)
        {
            serializedObject.Update();

            //Create buttons for Adding Squads Before/After and Removing Current Squad
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("(-) Remove This Squad Trigger", EditorStyles.miniButton))
            {
                Selection.activeGameObject = squadTrigger.SquadManager.gameObject;

                squadTrigger.SquadManager.RemoveSquadTrigger(squadTrigger);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            //Create buttons that reference previous next squad in Squad Manager's list
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("< Previous Squad", EditorStyles.miniButtonLeft))
            {
                for (int i = 0; i < squadTrigger.SquadManager.SquadTriggers.Count; i++)
                {
                    if (squadTrigger.SquadManager.SquadTriggers[i].GetInstanceID() == squadTrigger.GetInstanceID())
                    {
                        if (i > 0)
                        {
                            Selection.activeGameObject = squadTrigger.SquadManager.SquadTriggers[i - 1].gameObject;
                        }

                        break;
                    }
                }
            }

            if (GUILayout.Button("Next Squad >", EditorStyles.miniButtonRight))
            {
                for (int i = 0; i < squadTrigger.SquadManager.SquadTriggers.Count; i++)
                {
                    if (squadTrigger.SquadManager.SquadTriggers[i].GetInstanceID() == squadTrigger.GetInstanceID())
                    {
                        if (i < squadTrigger.SquadManager.SquadTriggers.Count - 1)
                        {
                            Selection.activeGameObject = squadTrigger.SquadManager.SquadTriggers[i + 1].gameObject;
                        }

                        break;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();


            //Create buttons for adding and removing squads
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("(-) Remove Squad At End"))
            {
                if (squadTrigger.Squads.Count > 0)
                {
                    squadTrigger.SquadManager.RemoveSquadAtEnd(squadTrigger);
                }
            }

            if (GUILayout.Button("(+) Add Squad At End"))
            {
                squadTrigger.SquadManager.AddSquadAtEnd(squadTrigger);
            }
            EditorGUILayout.EndHorizontal();

            //If there are squads to display
            if (squadTrigger.Squads.Count > 0)
            {
                //Iterate through the list of squads and display their properties
                for (int i = 0; i < squadTrigger.Squads.Count; i++)
                {
                    EditorGUILayout.Separator();
                    EditorGUILayout.Separator();
                    EditorGUILayout.Separator();

                    Squad squad = squadTrigger.Squads[i];

                    if (squad)
                    {
                        if (GUILayout.Button(squad.name))
                        {
                            //Make squad active GameObject in Hierarchy
                            Selection.activeGameObject = squad.gameObject;
                        }

                        //Create buttons for adding Squad After/Before and Removing a Squad
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("(+) Add Squad After", EditorStyles.miniButtonLeft))
                        {
                            squadTrigger.SquadManager.AddSquadAfter(squad);
                        }

                        if (GUILayout.Button("(+) Add Squad Before", EditorStyles.miniButtonMid))
                        {
                            squadTrigger.SquadManager.AddSquadBefore(squad);
                        }

                        if (GUILayout.Button("(-) Remove This Squad", EditorStyles.miniButtonRight))
                        {
                            squadTrigger.SquadManager.RemoveSquad(squad);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }

            if (squadTrigger)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

    public void OnSceneGUI()
    {
        UpdateEditor();
    }

    //Update the Editor
    private void UpdateEditor()
    {
        if (squadTrigger)
        {
            if (squadTrigger.SquadManager.alignSquadTriggers)
            {
                SquadEditorGUI.ShowAlignedSquadTriggerAxis(squadTrigger.SquadManager);
            }

            SquadEditorGUI.ShowOtherSquadBoundaries(squadTrigger);
            SquadEditorGUI.ShowSpawnMarkers(squadTrigger);
            SquadEditorGUI.ShowSquadTriggerMarkers(squadTrigger.SquadManager);

            Repaint();
            HandleUtility.Repaint();
        }
    }
}
