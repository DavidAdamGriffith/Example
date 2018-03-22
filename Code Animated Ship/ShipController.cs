/*

Description:        Controller triggering all movement and animation - plug all components in the object hierarchy in the Edtior

David Griffith 2017
 
 */

using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour
{
	/* References */

	public CameraBank	cameraBank;
	public CameraPan	cameraPan;

    private ShipBank	bank;
    private ShipRoll	roll;
	private ShipWing[]	wings;
	private ShipBoost	boost;
    private ShipRotate  rotate;
    private ShipMove    move;
	
	/* Methods */
	
	// Use this for initialization
	void Start ()
	{
        //Find and assign associated behaviours
        bank = gameObject.GetComponentInChildren<ShipBank>();
        roll = gameObject.GetComponentInChildren<ShipRoll>();
		wings = gameObject.GetComponentsInChildren<ShipWing>();
		boost = gameObject.GetComponent<ShipBoost>();
        rotate = gameObject.GetComponent<ShipRotate>();
        move = gameObject.GetComponent<ShipMove>();
		

        //
        //
		//Set camera's boundaries based on player's boundaries
		cameraPan.SetPanMaxDistance(5.0f, 5.0f);
        //
        //

        //Assign associated values to ensure animations sync up properly
        wings[0].wingDestroyed = false;
		wings[1].wingDestroyed = false;
	}
	

	// Update is called once per frame
	void Update ()
	{
		//Store player input, defaults to 0.0f to lock out controls if needed
		float hardBank = 0.0f;
		float horizontal = 0.0f;
		float vertical = 0.0f;
		
		//Get Bank, X and Y input
		hardBank = Input.GetAxis ("HardBank");
		horizontal = Input.GetAxis ("Horizontal");
		vertical = Input.GetAxis("Vertical");


        //Execute movement
        move.Move(!roll.IsRolling, horizontal, vertical, hardBank);



        //Execute rotation
        rotate.Rotate(vertical, horizontal, move.invert);

        //Execute banking
        bank.Bank(hardBank, horizontal, wings[0].wingDestroyed, wings[1].wingDestroyed, roll.IsRolling);



        //Execute forward boosting/braking
        boost.Boost(Input.GetAxis("BoostAndBrake"));
        boost.Brake(Input.GetAxis("BoostAndBrake"));

        //Execute rolling
        roll.Roll(Input.GetAxis("Roll"));


		//Execute camera banking
		cameraBank.Bank(hardBank, horizontal);
		
        //Execute camera panning
		cameraPan.Pan(transform.localPosition.x, transform.localPosition.y);
	}
}