using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public delegate void ChangeSelect_del(int count);

public class CameraControl : MonoBehaviour
{
    public float PanSpeed = 15.0f;
    private Health selected;
    private Health target;
    private List<Unit> selected_units = new List<Unit>();
    //потом добавить переменные для турели и завода?
    public List<Transform> blue_team = new List<Transform>();
    public List<Transform> yellow_team = new List<Transform>();
    private new Camera camera;
    private float minX = -45, maxX = 40, minZ = -50, maxZ = 30, minY = 2, maxY = 15;

    // Start is called before the first frame update
    void Start()
    {
        camera = gameObject.GetComponent<Camera>();
    }
    public event ChangeSelect_del ChangeSelectIvent;

    // Update is called once per frame
    void Update()
    {//чувствительность колёсика мыши должна быть выставлена на 2
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), -Input.GetAxis("Mouse ScrollWheel"), Input.GetAxis("Vertical"));
        Vector3 new_pos = transform.position + move * PanSpeed * Time.deltaTime;
        if (new_pos.x > minX && new_pos.x < maxX && new_pos.y > minY && 
            new_pos.y < maxY && new_pos.z > minZ && new_pos.z < maxZ)
                transform.position = new_pos;
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {//левый клик для выбора или перемещения
            HandleSelectionOrMove();
            ChangeSelectIvent?.Invoke(selected_units.Count);
        }
        else if ((selected_units != null || selected != null) && Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {//правый клик для атаки
            HandleAction();
        }
    }
    public void HandleSelectionOrMove()
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //the collider could be children of the unit, so we make sure to check in the parent
            var chosen = hit.collider.GetComponentInParent<Health>();
            if (chosen == selected)
            {
                selected = null;
                if (chosen != null) chosen.Selected(false);
                if (chosen is Unit unit) selected_units.Remove(unit);
                //этот блок нужно запихнуть в следующий, но сейчас мне влом.
            }
            else if (chosen != null)
            {
                if (selected_units != null && !(Input.GetKey("left shift") || Input.GetKey("right shift")))
                {
                    foreach (var item in selected_units)
                    {
                        item.Selected(false);
                    }
                    selected_units.Clear();
                    if(selected != null) selected.Selected(false);
                }
                if (chosen is Unit unit) selected_units.Add(unit);
                selected = chosen;
                chosen.Selected(true);
            }
            else if (selected_units != null)
            {
                foreach (var item in selected_units)
                {
                    item.MoveTo(hit.point, true);
                }
            }
        }
    }
    public void HandleAction()
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //the collider could be children of the unit, so we make sure to check in the parent
            target = hit.collider.GetComponentInParent<Health>();
            if (target != null)
            {
                foreach (var item in selected_units)
                {
                    item.Action(target);
                }
            }
            else if (selected != null)
            {
                selected.Action(hit.point);
            }
            //check if the hit object have a IUIInfoContent to display in the UI
            //if there is none, this will be null, so this will hid the panel if it was displayed
            //var uiInfo = hit.collider.GetComponentInParent<UIMainScene.IUIInfoContent>();
            //UIMainScene.Instance.SetNewInfoContent(uiInfo);
        }
    }

    public void Bye(Unit unit)
    {
        selected_units.Remove(unit);
    }
}