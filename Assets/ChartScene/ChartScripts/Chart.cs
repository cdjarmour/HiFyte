using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;


[Serializable]
public class Chart {
    public ChartData metaData;
    public Dictionary<int, Dictionary<int, Note>>[] notes;
}

[Serializable]
public class ChartData {
    public string name;
    public string composer;
    public int bpm;
    public float samplePos;
    public string audioFilePath;
    public string imageFilePath;
    public string notesFilePath;


    public ChartData(string name, string composer, int bpm, float samplePos) {
        this.name = name;
        this.composer = composer;
        this.bpm = bpm;
        this.audioFilePath = "AudioFiles/" + name;
        this.imageFilePath = "ChartCover/" + name;
        this.notesFilePath = "NoteData/" + name + "Notes";
        this.samplePos = samplePos;
    }
}


[Serializable]
public class Note {
    public float time;
    public int lane;
    public int subdivision;
    public int holdBeats;
    public String type;

    public Note() { }
    public Note(float time, int lane, int subdivision, int holdBeats, String nt) {
        this.type = nt;
        this.time = time;
        this.lane = lane;
        this.subdivision = subdivision;
        this.holdBeats = holdBeats;
        this.type = nt;
    }

    public Note(float time, int lane, int subdivision, String nt)
        : this(time, lane, subdivision, 0, nt)  {}

    public Note(float time, int lane, int subdivision)
    : this(time, lane, subdivision, 0, "Normal") { }

    public override bool Equals(object obj) {
        if (obj is Note other)
            return Mathf.Abs(time - other.time) < 0.0001f && lane == other.lane;
        return false;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Mathf.RoundToInt(time * 10000), lane);
    }
}





public static class ChartJSON {
    public static bool hasChart(String name) {
        return (Resources.Load<TextAsset>("ChartData/MetaData/" + name) != null);
    }
    public static void createChart(string name, string composer, int bpm, float samplePos) {
        ChartData newChart = new ChartData(name, composer, bpm, samplePos);
        Dictionary<int, Dictionary<int, Note>>[] notes = new Dictionary<int, Dictionary<int, Note>>[4];
        for (int i = 0; i < notes.Length; i++) {
            notes[i] = new Dictionary<int, Dictionary<int, Note>>();
        }
        string dataPath = Path.Combine(Application.dataPath, "Resources", "ChartData", "MetaData", newChart.name + ".json");
        string notePath = Path.Combine(Application.dataPath, "Resources", "ChartData", newChart.notesFilePath + ".json");

        string dataJSON = JsonConvert.SerializeObject(newChart, Formatting.Indented);
        string noteJSON = JsonConvert.SerializeObject(notes, Formatting.Indented);

        Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
        Directory.CreateDirectory(Path.GetDirectoryName(notePath));

        using StreamWriter dataSW = new StreamWriter(dataPath);
        dataSW.Write(dataJSON);

        using StreamWriter noteSW = new StreamWriter(notePath);
        noteSW.Write(noteJSON);
        UnityEditor.AssetDatabase.Refresh();
    }


    public static Dictionary<int, Dictionary<int, Note>>[] getNotes(ChartData mdata) {
        string data = Resources.Load<TextAsset>("ChartData/" + mdata.notesFilePath).text;

        return JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, Note>>[]>(data);
    }

    public static ChartData getMetaData(string name) {
        string data = Resources.Load<TextAsset>("ChartData/MetaData/" + name).text;

        return JsonConvert.DeserializeObject<ChartData>(data);
    }

    public static void convertNotes(ChartData data, Dictionary<int, Dictionary<int, Note>>[] notes) {
        string notePath = Path.Combine(Application.dataPath, "Resources", "ChartData", data.notesFilePath + ".json");

        string noteJSON = JsonConvert.SerializeObject(notes, Formatting.Indented);

        using StreamWriter sw = new StreamWriter(notePath);
        sw.Write(noteJSON);
    }

}

