/*

Description:        Unity Editor UI for ShipRotate, allowing to set up of ease - also constrains input

David Griffith 2017
 
 */

using UnityEngine;
using UnityEditor;
using DG.Tweening;

[CustomEditor(typeof(ShipRotate))]
public class ShipRotateEditor : Editor
{
    private ShipRotate shipRotate;

    public void OnEnable()
    {
        //Ensure there is a reference to the actual manager
        shipRotate = (ShipRotate)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("|Rotate|", EditorStyles.boldLabel);

        shipRotate.rotateEase = (Ease)EditorGUILayout.EnumPopup("Ease Curve", shipRotate.rotateEase);

        EditorGUILayout.Separator();

        shipRotate.rotationSpeed = EditorGUILayout.FloatField("Speed", shipRotate.rotationSpeed);

        EditorGUILayout.Separator();

        shipRotate.rotationPivotVertical = EditorGUILayout.FloatField("Max. Rotation (Y-Axis)", shipRotate.rotationPivotVertical);
        shipRotate.rotationPivotHorizontal = EditorGUILayout.FloatField("Max. Rotation (X-Axis)", shipRotate.rotationPivotHorizontal);


        if (shipRotate.rotationSpeed < Mathf.Epsilon)
        {
            shipRotate.rotationSpeed = Mathf.Epsilon;
        }

        if (shipRotate.rotationPivotVertical < 0.0f)
        {
            shipRotate.rotationPivotVertical = 0.0f;
        }
        else if (shipRotate.rotationPivotVertical > 359.0f)
        {
            shipRotate.rotationPivotVertical = 359.0f;
        }

        if (shipRotate.rotationPivotHorizontal < 0.0f)
        {
            shipRotate.rotationPivotHorizontal = 0.0f;
        }
        else if (shipRotate.rotationPivotHorizontal > 359.0f)
        {
            shipRotate.rotationPivotHorizontal = 359.0f;
        }


        if (GUI.changed)
        {
            shipRotate.ResetRotateTween();
        }
    }
}
