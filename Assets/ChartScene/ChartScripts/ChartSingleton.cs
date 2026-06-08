using System;
using UnityEngine;

public enum BuildState {
    Normal,
    Hold,
    Tap,
    Release
}

public class ChartSingleton : MonoBehaviour
{
    #region Static Members
    private const int INITIAL_SUBDIVISIONS = 8;
    private const int INITIAL_BASE_BEAT = 1;
    private const string INITIAL_BUILD_STATE = "Normal";
    [SerializeField] private Canvas _CANVAS;
    #endregion Static Members

    public static ChartSingleton instance { get; private set; }

    private int subdivisions;
    private String buildState;
    private int bpm;
    private float baseBeat;
    private Canvas canvas; 




    void Awake() {
        instance = this;
        canvas = _CANVAS;
        subdivisions = INITIAL_SUBDIVISIONS;
        baseBeat = INITIAL_BASE_BEAT;
        buildState = INITIAL_BUILD_STATE;
    }


    public void resetData() {
        canvas = _CANVAS;
        subdivisions = INITIAL_SUBDIVISIONS;
        baseBeat = INITIAL_BASE_BEAT;
        buildState = INITIAL_BUILD_STATE;
    }


    public Canvas getCanvas() {
        return canvas;
    }
    public int getBPM() {
        return bpm;
    }

    public float getBaseBeat() {
        return baseBeat;
    }

    public String getState() {
        return buildState;
    }

    public int getSubdivisions() {
        return subdivisions;
    }

    public void setBPM(int newBpm) {
        bpm = newBpm;
    }

    public void setState(string newState) {
        buildState = newState;
    }

    public void setBaseBeat(float newBaseBeat) {
        baseBeat = newBaseBeat;
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

