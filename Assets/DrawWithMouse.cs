using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawWithMouse : MonoBehaviour
{
    private LineRenderer line;
    private Vector3 previousPosition;
    private List<GameObject> lines = new List<GameObject>();
    private float lineWidth = 0.1f; // Set the desired line width here
    [SerializeField]
    private Button lineWidthButtonb;
    [SerializeField]
    private Button lineWidthButtons;

    [SerializeField]
    private float minDistance = 0.1f;

    [SerializeField]
    private Button lineWidthButtonm; // Assign the line width button in the inspector

    [SerializeField]
    private Material lineMaterial; // Assign a material with the desired color in the inspector

    [SerializeField]
    private Button screenshotButton; // Assign the screenshot button in the inspector

    [SerializeField]
    private Button greenButton; // Assign the blue color button in the inspector

    [SerializeField]
    private Button redButton; // Assign the blue color button in the inspector

    [SerializeField]
    private Button blueButton; // Assign the blue color button in the inspector

    [SerializeField]
    private Button blackButton; // Assign the black color button in the inspector

    [SerializeField]
    private RawImage screenshotImage; // Assign a RawImage component in the inspector to display the screenshot

    private void Start()
    {
        screenshotButton.onClick.AddListener(TakeScreenshot);
        redButton.onClick.AddListener(() => ChangeLineColor(Color.red));
        greenButton.onClick.AddListener(() => ChangeLineColor(Color.green));
        blueButton.onClick.AddListener(() => ChangeLineColor(Color.blue));
        blackButton.onClick.AddListener(() => ChangeLineColor(Color.black));
        lineWidthButtons.onClick.AddListener(() => ChangeLineWidth(0.1f));
        lineWidthButtonb.onClick.AddListener(() => ChangeLineWidth(1f));
        lineWidthButtonm.onClick.AddListener(() => ChangeLineWidth(0.5f));
        lineWidthButtons.onClick.AddListener(() => ChangeLineWidth(0.1f));


    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject lineObj = new GameObject("Line");
            line = lineObj.AddComponent<LineRenderer>();
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.material = lineMaterial;
            line.material = new Material(lineMaterial); // Create a new material instance for each line
            line.positionCount = 1;
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0f;
            line.SetPosition(0, currentPosition);
            previousPosition = currentPosition;
            lines.Add(lineObj);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0f;
            if (Vector3.Distance(currentPosition, previousPosition) > minDistance)
            {
                line.positionCount++;
                line.SetPosition(line.positionCount - 1, currentPosition);
                previousPosition = currentPosition;
            }
        }
    }

    private void ChangeLineWidth(float width)
    {
        lineWidth = width;
        if (line != null)
        {
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
        }
    }

    private void ChangeLineColor(Color color)
    {
        lineMaterial.color = color;
    }
    private void EraseLines()
    {
        foreach (GameObject lineObj in lines)
        {
            Destroy(lineObj);
        }
        lines.Clear();
    }
    private void TakeScreenshot()
    {
        StartCoroutine(CaptureAndShowScreenshot());
        StartCoroutine(DelayedEraseLines());
    }

    private IEnumerator DelayedEraseLines()
    {
        yield return new WaitForSeconds(0.1f);
        EraseLines();
    }

    private IEnumerator CaptureAndShowScreenshot()
    {
        yield return new WaitForEndOfFrame();

        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        Camera.main.targetTexture = renderTexture;
        Camera.main.Render();
        RenderTexture.active = renderTexture;

        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        screenshotImage.texture = screenshot;
        screenshotImage.gameObject.SetActive(true);
    }
}
