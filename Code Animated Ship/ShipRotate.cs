/*

Description:        Rotates objects on horizontal and vertical axis, interpolates between position through eases, constrains angles and speed of rotation

David Griffith 2017
 
 */

using UnityEngine;
using DG.Tweening;

public class ShipRotate : MonoBehaviour
{
    public Ease     rotateEase = Ease.Linear;

    public float    rotationSpeed = 1.0f;              //Speed of rotation towards target
    public float    rotationPivotVertical = 35.0f;     //Maximum vertical degrees to rotate
    public float    rotationPivotHorizontal = 35.0f;   //Maxium horizontal degrees to rotate

    private Vector3 target;
    private Vector3 lastTarget;
    private Tweener rotateTween;


    void Start()
    {
        ResetRotateTween();
    }


    //Tween Resets
    public void ResetRotateTween()
    {
        if (rotateTween != null)
        {
            rotateTween.Kill();
        }

        target = Vector3.zero;
        lastTarget = target;

        transform.localRotation = Quaternion.Euler(target);

        rotateTween = transform.DOLocalRotate(target, 1 / rotationSpeed)
                               .SetEase(rotateEase)
                               .SetAutoKill(false);
    }


    //Methods
    public void Rotate(float vertical, float horizontal, float invert)
    {
        //Calculate rotation value of ship based on player movement
        float tiltAroundX = vertical * rotationPivotVertical * -invert;
        float tiltAroundY = horizontal * rotationPivotHorizontal;

        target = new Vector3(tiltAroundX, tiltAroundY, 0.0f);

        //Don't go any further if the target hasn't changed
        if (lastTarget == target)
        {
            return;
        }

        //Apply rotation to object
        rotateTween.ChangeEndValue(target, true).Restart();

        lastTarget = target;
    }
}
