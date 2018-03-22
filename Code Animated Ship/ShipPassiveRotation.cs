/*

Description:        Rotates objects on Z-axis across an infinite domain curve, emulates a 'slow wobble' on one axis

                    NOTE:   Apply only to geometry's parent to separate passive rotations from active, 
                            otherwise all rotation will be taken into account for the player movement
                            and result in unexpected behaviour

David Griffith 2017
 
 */


using UnityEngine;
using System.Collections;

public class ShipPassiveRotation : MonoBehaviour
{
	/* Variables */

	public float maxRotationDegrees = 2.0f;
	public float period = 0.4f;	
	
	
	/* Methods */
	
	// Update is called once per frame
	void Update ()
	{
		//Rotate object about Z-axis using with sinusoidal motion
		transform.localRotation = Quaternion.Euler (0.0f, 0.0f, ShipMovementEase.BreatheLerp(maxRotationDegrees, period, Time.timeSinceLevelLoad));
	}
}
