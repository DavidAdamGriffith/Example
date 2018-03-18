/*
 
*Note: Spawning should be handled with object pools, this should be changed before production*

Description:        Defines a squad of objects with parameters in Editor:
                    - Number of units
                    - Formation, orientation of units (formations are pre-defined or custom)
                    
                    Handles spawning of units in-game, based on trigger (hit-box or otherwise)

David Griffith 2017
 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable, ExecuteInEditMode]
public class Squad : MonoBehaviour
{
    public enum SquadDistribution
    {
        AlternateLeader,
        Custom,
        Default
    }


    //Variables for spawning formations
    public SquadFormation.Formation formation = SquadFormation.Formation.Line;
    public SquadDistribution distribution = SquadDistribution.Default;

    public int numObjects = 6;          //Will be overridden by methods to meet their requirements

    public float spawnDistance = 1.0f;  //Distance to spawn from previous object, also used for setting the formation spacing

    public int numGeometryFaces = 3;    //Number of sides to be used with ShapeSpawn() and SpokeSpawn()
    public float wedgeAngle = 60.0f;    //Angle of separation used in WedgeSpawn()


    //References to prefabs to spawn
    public GameObject defaultPrefab;
    public GameObject alternateLeaderPrefab;
    public List<GameObject> customPrefabs = new List<GameObject>();


    //List of in-game units
    private List<GameObject> units;

    [SerializeField]
    private SquadTrigger squadTrigger;
    public SquadTrigger SquadTrigger
    {
        get
        {
            return squadTrigger;
        }

        set
        {
            squadTrigger = value;
        }
    }

    [SerializeField]
    private Vector3[] formationPositions;
    public Vector3[] FormationPositions
    {
        get
        {
            return formationPositions;
        }
        set
        {
            formationPositions = value;
        }
    }

    [SerializeField]
    private Vector3[] unitPositions;
    public Vector3[] UnitPositions
    {
        get
        {
            return unitPositions;
        }

        set
        {
            unitPositions = value;
        }
    }

    private bool customFormation = false;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUnitPositions();
    }

    public void SetPositions()
    {
        if (numObjects < 1)
        {
            numObjects = 1;
        }

        if (formation == SquadFormation.Formation.Custom)
        {
            SetCustomUnitPositions();
        }
        else
        {
            SetFormationUnitPositions();
        }
    }

    public void UpdateUnitPositions()
    {
        for (int i = 0; i < unitPositions.Length; i++)
        {
            unitPositions[i] = transform.rotation * formationPositions[i] + transform.position;
        }
    }


    private void SetFormationUnitPositions()
    {
        //Determine spawn locations and formation positioning for units
        formationPositions = SquadFormation.PickFormation(formation,
                                                        numObjects,
                                                        spawnDistance,
                                                        numGeometryFaces,
                                                        wedgeAngle);

        unitPositions = new Vector3[formationPositions.Length];

        UpdateUnitPositions();
    }

    private void SetCustomUnitPositions()
    {
        //If there are no unit positions set
        if (unitPositions == null)
        {
            //Create a new set of positions
            unitPositions = new Vector3[numObjects];

            //Set the position to make a line
            for (int i = 0; i < unitPositions.Length; i++)
            {
                unitPositions[i] = transform.position + Vector3.right * i;
            }
        }
        //Else if the length doesn't match the number of objects
        else if (unitPositions.Length != numObjects)
        {
            //Create a new set of positions with proper number of objects
            Vector3[] updatedPositions = new Vector3[numObjects];

            //If there are less than the proper number of objects
            if (unitPositions.Length < numObjects)
            {
                //Copy what positions we already have stored in the new array
                for (int i = 0; i < unitPositions.Length; i++)
                {
                    updatedPositions[i] = unitPositions[i];
                }

                //Add any remaining positions just to the right of the previous
                for (int i = unitPositions.Length; i < updatedPositions.Length; i++)
                {
                    updatedPositions[i] = updatedPositions[i - 1] + Vector3.up;
                }
            }
            //Else if there are more positions stored than the proper number of objects
            else if (unitPositions.Length > numObjects)
            {
                //Copy from the original array up until the proper number of objects, ignoring the rest
                for (int i = 0; i < updatedPositions.Length; i++)
                {
                    updatedPositions[i] = unitPositions[i];
                }
            }

            //Update unit position
            unitPositions = updatedPositions;
        }
        else
        {
            UpdateUnitPositions();
        }

        //Create a new set of formation positions
        formationPositions = new Vector3[unitPositions.Length];

        for (int i = 0; i < FormationPositions.Length; i++)
        {
            formationPositions[i] = unitPositions[i] - transform.position;
        }
    }

    public void SetCustomUnitPosition(int unitIndex, Vector3 position)
    {
        unitPositions[unitIndex] = position;
        formationPositions[unitIndex] = position - transform.position;
    }




    //Spawn the units in this squad
    public void SpawnUnits()
    {
        switch (distribution)
        {
            //Spawn units with an alternate leader prefab
            case SquadDistribution.AlternateLeader:
                
                //Only spawn if default and alternate leader prefabs are set
                if (defaultPrefab != null && alternateLeaderPrefab != null)
                {
                    Enemy[] enemies = SpawnAlternateLeader(formation,
                                                           unitPositions,
                                                           numGeometryFaces,
                                                           transform.position,
                                                           transform.rotation,
                                                           defaultPrefab,
                                                           alternateLeaderPrefab);

                    units = new List<GameObject>();

                    for (int i = 0; i < enemies.GetLength(0); i++)
                    {
                        units.Add(enemies[i].gameObject);
                        units[i].gameObject.transform.parent = gameObject.transform;
                        units[i].gameObject.name = "Unit " + i + " - " + gameObject.name;
                    }
                }
                //Otherwise throw an error
                else
                {
                    Debug.Log(gameObject.name + " is missing a required Default or Alternate Leader Prefab!");
                }

                break;
            
            //Spawn units based on a custom pre-defined list
            case SquadDistribution.Custom:

                //Only spawn if default prefab is set
                //Custom array can be incomplete or empty, default prefab will be spawned for any additional needed
                if (defaultPrefab != null)
                {
                    Enemy[] enemies = SpawnFromArray(formation,
                                                     unitPositions,
                                                     numGeometryFaces,
                                                     transform.position,
                                                     transform.rotation,
                                                     defaultPrefab,
                                                     customPrefabs);

                    units = new List<GameObject>();

                    for (int i = 0; i < enemies.GetLength(0); i++)
                    {
                        units.Add(enemies[i].gameObject);
                        units[i].gameObject.transform.parent = gameObject.transform;
                        units[i].gameObject.name = "Unit " + i + " - " + gameObject.name;
                    }
                }
                //Otherwise throw an error
                else
                {
                    Debug.Log(gameObject.name + " is missing a required Default Prefab!");
                }

                break;

            //Spawn units based on default prefab
            default:

                //Only spawn if default prefab is set
                if (defaultPrefab != null)
                {
                    Enemy[] enemies = SpawnDefault(formation,
                                                   unitPositions,
                                                   numGeometryFaces,
                                                   transform.position,
                                                   transform.rotation,
                                                   defaultPrefab);

                    units = new List<GameObject>();

                    for (int i = 0; i < enemies.GetLength(0); i++)
                    {
                        units.Add(enemies[i].gameObject);
                        units[i].gameObject.transform.parent = gameObject.transform;
                        units[i].gameObject.name = "Unit " + i + " - " + gameObject.name;
                    }
                }
                //Otherwise throw an error
                else
                {
                    Debug.Log(gameObject.name + " is missing a required Default Prefab!");
                }

                break;
        }
    }


    #region Spawning Methods
    //Spawns enemies in formation
    private Enemy[] SpawnDefault(SquadFormation.Formation formation,
                                 Vector3[] locations,
                                 int numGeometryFaces,
                                 Vector3 originOffset,
                                 Quaternion originRotation,
                                 GameObject defaultPrefab)
    {
        if (locations != null && defaultPrefab != null)
        {
            //Create array to hold enemies
            Enemy[] enemies = new Enemy[locations.Length];

            //Instantiate each enemy, and set it's Formation Position and Spacing
            for (int count = 0; count < locations.Length; count++)
            {
                //Spawn unit while getting reference - FIX ROTATION!
                GameObject enemyObject = (GameObject)Instantiate(defaultPrefab, locations[count], transform.rotation); //

                enemies[count] = enemyObject.GetComponent<Enemy>();
                enemies[count].FormationPosition = locations[count]; //
            }

            //Link together references to other objects in the formation
            SquadFormation.LinkFormation(enemies, formation, numGeometryFaces);

            //Final Step - Safe to initialize the AI
            for (int count = 0; count < enemies.Length; count++)
            {
                int formationOrder = 0;

                if (count == 0)
                {
                    //Set the first enemy in the list to be the leader
                    enemies[count].SetAsLeader();
                }
                else
                {
                    Enemy leader = enemies[count];

                    while (!leader.IsLeader)
                    {
                        leader = leader.PrevInFormation;

                        formationOrder++;
                    }
                }

                enemies[count].FormationOrder = formationOrder;

                enemies[count].AI.InitAI();
            }

            return enemies;
        }

        return null;
    }



    //Spawns enemies in formation with an alternate leader
    private Enemy[] SpawnAlternateLeader(SquadFormation.Formation formation,
                                         Vector3[] locations,
                                         int numGeometryFaces,
                                         Vector3 originOffset,
                                         Quaternion originRotation,
                                         GameObject defaultPrefab,
                                         GameObject leaderPrefab)
    {
        if (locations != null && defaultPrefab != null && leaderPrefab != null)
        {
            //Create array to hold enemies
            Enemy[] enemies = new Enemy[locations.Length];

            //Set the first prefab to be the leader's prefab
            GameObject prefab = leaderPrefab;

            //Instantiate each enemy, and set it's Formation Position and Spacing
            for (int count = 0; count < locations.Length; count++)
            {
                //Spawn unit while getting reference - FIX ROTATION!
                GameObject enemyObject = (GameObject)Instantiate(prefab, locations[count], transform.rotation); //

                enemies[count] = enemyObject.GetComponent<Enemy>();
                enemies[count].FormationPosition = locations[count]; //

                //Set every subsequent prefab to the default prefab
                prefab = defaultPrefab;
            }

            //Link together references to other objects in the formation
            SquadFormation.LinkFormation(enemies, formation, numGeometryFaces);

            //Final Step - Safe to initialize the AI
            for (int count = 0; count < enemies.Length; count++)
            {
                int formationOrder = 0;

                if (count == 0)
                {
                    //Set the first enemy in the list to be the leader
                    enemies[count].SetAsLeader();
                }
                else
                {
                    Enemy leader = enemies[count];

                    while (!leader.IsLeader)
                    {
                        leader = leader.PrevInFormation;

                        formationOrder++;
                    }
                }

                enemies[count].FormationOrder = formationOrder;

                enemies[count].AI.InitAI();
            }

            return enemies;
        }

        return null;
    }



    //Spawns enemies in formation, in order from an array of prefabs
    //Uses default prefab if the passed in array is less than the amount of spawns
    //Ignores remaining in list of prefabs if greater than the amount of spawns
    private Enemy[] SpawnFromArray(SquadFormation.Formation formation,
                                   Vector3[] locations,
                                   int numGeometryFaces,
                                   Vector3 originOffset,
                                   Quaternion originRotation,
                                   GameObject defaultPrefab,
                                   List<GameObject> customPrefabs)
    {
        if (locations != null && defaultPrefab != null && customPrefabs != null)
        {
            //Create array to hold enemies
            Enemy[] enemies = new Enemy[locations.Length];

            //Instantiate each enemy, and set it's Formation Position and Spacing
            for (int count = 0; count < locations.Length; count++)
            {
                GameObject enemyObject;

                //Spawn unit while getting reference - FIX ROTATION!
                if (count < customPrefabs.Count)
                {
                    //Prioritize spawning prefabs from array in the order they are in the array
                    enemyObject = (GameObject)Instantiate(customPrefabs[count], locations[count], transform.rotation);//
                }
                else
                {
                    //If the formation is larger than the provided array, use the default prefab instead
                    enemyObject = (GameObject)Instantiate(defaultPrefab, locations[count], transform.rotation);//
                }

                enemies[count] = enemyObject.GetComponent<Enemy>();
                enemies[count].FormationPosition = locations[count];//
            }

            //Link together references to other objects in the formation
            SquadFormation.LinkFormation(enemies, formation, numGeometryFaces);

            //Final Step - Safe to initialize the AI
            for (int count = 0; count < enemies.Length; count++)
            {
                int formationOrder = 0;

                if (count == 0)
                {
                    //Set the first enemy in the list to be the leader
                    enemies[count].SetAsLeader();
                }
                else
                {
                    Enemy leader = enemies[count];

                    while (!leader.IsLeader)
                    {
                        leader = leader.PrevInFormation;

                        formationOrder++;
                    }
                }

                enemies[count].FormationOrder = formationOrder;

                enemies[count].AI.InitAI();
            }

            return enemies;
        }

        return null;
    }
    #endregion
}