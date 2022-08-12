using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SavedUnit : SavedHealth
{
    public bool destination_bool;
    public SavedV3andQ destination;
    public SavedUnit(Unit unit) : base(unit)
    {
        NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
        destination_bool = agent.hasPath;
        if (destination_bool)
        {
            destination = new SavedV3andQ(agent.destination);
        }
    }
}
