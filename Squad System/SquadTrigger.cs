/*

Description:        Attach to a collider or other trigger to have one or more squads react
                    Currently handles spawning of units

David Griffith 2017
 
 */

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SquadTrigger : MonoBehaviour
{
    [SerializeField]
    private SquadManager squadManager;
    public SquadManager SquadManager
    {
        get
        {
            return squadManager;
        }

        set
        {
            squadManager = value;
        }
    }

    //Reference to squads managed  by this squad trigger
    [SerializeField]
    private List<Squad> squads;
    public List<Squad> Squads
    {
        get
        {
            return squads;
        }

        set
        {
            squads = value;
        }
    }


    void OnTriggerEnter(Collider col)
    {
        //Pass object of collision to Trigger Spawn
        TriggerSpawn();
    }

    //Trigger squads' spawn
    private void TriggerSpawn()
    {
        foreach (Squad squad in squads)
        {
            squad.SpawnUnits();
        }
    }
}
