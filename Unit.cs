using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Health
{
    // Описывает поведение движущихся объектов, таких как танки и строители.
    public float Speed = 3;
    protected NavMeshAgent agent;
    public List<Tank> companion = new List<Tank>();
    protected new void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
        agent.acceleration = 999;
        agent.angularSpeed = 999;
        DamageIvent += Help;
    }

    // Update is called once per frame
    public virtual void MoveTo(Vector3 position, bool interrupt = false)
    {
        agent.SetDestination(position);
        agent.isStopped = false;
    }
    public override bool IsWorking() 
    {
        float velocity = agent.velocity.magnitude;
        return target != null || velocity > 0.5f;
    }
    protected void OnTriggerEnter(Collider other)
    {
        var another = other.GetComponent<Tank>();
        if (another != null && another.team == team)
        {
            companion.Add(another);
            //Debug.Log(name + " " + other.name);
        }
    }
    protected void OnTriggerExit(Collider other)
    {        
        var another = other.GetComponent<Tank>();
        if (another != null && another.team == team)
        {
            companion.Remove(another);
            //Debug.Log(name + " " + other.name);
        }
    }
    protected void Help(Health aggressor)
    {
        foreach (var item in companion)
        {
            Debug.Log(name + " попросил помощи у " + item.name);
            item.Defend(aggressor);
        }
    }
    private void OnDisable()
    {
        camera_control?.Bye(this);
        Selected(false);
        if (this is Tank tank)
        {
            foreach (var item in companion)
            {
                item.Bye(tank);
            }
        }
    }
}
