/*

Description:        Rotates objects on Z-axis ('Aileron Roll') for one 360-degree turn, variable speed and number of rotations

David Griffith 2017
 
 */

using UnityEngine;
using DG.Tweening;

public class ShipRoll : MonoBehaviour
{
    public Ease     rollEase = Ease.Linear;

	public int 		numRolls = 1;           //Number of rolls to complete

    public float    rollDuration = 1.0f;    //Duration of rolling


    private bool    isRolling = false;      //Indicates if ship is rolling
    public bool     IsRolling
    {
        get
        {
            return isRolling;
        }
    }

    private Vector3 target = Vector3.zero;

    private Tweener rollTween;


    void Start()
    {
        ResetRollTween();
    }


    //Tween Resets
    public void ResetRollTween()
    {
        if (rollTween != null)
        {
            rollTween.Kill();
        }

        isRolling = false;

        target = Vector3.zero;

        transform.localRotation = Quaternion.Euler(target);

        rollTween = transform.DOLocalRotate(target, rollDuration, RotateMode.FastBeyond360)
                             .SetEase(rollEase)
                             .SetAutoKill(false)
                             .OnComplete(() => isRolling = false)
                             .Pause();
    }


    //Methods
    public void Roll(float direction)
    {
        //If not already rolling
        if (!isRolling && numRolls > 0)
        {
            //If the direction is not neutral
            if (!Mathf.Approximately(direction, 0.0f))
            {
                isRolling = true;

                //Choose target rotation of full circle
                float rotation = 360.0f;

                //Correct for dierection of roll based on direction
                if (direction > 0.0f)
                {
                    rotation = -360.0f;
                }

                //Choose target rotation based on the number of rolls
                target = new Vector3(0.0f, 0.0f, rotation * numRolls);

                rollTween.ChangeEndValue(target, true).Restart();

                target = Vector3.zero;
            }
        }
    }
}
