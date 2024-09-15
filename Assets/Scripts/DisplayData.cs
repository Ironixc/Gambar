using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayData : MonoBehaviour
{
    private List<Vector2> points = new List<Vector2>();
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();
    private string dataKey = DrawWithMouse.dataKey;
    int start = 2 * DrawWithMouse.Line;

    void Start()
    {
        UpdateData();
    }

    void Update()
    {
        if (dataKey != DrawWithMouse.dataKey || start != 2 * DrawWithMouse.Line)
        {
            dataKey = DrawWithMouse.dataKey;
            start = 2 * DrawWithMouse.Line;
            UpdateData();
        }

        float radius = 0.1f;
        float theta = Time.time % (2 * Mathf.PI);

        for (int i = 0; i < start && i < points.Count; i++)
        {
            Vector3 endPosition = new Vector3(points[i].x + radius * Mathf.Cos(theta), points[i].y + radius * Mathf.Sin(theta), 0);
            lineRenderers[i].SetPosition(1, endPosition);
        }
    }

    void UpdateData()
    {
        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            Destroy(lineRenderer.gameObject);
        }
        lineRenderers.Clear();

        Vector2[] data = MainProgram.Instance.storage.GetData(dataKey);
        points = new List<Vector2>(data);

        for (int i = 0; i < start && i < points.Count; i++)
        {
            Vector2 point = points[i];

            GameObject lineObj = new GameObject("Line");
            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;

            lineRenderer.SetPosition(0, new Vector2(point.x, point.y));

            lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
            lineRenderer.material.color = Color.black;

            lineRenderers.Add(lineRenderer);
        }
    }


}
