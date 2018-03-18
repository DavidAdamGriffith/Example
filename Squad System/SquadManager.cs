/*

*Note: This could be improved with recent Unity updates, using the EditableList (should be safe to use now)*

Description:        Manages and maintains reference to a list of squads, and their triggers (boundaries to act/spawn, etc.):
                    - Add/Remove Squads, Triggers
                    - Handles placement and removal before/after and beginning/end
                    
                    Enforces placement/boundary rules of squads' units

David Griffith 2017
 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[System.Serializable, ExecuteInEditMode]
public class SquadManager : MonoBehaviour
{
    //Option to align squad triggers on Z-axis
    public bool alignSquadTriggers = false;

    //Container for squads
    public GameObject spawnAreaObject;

    //Create list of squad's gameObjects
    [SerializeField]
    private List<SquadTrigger> squadTriggers = new List<SquadTrigger>();
    public List<SquadTrigger> SquadTriggers
    {
        get
        {
            return squadTriggers;
        }
    }

    [SerializeField]
    private GameObject unitCursor;
    public GameObject UnitCursor
    {
        get
        {
            return unitCursor;
        }
    }

    void Update()
    {
        SetEditorRules();
    }

    #region Add/Remove Squad Triggers
    public void AddSquadTriggerAtEnd()
    {
        SquadTrigger squadTrigger = CreateSquadTrigger();

        squadTriggers.Add(squadTrigger);

        UpdateNames();
    }

    //Remove last squad in the list
    public void RemoveSquadTriggerAtEnd()
    {
        if (squadTriggers.Count > 0)
        {
            RemoveSquadTrigger(squadTriggers[squadTriggers.Count - 1]);
        }
    }

    //Remove specific squad from list
    public void RemoveSquadTrigger(SquadTrigger squadTrigger)
    {
        //Remove squad object from list
        squadTriggers.Remove(squadTrigger);

        //Destroy the squad object and associated trigger
        DestroyImmediate(squadTrigger.gameObject);

        //Update names to ensure list makes sense
        UpdateNames();
    }
    #endregion

    #region Add/Remove Squads
    //Add a new squad to the manager at end
    public void AddSquadAtEnd(SquadTrigger squadTrigger)
    {
        Squad squad = CreateSquad(squadTrigger);

        squadTrigger.Squads.Add(squad);

        //Update names to ensure list makes sense
        UpdateNames();
    }

    //Add a new squad to the manager before another squad in the list
    public Squad AddSquadBefore(Squad referenceSquad)
    {
        SquadTrigger squadTrigger = referenceSquad.SquadTrigger;

        //Iterate through array of squad objects
        for (int i = 0; i < squadTrigger.Squads.Count; i++)
        {
            //If iterated object is the same as the reference object passed in
            if (squadTrigger.Squads[i].GetInstanceID() == referenceSquad.GetInstanceID())
            {
                //Create new child object, attach Squad script and set it's Squad Manager to this
                Squad squad = CreateSquad(squadTrigger);

                //Insert new object at position of reference object
                squadTrigger.Squads.Insert(i, squad);

                //Update names to ensure list makes sense
                UpdateNames();

                return squad;
            }
        }

        return null;
    }

    //Add a new squad to the manager after another squad in the list
    public Squad AddSquadAfter(Squad referenceSquad)
    {
        SquadTrigger squadTrigger = referenceSquad.SquadTrigger;

        //Iterate through array of squad objects
        for (int i = 0; i < squadTrigger.Squads.Count; i++)
        {
            //If iterated object is the same as the reference object passed in
            if (squadTrigger.Squads[i].GetInstanceID() == referenceSquad.GetInstanceID())
            {
                //Create new child object, attach Squad script and set it's Squad Manager to this
                Squad squad = CreateSquad(squadTrigger);

                //Insert new object at position after reference object
                squadTrigger.Squads.Insert(i + 1, squad);

                //Update names to ensure list makes sense
                UpdateNames();

                return squad;
            }
        }

        return null;
    }

    //Remove last squad in the list
    public void RemoveSquadAtEnd(SquadTrigger squadTrigger)
    {
        if (squadTrigger.Squads.Count > 0)
        {
            RemoveSquad(squadTrigger.Squads[squadTrigger.Squads.Count - 1]);
        }
    }

    //Remove specific squad from list
    public void RemoveSquad(Squad squad)
    {
        //Remove squad object from list
        squad.SquadTrigger.Squads.Remove(squad);

        //Destroy the squad object and associated trigger
        DestroyImmediate(squad.gameObject);

        //Update names to ensure list makes sense
        UpdateNames();
    }
    #endregion

    #region Object Creation
    public SquadTrigger CreateSquadTrigger()
    {
        GameObject squadTriggerObject = new GameObject();
        squadTriggerObject.transform.parent = transform;
        squadTriggerObject.AddComponent<BoxCollider>();
        squadTriggerObject.AddComponent<SquadTrigger>();

        SquadTrigger squadTrigger = squadTriggerObject.GetComponent<SquadTrigger>();

        squadTrigger.SquadManager = this;
        squadTrigger.Squads = new List<Squad>();

        float zScale = spawnAreaObject.transform.localScale.z;
        float zOffset = 0.0f;

        foreach (SquadTrigger refSquadTrigger in SquadTriggers)
        {
            if (refSquadTrigger.transform.position.z > zOffset)
            {
                zOffset = refSquadTrigger.transform.position.z;
            }
        }

        squadTriggerObject.transform.position = new Vector3(transform.position.x, transform.position.y, zOffset + zScale);       

        squadTriggerObject.transform.localScale = spawnAreaObject.transform.localScale;

        return squadTrigger;
    }

    public Squad CreateSquad(SquadTrigger squadTrigger)
    {
        //Create new squad GameObject, add appropraite scripts and position
        GameObject squadObject = new GameObject();
        squadObject.AddComponent<Squad>();

        Squad squad = squadObject.GetComponent<Squad>();

        squadObject.transform.parent = squadTrigger.transform;
        squadObject.transform.position = new Vector3(squadTrigger.transform.position.x, squadTrigger.transform.position.y, squadTrigger.transform.position.z + squadTrigger.Squads.Count + 1.0f);
        //Set the squad's trigger
        squad.SquadTrigger = squadTrigger;

        squad.SetPositions();

        return squad;
    }
    #endregion

    #region Methods
    //Update the names of squads
    private void UpdateNames()
    {
        //Iterate through list of squad objects assigning name based on position
        for (int i = 0; i < squadTriggers.Count; i++)
        {
            squadTriggers[i].name = "Squad Trigger " + i;
        }

        int count = 0;

        //Iterate through list of squad objects assigning name based on position
        foreach (SquadTrigger squadTrigger in squadTriggers)
        {
            for (int i = 0; i < squadTrigger.Squads.Count; i++)
            {
                squadTrigger.Squads[i].name = "Squad " + count;
                count++;
            }
        }
    }

    private void CreateUnitCursor()
    {
        if (unitCursor == null)
        {
            unitCursor = new GameObject("Cursor: [Null]");
            unitCursor.AddComponent<UnitCursor>();

            unitCursor.transform.parent = transform;
        }
    }

    //Calls all design rules to be enforced
    private void SetEditorRules()
    {
        SetSquadManagerScale();
        SetSquadTriggerZPosition();
        SetSquadTriggerAlignOnAxis();
        SetSize();
        SetSquadSpawnBoundaries();

        CreateUnitCursor();
    }

    private void SetSquadManagerScale()
    {
        Vector3 defaultScale = new Vector3(1.0f, 1.0f, 1.0f);

        if (transform.localScale != defaultScale)
        {
            transform.localScale = defaultScale;
        }
    }

    private void SetSquadTriggerZPosition()
    {
        if (spawnAreaObject)
        {
            foreach (SquadTrigger squadTrigger in squadTriggers)
            {
                float zPositionMin = transform.position.z + (spawnAreaObject.transform.localScale.z / 2);

                if (squadTrigger.transform.position.z < zPositionMin)
                {
                    squadTrigger.transform.position = new Vector3(squadTrigger.transform.position.x, squadTrigger.transform.position.y, zPositionMin);
                }
            }
        }
    }

    //Align's trigger's position to parent
    private void SetSquadTriggerAlignOnAxis()
    {
        if (alignSquadTriggers)
        {
            foreach (SquadTrigger squadTrigger in squadTriggers)
            {
                //Align Squad Trigger on X and Y
                squadTrigger.transform.position = new Vector3(transform.position.x, transform.position.y, squadTrigger.transform.position.z);
            }
        }
    }

    //Enforces position boundaries for squads, so squads are located within the box collider's bounds
    private void SetSquadSpawnBoundaries()
    {
        foreach(SquadTrigger squadTrigger in squadTriggers)
        {
            BoxCollider boxCollider = squadTrigger.GetComponent<BoxCollider>();

            foreach (Squad squad in squadTrigger.Squads)
            {
                if (squad.transform.position.x < boxCollider.bounds.min.x)
                {
                    squad.transform.position = new Vector3(boxCollider.bounds.min.x, squad.transform.position.y, squad.transform.position.z);
                }
                else if (squad.transform.position.x > boxCollider.bounds.max.x)
                {
                    squad.transform.position = new Vector3(boxCollider.bounds.max.x, squad.transform.position.y, squad.transform.position.z);
                }

                if (squad.transform.position.y < boxCollider.bounds.min.y)
                {
                    squad.transform.position = new Vector3(squad.transform.position.x, boxCollider.bounds.min.y, squad.transform.position.z);
                }
                else if (squad.transform.position.y > boxCollider.bounds.max.y)
                {
                    squad.transform.position = new Vector3(squad.transform.position.x, boxCollider.bounds.max.y, squad.transform.position.z);
                }

                if (squad.transform.position.z < boxCollider.bounds.min.z)
                {
                    squad.transform.position = new Vector3(squad.transform.position.x, squad.transform.position.y, boxCollider.bounds.min.z);
                }
                else if (squad.transform.position.z > boxCollider.bounds.max.z)
                {
                    squad.transform.position = new Vector3(squad.transform.position.x, squad.transform.position.y, boxCollider.bounds.max.z);
                }
            }
        }
    }

    //Enforces the size of the squad trigger is equal to that of the spawn area
    private void SetSize()
    {
        if (spawnAreaObject)
        {
            foreach (SquadTrigger squadTrigger in squadTriggers)
            {
                if (squadTrigger.transform.localScale != spawnAreaObject.transform.localScale)
                {
                    squadTrigger.transform.localScale = spawnAreaObject.transform.localScale;
                }
            }
        }
    }
    #endregion
}