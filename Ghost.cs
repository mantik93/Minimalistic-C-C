using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Renderer[] exterior;
    private Color white, red;
    private Vector3 screenPoint, newpos;
    public float posY;
    public Crane crane;
    public int Conflicts { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        exterior = GetComponentsInChildren<Renderer>(true);
        white = new Color(1, 1, 1, 0.2f);
        red = new Color(1, 0, 0, 0.2f);
        //enabled = false; //тут мы отключаем Update
    }

    // Update is called once per frame
    void Update()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        newpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        newpos.y = posY;
        transform.position = newpos;
        if (Input.GetKeyDown("space"))
        {
            transform.Rotate(0, 90, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Conflicts++;
        foreach (var item in exterior)
        {
            item.material.color = red;
        }
        Debug.Log(other.name);
    }
    private void OnTriggerExit(Collider other)
    {
        Conflicts--;
        if(Conflicts == 0) 
            foreach (var item in exterior)
            {
                item.material.color = white;
            }
    }
}
