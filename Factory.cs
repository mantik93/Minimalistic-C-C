using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Health
{
    public Transform gate, point;
    public List<GameObject> unit_prefab;
    public FactoryPanel panel;
    private bool gate_move, closed = true;
    private int gate_direction;
    private Unit unit;
    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        panel = FindObjectOfType<FactoryPanel>(true);
    }
    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (gate_move)
        {
            gate.Translate(gate_direction * Vector3.up * 0.5f * Time.deltaTime, Space.World);
            gate.localScale -= new Vector3(gate_direction * Time.deltaTime, 0, 0);
            if (closed && gate.localPosition.y > 2.9)
            {
                gate_move = false;
                closed = false;
            }
            else if (!closed && gate.localPosition.y < 1.5)
            {
                gate_move = false;
                closed = true;
                gate.localPosition = new Vector3(0f, 1.5f, 1.95f);
                gate.localScale = new Vector3(2.9f, 0.1f, 3.8f);
            }
        }
    }
    public void OpenOrCloseGate()
    {
        if (!gate_move)
        {
            if (closed)
            {
                gate_direction = 1;
            }
            else
            {
                gate_direction = -1;
            }
            gate_move = true;
        }
    }
    public void CreateUnit(int n)
    {
        var product = Instantiate(unit_prefab[n], transform.position + Vector3.up, transform.rotation);
        unit = product.GetComponent<Unit>();
        OpenOrCloseGate();
        Invoke(nameof(OutputUnit), 2);
    }
    public void OutputUnit()
    {
        unit.MoveTo(point.position);
        Invoke(nameof(OpenOrCloseGate), 2);
    }
    public override void Action(Vector3 target)
    {
        point.position = target;
    }
    public override void Selected(bool select)
    {
        base.Selected(select);
        point.gameObject.SetActive(select);
        panel.gameObject.SetActive(select);
        panel.factory = this;
    }
}
