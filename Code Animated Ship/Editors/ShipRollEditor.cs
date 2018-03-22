/*

Description:        Unity Editor UI for ShipRoll, allowing to set up of ease - also constrains input

David Griffith 2017
 
 */

using UnityEngine;
using UnityEditor;
using DG.Tweening;

[CustomEditor(typeof(ShipRoll))]
public class ShipRollEditor : Editor
{
    private ShipRoll shipRoll;

    public void OnEnable()
    {
        //Ensure there is a reference to the actual manager
        shipRoll = (ShipRoll)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("|Roll|", EditorStyles.boldLabel);

        shipRoll.rollEase = (Ease)EditorGUILayout.EnumPopup("Ease Curve", shipRoll.rollEase);

        EditorGUILayout.Separator();

        shipRoll.numRolls = EditorGUILayout.IntField("Num. Rolls", shipRoll.numRolls);

        EditorGUILayout.Separator();

        shipRoll.rollDuration = EditorGUILayout.FloatField("Duration", shipRoll.rollDuration);


        if (shipRoll.numRolls < 0)
        {
            shipRoll.numRolls = 0;
        }

        if (shipRoll.rollDuration < Mathf.Epsilon)
        {
            shipRoll.rollDuration = Mathf.Epsilon;
        }


        if (GUI.changed)
        {
            shipRoll.ResetRollTween();
        }
    }
}