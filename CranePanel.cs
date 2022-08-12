using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CranePanel : MonoBehaviour
{
    public Crane crane;
    public void BuildTurret()
    {
        crane.BuildPreview(0);
    }
}
