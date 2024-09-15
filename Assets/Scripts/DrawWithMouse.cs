using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DrawWithMouse : MonoBehaviour
{
    public static bool JN = false;
    public static int Line = 1;
    public static string dataKey = "a";
    public static int  jmlN;
    private int i = 0;
    public static Vector2 data1;
    public static Vector2 data2;
    private int lineLimit;//line limit
    private bool isLineLimitReached = false;// if false
    public Vector2 startPosition;
    public Vector2 endPosition;
    private int currentLines = 0;
    public  int Nilai;
    private LineRenderer line;
    private Vector2 previousPosition;
    private List<GameObject> lines = new List<GameObject>();
    private float lineWidth = 0.1f; // Set the desired line width here

    [SerializeField]
    private Button DelLine;

    [SerializeField]
    private float minRigt = 1;//nilai minimal benarnya

    [SerializeField]
    private float minDistance = 0.1f;

    [SerializeField]
    private Material lineMaterial; // Assign a material with the desired color in the inspector

    [SerializeField]
    private Button screenshotButton; // Assign the screenshot button in the inspector

    [SerializeField]
    private Button greenButton;

    [SerializeField]
    private Button redButton;

    [SerializeField]
    private Button blueButton;

    [SerializeField]
    private Button blackButton;

    [SerializeField]
    private RawImage[] screenshotImages;
    private int currentImageIndex = 0;

    [SerializeField]
    private Button A;

    [SerializeField]
    private Button B;

    [SerializeField]
    private Button C;
    private void Start()
    {
        DelLine.onClick.AddListener(EraseLines);
        screenshotButton.onClick.AddListener(TakeScreenshot);
        redButton.onClick.AddListener(() => ChangeLineColor(Color.red));
        greenButton.onClick.AddListener(() => ChangeLineColor(Color.green));
        blueButton.onClick.AddListener(() => ChangeLineColor(Color.blue));
        blackButton.onClick.AddListener(() => ChangeLineColor(Color.black));
        A.onClick.AddListener(() => ChangeDataKey("a"));
        B.onClick.AddListener(() => ChangeDataKey("b"));
        C.onClick.AddListener(() => ChangeDataKey("c"));
    }

    private void Update()
    {
        Vector2[] data = MainProgram.Instance.storage.GetData(dataKey);
        lineLimit = (data.Length / 2) + 1; // line limit
        JN = false;
        if (!isLineLimitReached && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 currentPosition;

            if (Input.GetMouseButtonDown(0) && currentLines <= lineLimit)
            {
                GameObject lineObj = new GameObject("Lines");
                line = lineObj.AddComponent<LineRenderer>();
                line.startWidth = lineWidth;
                line.endWidth = lineWidth;
                line.material = lineMaterial;
                line.material = new Material(lineMaterial); // Create a new material instance for each line
                line.positionCount = 1;
                currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                line.SetPosition(0, currentPosition);
                startPosition = currentPosition; // Save the start position
                previousPosition = currentPosition;
                lines.Add(lineObj);
                currentLines++;
                if (currentLines >= lineLimit)
                {
                    isLineLimitReached = true;
                }
            }
            else if (Input.GetMouseButton(0) && currentLines <= lineLimit)
            {
                currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(currentPosition, previousPosition) > minDistance)
                {
                    line.positionCount++;
                    line.SetPosition(line.positionCount - 1, currentPosition);
                    endPosition = currentPosition; // Save the end position
                    previousPosition = currentPosition;
                }

            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (i < data.Length && i % 2 == 0)
                {
                    data1.x = data[i].x;
                    data1.y = data[i].y;
                    data2.x = data[i + 1].x;
                    data2.y = data[i + 1].y;
                    Debug.Log("Start Position: " + startPosition);
                    Debug.Log("End Position: " + endPosition);
                    if (Mathf.Abs(startPosition.x - data1.x) <= minRigt && Mathf.Abs(startPosition.y - data1.y) <= minRigt
                        && (endPosition.x - data2.x) <= minRigt && Mathf.Abs(endPosition.y - data2.y) <= minRigt)
                    {
                        Debug.Log("Benar");
                        float distancex1 = Mathf.Abs(startPosition.x - data1.x);
                        float distancey1 = Mathf.Abs(startPosition.y - data1.y);
                        float distancex2 = Mathf.Abs(endPosition.x - data2.x);
                        float distancey2 = Mathf.Abs(endPosition.y - data2.y);
                        float nilaix1 = minRigt - (distancex1 + distancey1);
                        float nilaix2 = minRigt - (distancex2 + distancey2);
                        float Nilais = 50 * (nilaix1 + nilaix2);
                        Nilai = Mathf.RoundToInt(Nilais);
                        Debug.Log(Nilai);
                    }
                    else
                    {
                        Nilai = 0;
                        Debug.Log(Nilai);
                        Debug.Log("salah");
                    }
                    i += 2;
                    Line++;
                }
                jmlN += Nilai / (data.Length / 2);
                if (i >= data.Length || i % 2 != 0)
                {
                    JN = true;
                    jmlN = 0;
                    i = 0;
                }

            }
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
        Line = 1;
        currentLines = 0;
        isLineLimitReached = false;
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

        screenshotImages[currentImageIndex].texture = screenshot;
        screenshotImages[currentImageIndex].gameObject.SetActive(true);

        currentImageIndex = (currentImageIndex + 1) % screenshotImages.Length;
    }
    public void ChangeDataKey(string newKey)
    {
        dataKey = newKey;
        StartCoroutine(CaptureAndShowScreenshot());
        StartCoroutine(DelayedEraseLines());

    }
}
