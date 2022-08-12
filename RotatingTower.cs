using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingTower : MonoBehaviour
{
    public string team;
    private Health target, basis;
    public List<Health> enemies = new List<Health>();
    private float radius = 15f;
    private Transform tower, canon;
    private Transform[] children;
    public GameObject missile;
    private List<GameObject> missilePool;
    private Vector3 forward;

    // Start is called before the first frame update
    void Start()
    {
        children = gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name.EndsWith("Canon"))
                canon = children[i];
            if (children[i].name.EndsWith("Tower"))
                tower = children[i];
        }
        basis = gameObject.GetComponent<Health>();
        missilePool = new List<GameObject>
        { Instantiate(missile, canon) };
        missilePool[0].SetActive(false);
        forward = tower.forward;
    }

    // Update is called once per frame
    void Update()
    {
        //поворачиваем башню к цели
        if (target != null && target.isActiveAndEnabled)
        {
            Vector3 targetDirection = target.transform.position - tower.position;
            targetDirection.y = 0;
            float singleStep = 3 * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(tower.forward, targetDirection, singleStep, 0.0f);
            tower.rotation = Quaternion.LookRotation(newDirection);
        }
        else if (enemies.Count != 0)
        {
            if (!enemies[0].isActiveAndEnabled)
                enemies.Remove(enemies[0]);
            else
                target = enemies[0];
        }
        else if (target == null || !target.isActiveAndEnabled)
        {
            float singleStep = 3 * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(tower.forward, forward, singleStep, 0.0f);
            tower.rotation = Quaternion.LookRotation(newDirection);
        }
    }
    public void Attack(Health target_)
    {
        target = target_;
        Debug.Log(name + " " + target.name);
        if (!basis.IsWorking())     //иначе будет бесконечный цикл!
            basis.Action(target);
        InvokeRepeating(nameof(Shot), 1f, 2f);
    }
    public void Off()
    {
        enemies.Remove(target);
        target = null;
        CancelInvoke();
    }
    private void Shot()
    {
        RaycastHit hit;
        if (Physics.Raycast(tower.position, tower.forward, out hit, radius))
        {
            //the collider could be children of the unit, so we make sure to check in the parent
            var target_ = hit.collider.GetComponentInParent<Health>();
            if (target_ == target)
            {
                //Debug.Log(name + " атакует " + target.name);
                Vector3 missilePos = canon.position + tower.forward;
                for (int i = 0; i < missilePool.Count; i++)
                {
                    if (!missilePool[i].activeInHierarchy)
                    {
                        missilePool[i].transform.position = missilePos;
                        missilePool[i].transform.rotation = tower.rotation;
                        missilePool[i].SetActive(true);
                        return;
                    }
                }
                missilePool.Add(Instantiate(missile, missilePos, canon.rotation, canon));
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        var another = other.GetComponent<Health>();
        if (another != null && another.team != team)
        {
            enemies.Add(another);
            if (target == null && !basis.IsWorking())
                Attack(another);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var another = other.GetComponent<Health>();
        if (another == target)
            target = null;
        if (another != null && another.team != team)
            enemies.Remove(another);
        if(enemies.Count == 0)
            CancelInvoke();
    }
    void OnDisable()
    {
        CancelInvoke();
    }
}
