/*

Description:        Applies a 'banking' rotation to object around Z-axis, with an options for a jerkier rotation, configures ease curve and speed

David Griffith 2017
 
 */

using UnityEngine;
using DG.Tweening;

public class ShipBank : MonoBehaviour
{
    public Ease     bankEase = Ease.Linear;

    //Banking (Rotation) variables
    public float 	bankAngle = 35.0f;                  //Default banking angle, when player is moving
	public float 	hardBankAngle = 90.0f;              //Hard baking angle, when player is Hard Banking left/right
	public float 	bankAngleMax = 90.0f;               //Maximum angle the ship can bank at
	public float 	bankRotationSpeed = 2.0f;           //Speed of time to target
    public float 	bankRotationSlowModifier = 0.5f;   //Modify time to target

    private Vector3 target;
    private Vector3 lastTarget;

    private Tweener bankTween;


    void Start()
    {
        ResetBankTween();
    }


    //Tween Resets
    public void ResetBankTween()
    {
        if (bankTween != null)
        {
            bankTween.Kill();
        }

        target = Vector3.zero;
        lastTarget = target;

        transform.localRotation = Quaternion.Euler(target);

        bankTween = transform.DOLocalRotate(target, 1.0f)
                             .SetEase(bankEase)
                             .SetAutoKill(false);
    }


    //Methods
    public void Bank(float hardBank, float horizontal, bool leftWingDestroyed, bool rightWingDestroyed, bool interrupt)
	{
        if (!interrupt)
        {
            /* Currently Aesthetic Only */
            //If a wing was destroyed, slow banking speed by changing tweens in opposite direction of hard bank, and of turn
            if (leftWingDestroyed && (hardBank > 0.0f || horizontal > 0.0f))
            {
                bankTween.timeScale = bankRotationSpeed * bankRotationSlowModifier;
            }
            else if (rightWingDestroyed && (hardBank < 0.0f || horizontal < 0.0f))
            {
                bankTween.timeScale = bankRotationSpeed * bankRotationSlowModifier;
            }
            else
            {
                bankTween.timeScale = bankRotationSpeed;
            }
            

            //Determine angle to bank around Z-axis with
            float bankAroundZ = Mathf.Clamp(((horizontal * bankAngle) + (hardBank * hardBankAngle)), -bankAngleMax, bankAngleMax);

            //Correct angle to bank at, if hard-banking the opposite direction of movement (banking from movement is neglected)
            if (hardBank / horizontal < 0.0f)
            {
                bankAroundZ -= horizontal * bankAngle;
            }

            target = new Vector3(0.0f, 0.0f, -bankAroundZ);

            if (lastTarget == target)
            {
                return;
            }


            //Apply rotation to object
            bankTween.ChangeEndValue(target, true).Restart();

            lastTarget = target;
        }
    }
}
