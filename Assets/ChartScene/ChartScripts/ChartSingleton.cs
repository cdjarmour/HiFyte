using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class ChartSingleton : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private int _bpm;
    [SerializeField] private int _subdivisions;


    public static int bpm;
    public static int subdivisions;
    public static float baseBeat;
    public static Canvas canvas;

    public void Awake() {
        canvas = _canvas;
        bpm = _bpm;
        subdivisions = _subdivisions;
        baseBeat = 1;
    }

    public int getSubdivisions() {
        return subdivisions;
    }

    public void setSubdivisions(int sd) {
        baseBeat = Mathf.Round(baseBeat * sd) / sd;
        subdivisions = sd;
        Debug.Log(baseBeat);
    }

    public void BeatAdd() {
        baseBeat = baseBeat + (1f/  (float) subdivisions);
        baseBeat = Mathf.Round(baseBeat * subdivisions) / subdivisions;
        Debug.Log(baseBeat);
    }

    public void BeatRemove() {
        baseBeat = baseBeat - (1f/ (float) subdivisions);
        baseBeat = Mathf.Round(baseBeat * subdivisions) / subdivisions;
        Debug.Log(baseBeat);
    }

    public void setBeat(int beat) {
        baseBeat = beat;
    }


}
