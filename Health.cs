using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void Damage_del(Health aggressor);

public class Health : MonoBehaviour
{
    public int Max_hp { get; private set; }
    public int Num_hp { get; private set; }
    private Transform healthbar;
    private Transform[] children;
    private Vector3 originalScale, hp;
    private Material material;
    protected Health target;
    public Health Target { get { return target; } }
    public CameraControl camera_control;
    public bool IsSelected { get; private set; }
    public GameObject marker;
    public string team;
    private readonly string[] teamArray = new string[4] { "Blue", "Green", "Red", "Yellow" };
    protected void Awake()
    {
        Max_hp = 5;
        Num_hp = Max_hp;
    }
    protected void Start()
    {
        camera_control = Camera.main.GetComponent<CameraControl>();
        children = gameObject.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name == "Healthbar")
                healthbar = children[i];
            if (children[i].name == "Marker")
                marker = children[i].gameObject;
        }
        material = healthbar.GetComponent<MeshRenderer>().material;
        originalScale = healthbar.localScale;
        hp = new Vector3(originalScale.x / Max_hp, 0f, 0f);
        //if (!CompareTag(team)) Debug.Log(name);
        //if (!name.EndsWith(team)) Debug.Log(name);
    }
    protected event Damage_del DamageIvent;
    public void Damage(Health aggressor)
    {
        //Debug.Log(healthbar.localScale.x);
        healthbar.localScale -= hp;
        Num_hp -= 1;
        if (healthbar.localScale.x <= hp.x * 3)
        {
            material.color = Color.red;
        }
        if (healthbar.localScale.x < hp.x)
        {
            Debug.Log(name + " погиб");
            gameObject.SetActive(false);
        }
        //Debug.Log(aggressor.name + " напал на " + name);
        DamageIvent?.Invoke(aggressor);
        //if (this is Tank tank && !tank.IsWorking())
        //{
            //tank.Action(aggressor);
        //}
    }
    public void Healing()
    {
        Num_hp += 1;
        if (healthbar.localScale.x < originalScale.x)
        {
            healthbar.localScale += hp;
            if (healthbar.localScale.x > originalScale.x)
                healthbar.localScale = originalScale;
        }
        if (healthbar.localScale.x >= hp.x * 2)
        {
            material.color = Color.green;
        }
    }
    public void ReturnToOriginal()
    {
        healthbar.localScale = originalScale;
        material.color = Color.green;
    }
    public virtual void Selected(bool select)
    {
        marker.SetActive(select);
        IsSelected = select;
    }
    public virtual void Action(Health target)
    {
        if (target.team == team)
        {
            Debug.Log("Это свой");
        }
        else
        {
            Debug.Log("Это враг");
        }
    }
    public virtual void Action(Vector3 target)
    {
        Debug.Log(target);
    }
    public virtual bool IsWorking()
    {
        return target != null;
    }
    protected void Update()
    {
        healthbar.forward = Camera.main.transform.forward;
    }
    void OnEnable()
    {
        //if (!GetComponent<Renderer>().isVisible)
        //{
            //enabled = false;
        //}
    }
    void OnBecameVisible()
    {
        //enabled = true;
    }
    void OnBecameInvisible()
    {
        //enabled = false;
    }
}
