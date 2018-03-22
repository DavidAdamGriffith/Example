/*

Description:        Camera banking script, to better follow animated object

David Griffith 2017
 
 */

using UnityEngine;
using System.Collections;

public class CameraBank : MonoBehaviour
{
	/* Variables */
	
	public float bankAngle = 12.5f;
	public float bankRotationSpeed = 0.0325f;
	
	
	/* Methods */
			
	public void Bank(float hardBank, float horizontal)
	{
		//Default target rotation to center
		Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
		
		//Only apply banking if not hard-banking (to reduce nausea)
		if(hardBank == 0.0f)
		{
			//Determine angle to bank around Z-axis with
			float bankAroundZ = Mathf.Clamp((horizontal * bankAngle), -bankAngle, bankAngle);
			
			//Determine final rotation
			targetRotation = Quaternion.Euler(0.0f, 0.0f, -bankAroundZ);
		}
		
		//Apply rotation to object
		transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, bankRotationSpeed);
	}
}
