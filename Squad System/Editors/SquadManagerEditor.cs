/*

Description:        Editor for SquadManager:
                    - Provides UI access to add/remove Triggers for squads
                    - Buttons and 'paginated' options
                    - Provides auto-alignment for triggers (in case things get messy in the Editor)

David Griffith 2017
 
 */
 
 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


[CustomEditor(typeof(SquadManager))]
public class SquadManagerEditor : Editor
{
    private SquadManager squadManager;

    public void OnEnable()
    {
        squadManager = (SquadManager)target;
    }

    public override void OnInspectorGUI()
    {
        if (serializedObject.targetObject)
        {
            serializedObject.Update();

            EditorGUILayout.Separator();

            squadManager.spawnAreaObject = (GameObject)EditorGUILayout.ObjectField("Spawn Area: ", squadManager.spawnAreaObject, typeof(GameObject), true);

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            if (squadManager.spawnAreaObject)
            {
                squadManager.alignSquadTriggers = EditorGUILayout.Toggle("Align Squads on Z-Axis", squadManager.alignSquadTriggers);

                EditorGUILayout.Separator();
                EditorGUILayout.Separator();

                //Create buttons for adding and removing squad triggers
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("(-) Remove Squad Trigger At End"))
                {
                    if (squadManager.SquadTriggers.Count > 0)
                    {
                        squadManager.RemoveSquadTriggerAtEnd();
                    }
                }

                if (GUILayout.Button("(+) Add Squad Trigger At End"))
                {
                    squadManager.AddSquadTriggerAtEnd();
                }
                EditorGUILayout.EndHorizontal();


                //If there are squad triggers to display
                if (squadManager.SquadTriggers.Count > 0)
                {
                    //Iterate through the list of squad triggers
                    for (int i = 0; i < squadManager.SquadTriggers.Count; i++)
                    {
                        EditorGUILayout.Separator();
                        EditorGUILayout.Separator();
                        EditorGUILayout.Separator();

                        SquadTrigger squadTrigger = squadManager.SquadTriggers[i];

                        if (squadTrigger)
                        {
                            if (GUILayout.Button(squadTrigger.name))
                            {
                                //Make squad trigger active GameObject in Hierarchy
                                Selection.activeGameObject = squadTrigger.gameObject;
                            }

                            //Create buttons for adding Squad Trigger After/Before and Removing a Squad Trigger
                            EditorGUILayout.BeginHorizontal();
                            if (GUILayout.Button("(-) Remove This Squad Trigger", EditorStyles.miniButton))
                            {
                                squadManager.RemoveSquadTrigger(squadTrigger);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }

                if (squadManager)
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }

    public void OnSceneGUI()
    {
        UpdateEditor();
    }

    //Updates the Editor
    private void UpdateEditor()
    {
        if (squadManager)
        {
            if (squadManager.alignSquadTriggers)
            {
                SquadEditorGUI.ShowAlignedSquadTriggerAxis(squadManager);
            }

            SquadEditorGUI.ShowSpawnMarkers(squadManager);
            SquadEditorGUI.ShowSquadTriggerMarkers(squadManager);

            Repaint();
            HandleUtility.Repaint();
        }
    }
}

