using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : Unit
{
    public CranePanel panel;
    public List<GameObject> building_prefab, samples;
    public GameObject scaffold_prefab;
    private List<Vector3> scaffold_size = new List<Vector3>();
    private bool preview;
    private int plan;
    private Ghost ghost;
    private GameObject building, scaffold;
    private Material mat;
    private new void Start()
    {
        base.Start();
        panel = FindObjectOfType<CranePanel>(true);
        camera_control.ChangeSelectIvent += PanelActive;
        scaffold_size.Add(new Vector3(2.4f, 2, 2)); //turret
        scaffold_size.Add(new Vector3(6, 7, 6)); //factory
    }
    public override void Selected(bool select)
    {
        base.Selected(select);
        if (panel != null)
        {
            panel.gameObject.SetActive(select);
            panel.crane = this;
        }
    }
    public override void MoveTo(Vector3 position, bool interrupt = false)
    {
        if (!preview) base.MoveTo(position);
    }
    private void PanelActive(int count)
    {
        if (count != 1) panel.gameObject.SetActive(false);
    }
    public void BuildPreview(int n)
    {
        samples[n].transform.position = transform.position;
        samples[n].SetActive(true);
        ghost = samples[n].GetComponent<Ghost>();
        preview = true;
        plan = n;
    }
    public override void Action(Vector3 target)
    {
        if (preview && ghost.Conflicts == 0)
        {
            var sc_pos = samples[plan].transform.position;
            sc_pos.y = scaffold_size[plan].y / 2;
            building = Instantiate(building_prefab[plan], samples[plan].transform.position, samples[plan].transform.rotation);
            scaffold = Instantiate(scaffold_prefab, sc_pos, samples[plan].transform.rotation);
            scaffold.transform.localScale = scaffold_size[plan];
            mat = scaffold.GetComponent<Renderer>().material;
            preview = false;
            MoveTo(target);
            samples[plan].SetActive(false);
        }
    }
    private void Build()
    {
        var c = mat.color;
        c.a -= 0.16f;
        mat.color = c;
        if (mat.color.a < 0.1f)
        {
            CancelInvoke();
            Destroy(scaffold);
            MoveTo(transform.position - transform.forward);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (target != null && target.gameObject == collision.gameObject)
        {
            agent.isStopped = true;
        }
        if (scaffold != null && scaffold == collision.gameObject)
        {
            agent.isStopped = true;
            InvokeRepeating(nameof(Build), 0.5f, 2.5f);
        }
    }
}
