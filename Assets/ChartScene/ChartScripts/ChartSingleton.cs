using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.TerrainTools;
using UnityEngine;

public class ChartSingleton : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    public static int bpm;
    public static int subdivisions;
    public static float baseBeat;
    public static Canvas canvas;
    public static String buildState;

    public void OnEnable() {
        canvas = _canvas;
        subdivisions = 8;
        baseBeat = 1;
        buildState = "Normal";
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
}

