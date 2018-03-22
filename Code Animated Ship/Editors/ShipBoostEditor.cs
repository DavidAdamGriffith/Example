/*

Description:        Unity Editor UI for ShipBoost, allowing to set up of ease - also constrains input

David Griffith 2017
 
 */

using UnityEngine;
using UnityEditor;
using DG.Tweening;

[CustomEditor(typeof(ShipBoost))]
public class ShipBoostEditor : Editor
{
    private ShipBoost shipBoost;

    public void OnEnable()
    {
        shipBoost = (ShipBoost)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("|Forward Boost|", EditorStyles.boldLabel);


        EditorGUI.BeginChangeCheck();

        shipBoost.forwardBoostEase = (Ease)EditorGUILayout.EnumPopup("Ease Curve", shipBoost.forwardBoostEase);

        EditorGUILayout.Separator();

        shipBoost.forwardBoostMaxDistance = EditorGUILayout.FloatField("Max. Distance", shipBoost.forwardBoostMaxDistance);

        EditorGUILayout.Separator();

        shipBoost.forwardBoostDuration = EditorGUILayout.FloatField("Duration", shipBoost.forwardBoostDuration);
        shipBoost.forwardBoostDurationModifierTo = EditorGUILayout.FloatField("Speed Modifier (To)", shipBoost.forwardBoostDurationModifierTo);
        shipBoost.forwardBoostDurationModifierFrom = EditorGUILayout.FloatField("Speed Modifier (Back)", shipBoost.forwardBoostDurationModifierFrom);


        if (shipBoost.forwardBoostMaxDistance < 0.0f)
        {
            shipBoost.forwardBoostMaxDistance = 0.0f;
        }

        if (shipBoost.forwardBoostDuration < Mathf.Epsilon)
        {
            shipBoost.forwardBoostDuration = Mathf.Epsilon;
        }

        if (shipBoost.forwardBoostDurationModifierTo < Mathf.Epsilon)
        {
            shipBoost.forwardBoostDurationModifierTo = Mathf.Epsilon;
        }

        if (shipBoost.forwardBoostDurationModifierFrom < Mathf.Epsilon)
        {
            shipBoost.forwardBoostDurationModifierFrom = Mathf.Epsilon;
        }

        if (EditorGUI.EndChangeCheck())
        {
            shipBoost.ResetForwardBoostTween();
        }


        EditorGUILayout.Separator();
        EditorGUILayout.Separator();


        EditorGUILayout.LabelField("|Brake|", EditorStyles.boldLabel);


        EditorGUI.BeginChangeCheck();

        shipBoost.brakeEase = (Ease)EditorGUILayout.EnumPopup("Ease Curve", shipBoost.brakeEase);

        EditorGUILayout.Separator();

        shipBoost.brakeMaxDistance = EditorGUILayout.FloatField("Max. Distance", shipBoost.brakeMaxDistance);

        EditorGUILayout.Separator();

        shipBoost.brakeDuration = EditorGUILayout.FloatField("Duration", shipBoost.brakeDuration);
        shipBoost.brakeDurationModifierTo = EditorGUILayout.FloatField("Speed Modifier (To)", shipBoost.brakeDurationModifierTo);
        shipBoost.brakeDurationModifierFrom = EditorGUILayout.FloatField("Speed Modifier (Back)", shipBoost.brakeDurationModifierFrom);


        if (shipBoost.brakeMaxDistance < 0.0f)
        {
            shipBoost.brakeMaxDistance = 0.0f;
        }

        if (shipBoost.brakeDuration < Mathf.Epsilon)
        {
            shipBoost.brakeDuration = Mathf.Epsilon;
        }

        if (shipBoost.brakeDurationModifierTo < Mathf.Epsilon)
        {
            shipBoost.brakeDurationModifierTo = Mathf.Epsilon;
        }

        if (shipBoost.brakeDurationModifierFrom < Mathf.Epsilon)
        {
            shipBoost.brakeDurationModifierFrom = Mathf.Epsilon;
        }


        if (EditorGUI.EndChangeCheck())
        {
            shipBoost.ResetBrakeTween();
        }
    }
}