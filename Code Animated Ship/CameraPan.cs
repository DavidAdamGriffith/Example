/*

Description:        Simple camera panning movement by interpolating positions over a curve

David Griffith 2016
 
 */

using UnityEngine;
using System.Collections;

public class CameraPan : MonoBehaviour
{
	/* Variables */
	
	public float panRatioX = 1.0f;
	public float panRatioY = 1.0f;
	
	private float panMaxDistanceX = 0.0f;
	private float panMaxDistanceY = 0.0f;
	
	/* Methods */
	
	public void Pan(float positionRatioX, float positionRatioY)
	{
		float targetX = Mathf.SmoothStep(0.0f, panMaxDistanceX, Mathf.Abs(positionRatioX) * panRatioX);
		float targetY = Mathf.SmoothStep(0.0f, panMaxDistanceY, Mathf.Abs(positionRatioY) * panRatioY);
	
		if(positionRatioX < 0.0f)
		{
			targetX *= -1.0f;
		}
		
		if(positionRatioY < 0.0f)
		{
			targetY *= -1.0f;
		}
		
		//Default target rotation to center
		Vector3 targetPosition = new Vector3((positionRatioX * panRatioX), (positionRatioY * panRatioY), transform.localPosition.z);

		//Apply rotation to object
		transform.localPosition = targetPosition;
	}
	
	public void SetPanMaxDistance(float maxDistanceX, float maxDistanceY)
	{
		panMaxDistanceX = maxDistanceX;
		panMaxDistanceY = maxDistanceY;
	}
}
