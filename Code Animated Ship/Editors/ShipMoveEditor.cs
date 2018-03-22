/*

Description:        Unity Editor UI for ShipMove, allowing to set up of ease - also constrains input

David Griffith 2017
 
 */

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShipMove))]
public class ShipMoveEditor : Editor
{
    private ShipMove shipMove;

    public void OnEnable()
    {
        shipMove = (ShipMove)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("|Move|", EditorStyles.boldLabel);

        shipMove.alwaysApplyBoosts = EditorGUILayout.Toggle("Always Apply Speed Modifier", shipMove.alwaysApplyBoosts);

        EditorGUILayout.Separator();

        shipMove.invert = EditorGUILayout.IntField("Invert Y-Axis", shipMove.invert);

        EditorGUILayout.Separator();

        shipMove.movementSpeed = EditorGUILayout.FloatField("Speed", shipMove.movementSpeed);
        shipMove.movementSpeedModifier = EditorGUILayout.FloatField("Speed Modifier (Boost)", shipMove.movementSpeedModifier);


        if (shipMove.invert == 0)
        {
            shipMove.invert = 1;
        }
        else if (shipMove.invert < -1)
        {
            shipMove.invert = -1;
        }
        else if (shipMove.invert > 1)
        {
            shipMove.invert = 1;
        }

        if (shipMove.movementSpeed < Mathf.Epsilon)
        {
            shipMove.movementSpeed = Mathf.Epsilon;
        }

        if (shipMove.movementSpeedModifier < 1.0f)
        {
            shipMove.movementSpeedModifier = 1.0f;
        }
    }
}