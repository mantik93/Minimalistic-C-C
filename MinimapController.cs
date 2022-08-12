using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MinimapController : MonoBehaviour
{
    public Collider ground;
    public Material material;
    public float lineWidth;
    public new Camera camera;
    public RectTransform image;
    private Transform cameraT;
    private Camera minimap;
    private Vector3 worldSize;
    bool signal;

    // Start is called before the first frame update
    void Start()
    {
        minimap = gameObject.GetComponent<Camera>();
        cameraT = camera.GetComponent<Transform>();
        GetWorldSize();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MinimapClick()
    {
        var miniMapRect = image.rect;
        var screenRect = new Rect(
            image.transform.position.x,
            image.transform.position.y,
            miniMapRect.width, miniMapRect.height);

        var mousePos = Input.mousePosition;
        mousePos.y -= screenRect.y;
        mousePos.x -= screenRect.x;

        var camPos = new Vector3(
            mousePos.x * (worldSize.x / screenRect.width),
            cameraT.position.y,
            mousePos.y * (worldSize.y / screenRect.height) - 15
            );
        cameraT.position = camPos;
    }
    private void OnPostRender()
    {
        Vector3 point = minimap.WorldToViewportPoint(GetPointOnGround(new Vector3(0, 0)));
        float minX = point.x;
        float minY = point.y;
        point = minimap.WorldToViewportPoint(GetPointOnGround(new Vector3(camera.pixelWidth * 0.9f, camera.pixelHeight * 0.9f)));
        float maxX = point.x;
        float maxY = point.y;

        GL.PushMatrix();
        material.SetPass(0);
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Color(Color.white);
        {
            GL.Vertex3(minX, minY + lineWidth, 0);
            GL.Vertex3(minX, minY - lineWidth, 0);
            GL.Vertex3(maxX, minY - lineWidth, 0);
            GL.Vertex3(maxX, minY + lineWidth, 0);

            GL.Vertex3(minX + lineWidth, minY, 0);
            GL.Vertex3(minX - lineWidth, minY, 0);
            GL.Vertex3(minX - lineWidth, maxY, 0);
            GL.Vertex3(minX + lineWidth, maxY, 0);

            GL.Vertex3(minX, maxY + lineWidth, 0);
            GL.Vertex3(minX, maxY - lineWidth, 0);
            GL.Vertex3(maxX, maxY - lineWidth, 0);
            GL.Vertex3(maxX, maxY + lineWidth, 0);

            GL.Vertex3(maxX + lineWidth, minY, 0);
            GL.Vertex3(maxX - lineWidth, minY, 0);
            GL.Vertex3(maxX - lineWidth, maxY, 0);
            GL.Vertex3(maxX + lineWidth, maxY, 0);
        }    
        GL.End();
        GL.PopMatrix();
    }
    Vector3 GetPointOnGround(Vector3 position)
    {
        var ray = camera.ScreenPointToRay(position);
        RaycastHit hitInfo;
        if (!ground.Raycast(ray, out hitInfo, 200))
        {
            signal = true;
        }
        return hitInfo.point;
    }
    void GetWorldSize()
    {
        var ray = minimap.ScreenPointToRay(new Vector3(0, 0));
        RaycastHit hitInfo;
        ground.Raycast(ray, out hitInfo, 200);
        var ray2 = minimap.ScreenPointToRay(new Vector3(minimap.pixelWidth, minimap.pixelHeight));
        RaycastHit hitInfo2;
        ground.Raycast(ray2, out hitInfo2, 200);
        worldSize.x = hitInfo2.point.x - hitInfo.point.x;
        worldSize.y = hitInfo2.point.z - hitInfo.point.z;
    }
}
