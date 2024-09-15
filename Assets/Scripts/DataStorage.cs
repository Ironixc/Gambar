using System.Collections.Generic;
using UnityEngine;

public class DataStorage
{
    private Dictionary<string, Vector2[]> data = new Dictionary<string, Vector2[]>();

    [System.Serializable]
    public class DataEntry
    {
        public string key;
        public Vector2[] values;
    }

    [System.Serializable]
    public class DataWrapper
    {
        public List<DataEntry> dataEntries;
    }

    public void AddData(string key, params Vector2[] values)
    {
        data[key] = values;
    }

    public Vector2[] GetData(string key)
    {
        if (data.ContainsKey(key))
        {
            return data[key];
        }
        else
        {
            return new Vector2[0];
        }
    }

    public void FromJson(string jsonData)
    {
        DataWrapper wrapper = JsonUtility.FromJson<DataWrapper>(jsonData);
        foreach (var entry in wrapper.dataEntries)
        {
            AddData(entry.key, entry.values);
        }
    }
}
