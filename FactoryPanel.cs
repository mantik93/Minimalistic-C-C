using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryPanel : MonoBehaviour
{
    public Factory factory;
    public void CreateTank()
    {
        factory.CreateUnit(0);
    }
    public void CreateCrane()
    {
        factory.CreateUnit(1);
    }
}
