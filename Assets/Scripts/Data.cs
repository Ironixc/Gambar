using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainProgram : MonoBehaviour
{
    public static MainProgram Instance { get; private set; }
    public DataStorage storage;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        if (Instance == null)
        {
            GameObject instance = new GameObject("MainProgram");
            instance.AddComponent<MainProgram>();
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            storage = new DataStorage();

            // Load JSON from the "data" folder
            string filePath = Path.Combine(Application.dataPath, "data", "data.json");
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                storage.FromJson(jsonData);
            }
            else
            {
                Debug.LogError("Data file not found at " + filePath);
            }

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
