/*

Description:        Generates a 2D shape pattern of spawn positions (Vector List):
                    - Poly (3 - x sides)
                    - Circle
                    - Wedge (or Vee)
                    - Spoke
                    - Line
                    - Custom (placeholder to accomodate manual unit placement)

                    Optionally links the objects' reference to one another per game rules (Linked List)

David Griffith 2017
 
 */


using UnityEngine;

public static class SquadFormation
{
	public enum Formation
	{
		Polygon2D,
		Circular,
		Wedge,
		Spoke,
		Line,
        Custom,
        COUNT
	}
	
	//Returns appropriate formation based on input
	public static Vector3[] PickFormation(Formation formation, 
	                                      int numObjects,
	                                      float spawnDistance, 
	                                      int numGeometryFaces, 
	                                      float wedgeAngle)
	{
		switch(formation)
		{
		case Formation.Polygon2D:
			return Polygon2D(numObjects, spawnDistance, numGeometryFaces);
			
		case Formation.Circular:
			return Circular(numObjects, spawnDistance);
			
		case Formation.Wedge:
			return Wedge(numObjects, spawnDistance, wedgeAngle);
			
		case Formation.Spoke:
			return Spoke(numObjects, spawnDistance, numGeometryFaces);
			
		case Formation.Line:
			return Line (numObjects, spawnDistance);
			
		default:
			break;
		}
		
		return null;
	}
	

	#region Formations
	//Determine where to spawn prefabs in a 2D Polygon pattern
	//Corrects for numSides minimum of 1 side
	//Corrects for minmum of numSides if too litte, and rounds down if too many
	public static Vector3[] Polygon2D(int numObjects,
									  float spawnDistance, 
									  int numGeometryFaces)
	{
		//Checks to enforce the number of sides is above 2
		//Ensures default position will always be returned
		if(numGeometryFaces < 3)
		{
			numGeometryFaces = 3;
		}
		
		
		//Calculate the starting angle to rotate about based on the number of sides
		float exteriorAngle = 180.0f - ((((numGeometryFaces - 3) * 180.0f)  + 180.0f) / numGeometryFaces);
		
		
		//Checks to enforce the number of objects to spawn against the number of sides of geometry divisibe
		//Ensures integer values for enemies per-side, with at least 1 per side
		if(numObjects <= 0 || numObjects % numGeometryFaces != 0)
		{
			if(numObjects < numGeometryFaces)
			{
				numObjects = numGeometryFaces;
			}
			else
			{
				numObjects -= numObjects % numGeometryFaces;
			}
		}
		
		
		//Create position cursor at (0, 0, 0) to locate and hold target position
		Vector3 cursor = Vector3.zero;
		
		//Determine initial rotation based on rotationOffset plus 1/2 exterior angle of the shape, rotated about 180 degrees
		//Ensures default spawn positions (when rotationOffset == 0) are pointing upwards
		float cursorRotation = (180 - (180 - exteriorAngle) / 2.0f);
		
		//Create an array of enemy spawn locations and formation locations
		Vector3[] formationLocations = new Vector3[numObjects];
		
		//Determine spawn locations in 2D-polygon formation
		for(int count = numObjects - 1; count >= 0; count--)
		{
			//Only update position after first iteration
			if(count < numObjects)
			{
				//Add the next position's X and Y values to the cursor
				cursor.x += spawnDistance * Mathf.Sin((cursorRotation + exteriorAngle * Mathf.Ceil(count / (numObjects / numGeometryFaces))) * Mathf.Deg2Rad);
				cursor.y += spawnDistance * Mathf.Cos((cursorRotation + exteriorAngle * Mathf.Ceil(count / (numObjects / numGeometryFaces))) * Mathf.Deg2Rad);
			}
			
			//Apply rotation for formation location, and add to array
			formationLocations[count] = cursor;
		}
		
		return formationLocations;
	}
	
	
	
	//Determine where to spawn prefabs in a Circular pattern
	//Corrects for minimum of 1 numObjects if too little
	public static Vector3[] Circular(int numObjects,
									 float spawnDistance)
	{	
		//Checks to enforce the number of sides is above 2
		//Ensures default positions will always be returned
		if(numObjects < 3)
		{
			numObjects = 3;
		}
		
		
		float separationAngle = 360.0f / numObjects;
		
		//Set the spawnDistance based on the distance between objects and the separationAngle
		spawnDistance = (spawnDistance / 2.0f) / Mathf.Cos ((90.0f - (separationAngle / 2.0f)) * Mathf.Deg2Rad);
		
		
		//Create position cursor at (0, 0, 0) to locate and hold target position 
		Vector3 cursor = Vector3.zero;
		
		//Create an array of enemy spawn locations and formation locations
		Vector3[] formationLocations = new Vector3[numObjects];
		
		//Determine spawn locations in circular formation
		for (int count = 0; count < numObjects; count++)
		{
			//Create the spawn position based on angle
			//  *Will not work where numObjects = 2 as Sin(0) == Sin(180) and Cos(0) == Cos(180)
			cursor.x = spawnDistance * Mathf.Sin((separationAngle * count) * Mathf.Deg2Rad);
			cursor.y = spawnDistance * Mathf.Cos((separationAngle * count) * Mathf.Deg2Rad);
			
			//Apply rotation for formation location, and add to array
			formationLocations[count] = cursor;
		}
		
		return formationLocations;
	}
	
	
	
	//Determine where to spawn prefabs in a Wedge (Vee) pattern
	////Corrects for minimum of 1 numObjects if too little, and rounds down if too many
	public static Vector3[] Wedge(int numObjects,
								  float spawnDistance, 
								  float wedgeAngle)
	{
		wedgeAngle = wedgeAngle / 2.0f;	//Divide wedgeAngle by 2 as angle will be reflected (doubled) if not
		
		
		//Use default location of 1 degree if the wedge angle is invalid
		if(wedgeAngle % 360 == 0)
		{
			wedgeAngle = 1.0f;
		}
		
		//Checks and enforces projected distance between objects on spokes is at least equal to spawnDistance
		//Ensures the side-to-side distance will always be >= spawnDistance by increasing it to a larger size if need be
		if(spawnDistance * Mathf.Cos ((90.0f - wedgeAngle) * Mathf.Deg2Rad) < spawnDistance / 2.0f)
		{
			//Set the spawn distance based on the distance between objects and the separation angle
			spawnDistance = (spawnDistance / 2.0f) / Mathf.Cos ((90.0f - wedgeAngle) * Mathf.Deg2Rad);
		}
		
		//Checks to enforce the number of objects spawned is not divisible by 2
		//Ensures shape will be balanced, with at least 1 object
		if (numObjects % 2 == 0)
		{
			if(numObjects < 1)
			{
				numObjects = 1;
			}
			else
			{
				numObjects--;
			}
		}
		
		
		//Create position cursor at (0, 0, 0) to locate and hold target position
		Vector3 cursor = Vector3.zero;
		
		//Create an array of enemy spawn locations and formation locations
		Vector3[] formationLocations = new Vector3[numObjects];
		
		//Determine spawn locations for enemies in wedge formation
		for (int count = 0; count < numObjects; count++)
		{
			//Only move cursor after first iteration
			if(count > 0)
			{
				//Calculate next position on X, Y planes
				cursor.x += spawnDistance * Mathf.Sin(wedgeAngle * Mathf.Deg2Rad);
				cursor.y += spawnDistance * Mathf.Cos(wedgeAngle * Mathf.Deg2Rad);
				
				//Apply rotation for formation location, and add to array
				formationLocations[count] = Quaternion.AngleAxis(0.0f, Vector3.forward) * cursor;
				formationLocations[count + 1] = Quaternion.AngleAxis(0.0f, Vector3.forward) * Vector3.Reflect(cursor, Vector3.right);
				
				count++;
			}
			else
			{
				//Apply rotation for formation location, and add to array
				formationLocations[count] = cursor;
			}
		}

        return formationLocations;
    }
	
	
	
	//Determine where to spawn prefabs in a Spoke pattern
	//Expects an integer (> 2) value for numSides, corrects for minimum of 1 if too little and rounds down if too many
	//numObjects - 1 should be divisible by numSides
	public static Vector3[] Spoke(int numObjects,
								  float spawnDistance, 
								  int numGeometryFaces)
	{
		//Return default location if under 2 sides
		if(numGeometryFaces < 2)
		{
			numGeometryFaces = 2;
		}
		
		
		float separationAngle = 360.0f / numGeometryFaces;
		
		
		//Checks and enforces projected distance between objects on spokes is at least equal to spawnDistance
		//Ensures the side-to-side distance will always be >= spawnDistance by increasing it to a larger size if need be
		if(spawnDistance * Mathf.Cos ((90.0f - (separationAngle / 2.0f)) * Mathf.Deg2Rad) < spawnDistance / 2.0f)
		{
			//Set the spawn distance based on the distance between objects and the separation angle
			spawnDistance = (spawnDistance / 2.0f) / Mathf.Cos ((90.0f - (separationAngle / 2.0f)) * Mathf.Deg2Rad);
		}
		
		//Checks to enforce the number of objects after the first (middle) is divisble by the number of sides
		//Ensures shape will be balanced, with at least 1 object in center
		if ((numObjects - 1) % numGeometryFaces != 0)
		{
			if(numObjects < (numGeometryFaces + 1))
			{
				numObjects = 1;
			}
			else
			{
				numObjects -= (numObjects - 1) % numGeometryFaces;
			}
		}
		
		
		//Create position cursor at (0, 0, 0) to locate and hold target position 
		Vector3 cursor = Vector3.zero;
		
		//Create an array of enemy spawn locations and formation locations
		Vector3[] formationLocations = new Vector3[numObjects];
		
		//Spawn enemies in spoke formation
		for (int count = 0; count < numObjects; count++)
		{
			//Only move cursor after first iteration
			if(count > 0)
			{
				//Create the spawn positions based on separation angle
				cursor.x = spawnDistance * Mathf.Ceil((float)count / (float)numGeometryFaces) * Mathf.Sin((separationAngle * (count % numGeometryFaces + 1)) * Mathf.Deg2Rad);
				cursor.y = spawnDistance * Mathf.Ceil((float)count / (float)numGeometryFaces) * Mathf.Cos((separationAngle * (count % numGeometryFaces + 1)) * Mathf.Deg2Rad);
			}
			
			//Apply rotation for formation location, and add to array
			formationLocations[count] = cursor;
		}
		
		return formationLocations;
	}
	
	
	
	//Determine where to spawn prefabs in a Line pattern
	//Expects an integer (> 0) value for numObjects, corrects for minimum of 1 if too little
	public static Vector3[] Line(int numObjects,
								 float spawnDistance)
	{
		//Return default location if under 2 objects
		if(numObjects < 1)
		{
			numObjects = 1;
		}
		
		
		//Create position cursor at (0, 0, 0) to locate and hold target position 
		Vector3 cursor = Vector3.zero;
		
		//Create an array of enemy spawn locations and formation locations
		Vector3[] formationLocations = new Vector3[numObjects];
		
		//Determine spawn locations for enemies in line formation
		for (int count = 0; count < numObjects; count++)
		{
			//Create the spawn positions based on rotation offset angle
			cursor.x = count * spawnDistance;
			cursor.y = count * spawnDistance;
			
			//Apply rotation for formation location, and add to array
			formationLocations[count] = cursor;
		}

		
		return formationLocations;
	}
    #endregion

    #region Link Formations
    //Links Enemies together appropriately based on Formation
    //Should be called after initializing spawn has complete, on array of spawned enemies
    public static void LinkFormation(Enemy[] enemies, Formation formation, int numGeometryFaces)
    {
        switch (formation)
        {
            case Formation.Polygon2D:
                LinkFormationSplit(enemies);
                break;

            case Formation.Circular:
                LinkFormationSplit(enemies);
                break;

            case Formation.Wedge:
                LinkFormationBranched(enemies, 2);
                break;

            case Formation.Spoke:
                LinkFormationBranched(enemies, numGeometryFaces);
                break;

            case Formation.Line:
                LinkFormationLinear(enemies);
                break;

            default:
                break;
        }
    }

    private static void LinkFormationLinear(Enemy[] enemies)
    {
        for (int count = 0; count < enemies.Length; count++)
        {
            if (count == 0)
            {
                enemies[count].PrevInFormation = null;

                if (enemies.Length > 1)
                {
                    enemies[count].NextInFormation = new Enemy[1];
                    enemies[count].NextInFormation[0] = enemies[count + 1];
                }

                enemies[count].FormationSpacing = 0.0f;
            }
            else if (count == enemies.Length - 1)
            {
                if (enemies.Length > 1)
                {
                    enemies[count].PrevInFormation = enemies[count - 1];

                    enemies[count].NextInFormation = null;
                }

                enemies[count].FormationSpacing = Vector3.Distance(enemies[count].transform.position, enemies[count].PrevInFormation.transform.position);
            }
            else
            {
                enemies[count].PrevInFormation = enemies[count - 1];

                enemies[count].NextInFormation = new Enemy[1];
                enemies[count].NextInFormation[0] = enemies[count + 1];

                enemies[count].FormationSpacing = Vector3.Distance(enemies[count].transform.position, enemies[count].PrevInFormation.transform.position);
            }
        }
    }



    private static void LinkFormationBranched(Enemy[] enemies, int numBranches)
    {
        for (int count = 0; count < enemies.Length; count++)
        {
            if (count == 0)
            {
                enemies[count].PrevInFormation = null;

                //If there are at least 3 enemies, add the first of each spoke as next in formation
                if (enemies.Length > 2)
                {
                    enemies[count].NextInFormation = new Enemy[numBranches];

                    for (int i = 0; i < numBranches; i++)
                    {
                        enemies[count].NextInFormation[i] = enemies[i + 1];
                    }
                }

                enemies[count].FormationSpacing = 0.0f;
            }
            else
            {
                //If past the first element on a 'branch' or 'arm'
                if (count > numBranches)
                {
                    //Add the previous element on the 'branch' or 'arm' as the previous in formation
                    enemies[count].PrevInFormation = enemies[count - numBranches];
                }
                else
                {
                    //Add the first element (leader) as previous in formation
                    enemies[count].PrevInFormation = enemies[0];
                }

                //If in the last element on a 'branch' or 'arm'
                if (count > (enemies.Length - 1) - numBranches)
                {
                    //Last on 'branch' or 'arm', set next in formation to null
                    enemies[count].NextInFormation = null;
                }
                else
                {
                    //Add the next element on the 'branch' or 'arm' as the next in formation
                    enemies[count].NextInFormation = new Enemy[1];
                    enemies[count].NextInFormation[0] = enemies[count + numBranches];
                }

                enemies[count].FormationSpacing = Vector3.Distance(enemies[count].transform.position, enemies[count].PrevInFormation.transform.position);
            }
        }
    }



    private static void LinkFormationSplit(Enemy[] enemies)
    {

        for (int count = 0; count < enemies.Length; count++)
        {
            if (count == 0)
            {
                enemies[count].PrevInFormation = null;

                //If at least 3 elements, add next element and last element as next in formation
                if (enemies.Length > 2)
                {
                    enemies[count].NextInFormation = new Enemy[2];
                    enemies[count].NextInFormation[0] = enemies[count + 1];
                    enemies[count].NextInFormation[1] = enemies[enemies.Length - 1];
                }

                enemies[count].FormationSpacing = 0.0f;
            }
            //If in latter half of array (includes bottom point if even number of elements)
            else if (count >= enemies.Length / 2.0f)
            {
                if (count == enemies.Length - 1)
                {
                    //If on the last element, add the first enemy as previous in formation
                    enemies[count].PrevInFormation = enemies[0];
                }
                else
                {
                    //Add next element as previous in formation
                    enemies[count].PrevInFormation = enemies[count + 1];
                }

                if (count == Mathf.CeilToInt(enemies.Length / 2.0f))
                {
                    //If on the last element for this side, set next in formation to null
                    enemies[count].NextInFormation = null;
                }
                else
                {
                    //Add previous to next in formation
                    enemies[count].NextInFormation = new Enemy[1];
                    enemies[count].NextInFormation[0] = enemies[count - 1];
                }

                enemies[count].FormationSpacing = Vector3.Distance(enemies[count].transform.position, enemies[count].PrevInFormation.transform.position);
            }

            else
            {
                //Add previous to previous in formation
                enemies[count].PrevInFormation = enemies[count - 1];

                if (count + 1 >= enemies.Length / 2.0f)
                {
                    //If on the last element for this side, set next in formation to null
                    enemies[count].NextInFormation = null;
                }
                else
                {
                    //Add next to next in formation
                    enemies[count].NextInFormation = new Enemy[1];
                    enemies[count].NextInFormation[0] = enemies[count + 1];
                }

                enemies[count].FormationSpacing = Vector3.Distance(enemies[count].transform.position, enemies[count].PrevInFormation.transform.position);
            }
        }
    }
    #endregion
}