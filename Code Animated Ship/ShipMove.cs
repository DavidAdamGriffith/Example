/*

Description:        Moves object across on X and Y axis, with optional speed modifier

David Griffith 2017
 
 */

using UnityEngine;
using DG.Tweening;

public class ShipMove : MonoBehaviour
{
    //Player input variables - Movement
    public bool  alwaysApplyBoosts = false; //Always apply speed boosts
    
    public int   invert = -1;               //-1 for Invert, +1 for Not

    public float movementSpeed = 5.0f;      //Input movement speed multiplier
    public float movementSpeedModifier = 1.5f; //Speed factor increase when hard banking


    //Methods
    public void Move(bool applyHardBankBoost, float horizontal, float vertical, float hardBank)
    {
        //If there is input on XY axis
        if (horizontal != 0.0f || vertical != 0.0f)
        {
            //Determine target location by movement speed, and clamp by movement speed to normalize input
            Vector3 target = Vector3.ClampMagnitude(new Vector3(horizontal, vertical * invert, 0.0f) * movementSpeed, movementSpeed);

            //If a boost should be added
            if (alwaysApplyBoosts || applyHardBankBoost)
            {
                //If boosting, and in the same direction as hard banking, multiply speed by speed factor only on X plane
                if (horizontal * hardBank > 0.0f)
                {
                    target.x *= movementSpeedModifier;
                }
                //Else if boosting, and in opposite direction of hard baking, divide speed by speed factor only in X plane
                else if (horizontal * hardBank < 0.0f)
                {
                    target.x /= movementSpeedModifier;
                }
            }

            //Move relative to parent object to target, smooth over time
            transform.localPosition += target * Time.deltaTime;
        }
    }
}