/*

Description:        Animates an object 'boosting' and 'braking' on Z-axis, constrains distances and configures ease curve

David Griffith 2017
 
 */
 
 using UnityEngine;
using DG.Tweening;

public class ShipBoost : MonoBehaviour
{
    //Forward Boost
    public Ease     forwardBoostEase = Ease.Linear;

    public float	forwardBoostMaxDistance = 20.0f;            //Maximum forward boost distance
	public float	forwardBoostDuration = 1.0f;                //Time to complete the boost
    public float    forwardBoostDurationModifierTo = 1.0f;      //Speed/slow time to target 
    public float    forwardBoostDurationModifierFrom = 1.0f;    //Speed/slow time from target

    private bool	isForwardBoosting = false;

    private float	forwardBoostTargetPositionZ;


    //Brake
    public Ease     brakeEase = Ease.Linear;

    public float 	brakeMaxDistance = 5.0f;                    //Maximum backward brake distance 
	public float	brakeDuration = 1.0f;                       //Time to complete the brake
    public float    brakeDurationModifierTo = 1.0f;             //Speed/slow time to target
    public float    brakeDurationModifierFrom = 1.0f;           //Speed/slow time from target

    private bool	isBraking = false;

    private float	brakeTargetPositionZ;


    //Helpers
    private float	initialPositionZ = 0.0f;

    private Tweener forwardBoostTween;
    private Tweener brakeTween;


	void Start()
	{
		initialPositionZ = transform.localPosition.z;
		
        ResetForwardBoostTween();
        ResetBrakeTween();
    }


    //Tween Resets
    public void ResetForwardBoostTween()
    {
        if (forwardBoostTween != null)
        {
            forwardBoostTween.Kill();
        }

        forwardBoostTargetPositionZ = initialPositionZ + forwardBoostMaxDistance;

        forwardBoostTween = transform.DOMoveZ(forwardBoostTargetPositionZ, forwardBoostDuration)
                                         .SetEase(forwardBoostEase)
                                         .SetAutoKill(false);

        forwardBoostTween.Rewind();
    }

    public void ResetBrakeTween()
    {
        if (brakeTween != null)
        {
            brakeTween.Kill();
        }

        brakeTargetPositionZ = initialPositionZ - brakeMaxDistance;

        brakeTween = transform.DOMoveZ(brakeTargetPositionZ, brakeDuration)
                              .SetEase(brakeEase)
                              .SetAutoKill(false);

        brakeTween.Rewind();
    }


    //Methods
    public void Boost(float boost)
    {
        //Only boost if not braking
        if (!isBraking && forwardBoostMaxDistance > 0.0f)
        {
            //If the player is not neutral, actively going forward
            if (boost > 0.0f)
            {
                //Set isForwardBoostng to true, and tween forward
                isForwardBoosting = true;
                forwardBoostTween.timeScale = forwardBoostDurationModifierTo;
                forwardBoostTween.PlayForward();
            }
            //Else the player is not boosting, is neutral, but still not at initial Z position
            else if (transform.position.z > initialPositionZ)
            {
                //Tween backward
                forwardBoostTween.timeScale = forwardBoostDurationModifierFrom;
                forwardBoostTween.PlayBackwards();
            }
            //Else the player is not boosting, is neutral and is now at initial Z position
            else
            {
                //No longer in forward boosting animation
                isForwardBoosting = false;
            }
        }

    }

    public void Brake(float brake)
    {
        //Only brake if not boosting
        if (!isForwardBoosting && brakeMaxDistance > 0.0f)
        {
            //If the player is not neutral, actively braking
            if (brake < 0.0f)
            {
                //Set isBraking to true, and tween forward
                isBraking = true;
                brakeTween.timeScale = brakeDurationModifierTo;
                brakeTween.PlayForward();
            }
            //Else the player is not braking, is neutral, but still not at initial Z position
            else if (transform.position.z < initialPositionZ)
            {
                //Tween backward
                brakeTween.timeScale = brakeDurationModifierFrom;
                brakeTween.PlayBackwards();
            }
            //Else the player is not braking, is neutral and is now at initial Z position
            else
            {
                //No longer in braking animation
                isBraking = false;
            }
        }
    }
}
