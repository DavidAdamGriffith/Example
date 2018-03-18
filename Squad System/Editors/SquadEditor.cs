/*

Description:        Editor for Squad:
                    - Squads will appear in Unity Editor UI for parameter set-up
                    - Allows for the moving of individual units (fast - only working with Vectors here!)
                    - Navigation buttons for moving in between Squads in the list
                    - Addition/removal options for new squads before and after

David Griffith 2017
 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Squad))]
public class SquadEditor : Editor
{
    private Squad squad;

    public void OnEnable()
    {
        //Ensure there is a reference to the actual manager
        squad = (Squad)target;
    }

    public override void OnInspectorGUI()
    {
        if (serializedObject.targetObject)
        {
            //Update the serialized object
            serializedObject.Update();

            //Create buttons for Adding Squads Before/After and Removing Current Squad
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("(+) Add Squad Before", EditorStyles.miniButtonLeft))
            {
                Selection.activeGameObject = squad.SquadTrigger.SquadManager.AddSquadBefore(squad).gameObject;
            }

            if (GUILayout.Button("(+) Add Squad After", EditorStyles.miniButtonMid))
            {
                Selection.activeGameObject = squad.SquadTrigger.SquadManager.AddSquadAfter(squad).gameObject;
            }

            if (GUILayout.Button("(-) Remove This Squad", EditorStyles.miniButtonRight))
            {
                Selection.activeGameObject = squad.SquadTrigger.gameObject;

                squad.SquadTrigger.SquadManager.RemoveSquad(squad);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.Separator();


            //Create buttons that reference previous next squad in Squad Manager's list
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("< Previous Squad", EditorStyles.miniButtonLeft))
            {
                for (int i = 0; i < squad.SquadTrigger.Squads.Count; i++)
                {
                    if (squad.SquadTrigger.Squads[i].GetInstanceID() == squad.GetInstanceID())
                    {
                        if (i > 0)
                        {
                            Selection.activeGameObject = squad.SquadTrigger.Squads[i - 1].gameObject;
                        }

                        break;
                    }
                }
            }

            if (GUILayout.Button("Next Squad >", EditorStyles.miniButtonRight))
            {
                for (int i = 0; i < squad.SquadTrigger.Squads.Count; i++)
                {
                    if (squad.SquadTrigger.Squads[i].GetInstanceID() == squad.GetInstanceID())
                    {
                        if (i < squad.SquadTrigger.Squads.Count - 1)
                        {
                            Selection.activeGameObject = squad.SquadTrigger.Squads[i + 1].gameObject;
                        }

                        break;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();


            //Set squad's formation from dropdown
            squad.formation = (SquadFormation.Formation)EditorGUILayout.EnumPopup("Formation", squad.formation);

            //Set basic squad parameters
            squad.numObjects = EditorGUILayout.IntField("# Objects", Mathf.Clamp(squad.numObjects, 1, squad.numObjects));

            squad.spawnDistance = EditorGUILayout.FloatField("Spawn Distance", squad.spawnDistance);


            //Hide special properties unless related spawn formation is selected
            //Properties for # Sides if Polygon/Spoke, Wedge Angle if Wedge
            switch (squad.formation)
            {
                case SquadFormation.Formation.Polygon2D:
                    squad.numGeometryFaces = EditorGUILayout.IntField("# Sides", squad.numGeometryFaces);
                    break;

                case SquadFormation.Formation.Wedge:
                    squad.wedgeAngle = EditorGUILayout.FloatField("Wedge Angle", squad.wedgeAngle);
                    break;

                case SquadFormation.Formation.Spoke:
                    squad.numGeometryFaces = EditorGUILayout.IntField("# Spokes", squad.numGeometryFaces);
                    break;

                default:
                    break;
            }


            EditorGUILayout.Separator();

            //Set squad's unit distribution from dropdown
            squad.distribution = (Squad.SquadDistribution)EditorGUILayout.EnumPopup("Distribution", squad.distribution);

            //Select defauly prefab to use for spawning
            squad.defaultPrefab = (GameObject)EditorGUILayout.ObjectField("Default Prefab", squad.defaultPrefab, typeof(GameObject), false);


            //Hide special properites unless related spawn distribution is selected
            //Properties for Alternate Leader Prefab if Alternate Leader, List Options if Custom
            switch (squad.distribution)
            {
                case Squad.SquadDistribution.AlternateLeader:

                    squad.alternateLeaderPrefab = (GameObject)EditorGUILayout.ObjectField("Alternate Leader Prefab", squad.alternateLeaderPrefab, typeof(GameObject), false);
                    break;

                case Squad.SquadDistribution.Custom:

                    EditorGUILayout.Separator();


                    EditorGUILayout.BeginHorizontal();
                    //Remove last unit
                    if (GUILayout.Button("(-) Remove Unit at End", EditorStyles.miniButtonLeft))
                    {
                        if (squad.customPrefabs.Count > 0)
                        {
                            squad.customPrefabs.RemoveAt(squad.customPrefabs.Count - 1);
                        }
                    }

                    //Add unit at end
                    if (GUILayout.Button("(+) Add Unit at End", EditorStyles.miniButtonRight))
                    {
                        //Ensure new unit is null
                        squad.customPrefabs.Add(null);
                    }
                    EditorGUILayout.EndHorizontal();

                    int i = 0;

                    //Iterate through each gameObject in the squad's custom prefabs
                    foreach (GameObject gameObject in squad.customPrefabs)
                    {
                        //Display object field for each object
                        squad.customPrefabs[i] = (GameObject)EditorGUILayout.ObjectField("Unit " + i, squad.customPrefabs[i], typeof(GameObject), false);

                        i++;
                    }
                    break;

                default:
                    break;
            }

            if (squad)
            {
                serializedObject.ApplyModifiedProperties();

                if (GUI.changed)
                {
                    squad.SetPositions();
                }
            }
        }
    }

    public void OnSceneGUI()
    {
        UpdateEditor();
    }

    //Call to update the editor
    public void UpdateEditor()
    {
        if (squad)
        {
            SquadEditorGUI.ShowSquadBoundaries(squad.SquadTrigger);
            SquadEditorGUI.ShowSpawnMarkers(squad.SquadTrigger);
            SquadEditorGUI.ShowSquadTriggerMarkers(squad.SquadTrigger.SquadManager);
            
            Repaint();
            HandleUtility.Repaint();
        }
    }
}  
