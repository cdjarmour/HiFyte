using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Lane : MonoBehaviour, IPointerClickHandler {
    private int lanes = 4;
    private int beat = 0;


    [SerializeField] private AudioClip song;
    [SerializeField] private int bpm;
    [SerializeField] private ChartSingleton _chartData;

    private int subdivisions;

    private Dictionary<int, Dictionary<int, Note>>[] chartLanes;

    void Awake() {
        chartLanes = new Dictionary<int, Dictionary<int, Note>>[4];

        for (int i = 0; i < chartLanes.Length; i++) {
            chartLanes[i] = new Dictionary<int, Dictionary<int, Note>>();
        }
    }

    void Start() {
        subdivisions = _chartData.getSubdivisions();
    }



    public void OnPointerClick(PointerEventData eventData) {
        subdivisions = _chartData.getSubdivisions();

        float beatClicked = ChartSingleton.baseBeat + (float)(eventData.position.y / 540);
        int currLane = Mathf.FloorToInt(eventData.position.x / 160);
        int currBeat = Mathf.FloorToInt(beatClicked);

        int subBeat = Mathf.FloorToInt((beatClicked - (float)currBeat) * subdivisions) + 1;

        float time = (currBeat - 1 + (float) (subBeat - 1) / subdivisions) * BeatManager.BeatLength(bpm); 

        Debug.Log("relBeat: " + subBeat);
        Debug.Log("CurrBeat: " + currBeat);
        Debug.Log("Time: " + time);
        Debug.Log("Lane: " + currLane);

        if (chartLanes[currLane].ContainsKey(currBeat)) {
            if (chartLanes[currLane][currBeat].ContainsKey(subBeat)) {
            } else {
                chartLanes[currLane][currBeat].Add(subBeat, new Note(time, currLane, subdivisions));
            }
        } else {
            chartLanes[currLane].Add(currBeat, new Dictionary<int, Note>());
            chartLanes[currLane][currBeat].Add(subBeat, new Note(time, currLane, subdivisions));
        }

    }

    public List<NoteDisplay> getIntervalNotes() {
        List<NoteDisplay> notes = new List<NoteDisplay>();
        int min = Mathf.FloorToInt(ChartSingleton.baseBeat);
        int max = Mathf.CeilToInt(ChartSingleton.baseBeat + 1);
        float epsilon = 0.0001f;

        for (int i = 0; i < 4; i++) {
            for (int j = min; j <= max; j++) {
                if (chartLanes[i].ContainsKey(j)) {
                    for (int k = 1; k <= 9; k++) {
                        if (chartLanes[i][j].ContainsKey(k)) {
                            if (chartLanes[i][j][k].time >= (ChartSingleton.baseBeat - 2) * BeatManager.BeatLength(bpm) - epsilon
                                && chartLanes[i][j][k].time <= (ChartSingleton.baseBeat + 1) * BeatManager.BeatLength(bpm) + epsilon){

                                GameObject go = new GameObject("Note", typeof(NoteDisplay));
                                NoteDisplay nd = go.GetComponent<NoteDisplay>();
                                nd.setNote(chartLanes[i][j][k]);
                                notes.Add(nd);
                            }
                        }
                    }
                }
            }
        }


        return notes;
    }


    public List<NoteDisplay> getIntervalNotes(int min, int max) {
        List<NoteDisplay> notes = new List<NoteDisplay>();

        for (int i = 0; i < 4; i++) {
            for (int j = min; j <= max; j++) {
                if (chartLanes[i].ContainsKey(j)) {
                    for (int k = 1; k <= 9; k++) {
                        if (chartLanes[i][j].ContainsKey(k)) {
                                GameObject go = new GameObject("Note", typeof(NoteDisplay));
                                NoteDisplay nd = go.GetComponent<NoteDisplay>();
                                nd.setNote(chartLanes[i][j][k]);
                                notes.Add(nd);
                        }
                    }
                }
            }
        }


        return notes;
    }

}


