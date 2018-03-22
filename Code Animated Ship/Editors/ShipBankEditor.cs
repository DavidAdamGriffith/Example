/*

Description:        Unity Editor UI for ShipBank, allowing to set up of ease - also constrains input

David Griffith 2017
 
 */

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShipBank))]
public class ShipBankEditor : Editor
{
    private ShipBank shipBank;

    public void OnEnable()
    {
        shipBank = (ShipBank)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("|Bank|", EditorStyles.boldLabel);

        shipBank.bankAngle = EditorGUILayout.FloatField("Banking Angle", shipBank.bankAngle);
        shipBank.hardBankAngle = EditorGUILayout.FloatField("Hard-Banking Angle", shipBank.hardBankAngle);
        shipBank.bankAngleMax = EditorGUILayout.FloatField("Max. Total Banking Angle", shipBank.bankAngleMax);

        EditorGUILayout.Separator();

        shipBank.bankRotationSpeed = EditorGUILayout.FloatField("Rotation Speed", shipBank.bankRotationSpeed);
        shipBank.bankRotationSlowModifier = EditorGUILayout.FloatField("Rotation Slow Modifier", shipBank.bankRotationSlowModifier);


        if (shipBank.bankAngle < 0.0f)
        {
            shipBank.bankAngle = 0.0f;
        }
        else if (shipBank.bankAngle >= 359.0f)
        {
            shipBank.bankAngle = 359.0f;
        }

        if (shipBank.hardBankAngle < 0.0f)
        {
            shipBank.hardBankAngle = 0.0f;
        }
        else if (shipBank.hardBankAngle > 359.0f)
        {
            shipBank.hardBankAngle = 359.0f;
        }

        if (shipBank.bankAngleMax < 0.0f)
        {
            shipBank.bankAngleMax = 0.0f;
        }
        else if (shipBank.bankAngleMax > 359.0f)
        {
            shipBank.bankAngleMax = 359.0f;
        }

        if (shipBank.bankRotationSpeed < Mathf.Epsilon)
        {
            shipBank.bankRotationSpeed = Mathf.Epsilon;
        }

        if (shipBank.bankRotationSlowModifier < Mathf.Epsilon)
        {
            shipBank.bankAngle = Mathf.Epsilon;
        }
        else if (shipBank.bankRotationSlowModifier > 1.0f)
        {
            shipBank.bankRotationSlowModifier = 1.0f;
        }


        if (GUI.changed)
        {
            shipBank.ResetBankTween();
        }
    }
}