using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tank : Unit
{
    private float radius = 15f;
    private RotatingTower tower;
    //private Transform tower, canon;
    //private Transform[] children;
    //    private NavMeshAgent agent;
    //public GameObject missile;
    //private List<GameObject> missilePool;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        tower = GetComponent<RotatingTower>();
        tower.team = team;
        DamageIvent += Defend;
        /*children = gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name == "TankFree_Canon")
            {
                canon = children[i];
            }
        }
        missilePool = new List<GameObject>();
        missilePool.Add(Instantiate(missile, canon));
        missilePool[0].SetActive(false);*/
        //agent = GetComponent<NavMeshAgent>();
    }

    private new void Update()
    {
        base.Update();
        if (target != null)
        {
            /*поворачиваем башню к цели
            Vector3 targetDirection = target.transform.position - transform.position;
            targetDirection.y = 0;
            float singleStep = 3 * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(tower.forward, targetDirection, singleStep, 0.0f);
            tower.rotation = Quaternion.LookRotation(newDirection);*/
            float distance = Vector3.Distance(target.transform.position, transform.position);
            //Может быть затратно. Переписать через триггеры?
            if (distance < radius)
            {
                agent.isStopped = true;
            }
            else if (distance > radius)
            {
                MoveTo(target.transform.position);
            }
            if (!target.isActiveAndEnabled)
            {
                target = null;
                tower.Off();
            }
        }
    }
    public override void Action(Health target)
    {
        if (target.team == team)
        {
            Debug.Log("Это свой");
        }
        else
        {
            Debug.Log("Это враг");
            Attack(target);
        }
    }
    public override void MoveTo(Vector3 position, bool interrupt = false)
    {
        base.MoveTo(position);
        if (interrupt)
        {
            target = null;
            tower.Off();
        }
    }
    public void Attack(Health target_)
    {
        target = target_;
        float distance = (target.transform.position - transform.position).magnitude;
        //Debug.Log(distance);
        if (distance > radius)
        {
            MoveTo(target.transform.position);
        }
        tower.Attack(target);
        //InvokeRepeating("Shot", 1f, 2f);
    }
    public void Defend(Health aggressor)
    {
        if (!IsWorking())
        {
            //Debug.Log(name + " решил сражаться");
            Attack(aggressor);
        }
        else
        {
            //Debug.Log(name + " решил не отвлекаться на драку");
            if (target != null)
            {
                Debug.Log(name + " сражается с " + target.name);
            }
            if (agent.hasPath)
            {
                float distance = Vector3.Distance(agent.destination, transform.position);
                Debug.Log(name + " " + distance);
            }
        }
    }
    public void Bye(Tank friend)
    {
        companion.Remove(friend);
        Debug.Log(name + " " + friend.name);
    }
    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        var another = other.GetComponent<Health>();
        if (another != null && another.team != team && target == null)
        {
            //Debug.Log("Враг близко!");
        }
    }
    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        var another = other.GetComponent<Health>();
        Debug.Log(other.name);
    }
    private void Shot()
    {/*
        RaycastHit hit;
        if (Physics.Raycast(tower.position, tower.forward, out hit, radius))
        {
            //the collider could be children of the unit, so we make sure to check in the parent
            var target_ = hit.collider.GetComponentInParent<Health>();
            if (target_ == target)
            {
                Debug.Log("Выстрел");
                Vector3 missilePos = canon.position + canon.forward;
                for (int i = 0; i < missilePool.Count; i++)
                {
                    if (!missilePool[i].activeInHierarchy)
                    {
                        missilePool[i].transform.position = missilePos;
                        missilePool[i].transform.rotation = canon.rotation;
                        missilePool[i].SetActive(true);
                        return;
                    }
                }
                missilePool.Add(Instantiate(missile, missilePos, canon.rotation, canon));
            }
        }
    */}
}
