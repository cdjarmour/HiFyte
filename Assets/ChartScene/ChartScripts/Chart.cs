using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public class Chart {
    public ChartData metaData;
    public List<Note> notes = new List<Note>();
}

[Serializable]
public class ChartData {
    public string name;
    public float bpm;
    public float duration;
    public String audioFilePath;


    public ChartData(string name, float bpm, float duration, String audio) {
        this.name = name;
        this.bpm = bpm;
        this.duration = duration;
        this.audioFilePath = audio;
    }
}

[Serializable]
public class Note {
    public float time;
    public int lane;
    public int subdivision;
    public float beat;

    public Note(float time, int lane, int subdivision) {
        this.time = time;
        this.lane = lane;
        this.subdivision = subdivision;
    }
}


public class ChartJSON {

    public static bool hasChart(String name) {
        if (Resources.Load<TextAsset>(Path.Combine("Charts", name + "Chart")) != null) {
            return true;
        }

        return false;
    }

    public static void ConvertToJSON(Chart chart) {
        string path = Path.Combine(Application.dataPath, "ChartScene", "Resources", "Charts", chart.metaData.name + "Chart.json");
        string json = JsonUtility.ToJson(chart, true);

        using StreamWriter sw = new StreamWriter(path);
        sw.Write(json);
        Debug.Log("file written");
    }


    public static Chart ConvertToChart(String songName) {
        string path = Path.Combine(Application.dataPath, "ChartScene", "Resources", "Charts", songName + "Chart.json");
        using StreamReader sr = new StreamReader(path);

        string json = sr.ReadToEnd();
        return JsonUtility.FromJson<Chart>(json);
    }
}

