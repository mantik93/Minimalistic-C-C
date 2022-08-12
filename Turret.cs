using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Health
{
    private RotatingTower tower;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        tower = GetComponent<RotatingTower>();
        tower.team = team;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        //if (target == null || !target.isActiveAndEnabled)
            //tower.CancelInvoke();
    }
}
