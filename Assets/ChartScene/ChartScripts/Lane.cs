using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Lane : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    private int lanes = 4;
    private int beat = 0;


    [SerializeField] private AudioClip song;
    [SerializeField] private int bpm;
    [SerializeField] private ChartSingleton _chartData;
    [SerializeField] private BuildDisplay _buildDisplay;

    private int subdivisions;

    private Note baseNote;
    private int baseBeat;
    private int baseSubBeat;

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





    public void OnPointerDown(PointerEventData eventData) {
        subdivisions = _chartData.getSubdivisions();

        float beatClicked = ChartSingleton.baseBeat + (float)(eventData.position.y / 540);
        int currLane = Mathf.FloorToInt(eventData.position.x / 160);
        int currBeat = Mathf.FloorToInt(beatClicked);

        int subBeat = Mathf.FloorToInt((beatClicked - (float)currBeat) * subdivisions) + 1;

        float time = (currBeat - 1 + (float)(subBeat - 1) / subdivisions) * BeatManager.BeatLength(bpm);

        Debug.Log("relBeat: " + subBeat);
        Debug.Log("CurrBeat: " + currBeat);
        Debug.Log("Time: " + time);
        Debug.Log("Lane: " + currLane);

        if (eventData.button == PointerEventData.InputButton.Left) {
            addNote(currLane, currBeat, subBeat, new Note(time, currLane, subdivisions, ChartSingleton.buildState));

            if (ChartSingleton.buildState == "Hold") {
                ChartSingleton.buildState = "Extending";
                baseNote = chartLanes[currLane][currBeat][subBeat];
                baseBeat = currBeat;
                baseSubBeat = subBeat;
            }
        } else if (eventData.button == PointerEventData.InputButton.Right) {

            Note clickedNote = removeNote(currLane, currBeat, subBeat);

            if (clickedNote != null && clickedNote.type == "Hold") {
                int holdCounter = 0;
                int holdBase = Mathf.FloorToInt(clickedNote.time / BeatManager.BeatLength(bpm)) + 1;
                int holdSub = Mathf.RoundToInt((((clickedNote.time / BeatManager.BeatLength(bpm)) + 1) - holdBase) * clickedNote.subdivision);
                while (holdCounter < clickedNote.holdBeats + 1) {

                    if (holdSub == clickedNote.subdivision) {
                        holdSub = 1;
                        holdBase++;
                    } else {
                        holdSub++;
                    }

                    Debug.Log("hb: " + holdBase);
                    Debug.Log("hs: " + holdSub);
                    removeNote(clickedNote.lane, holdBase, holdSub);

                    holdCounter++;
                }

            }

            Debug.Log("bruh");
        }


        _buildDisplay.updateDisplay();
    }


    public void OnPointerUp(PointerEventData eventData) {
        if (ChartSingleton.buildState == "Extending") {
            subdivisions = _chartData.getSubdivisions();
            ChartSingleton.buildState = "Hold";
            float beatClicked = ChartSingleton.baseBeat + (float)(eventData.position.y / 540);
            int currBeat = Mathf.FloorToInt(beatClicked);

            int subBeat = Mathf.FloorToInt((beatClicked - (float)currBeat) * subdivisions) + 1;

            float time = (currBeat - 1 + (float)(subBeat - 1) / subdivisions) * BeatManager.BeatLength(bpm);
            int holdBeats = Mathf.RoundToInt(((time - baseNote.time) / BeatManager.BeatLength(bpm)) * subdivisions);

            if (holdBeats > 0) {
                baseNote.holdBeats = holdBeats;

                int holdCounter = 0;

                while (holdCounter < holdBeats) {

                    if (baseSubBeat == subdivisions) {
                        baseSubBeat = 1;
                        baseBeat++;
                    } else {
                        baseSubBeat++;
                    }

                    addNote(baseNote.lane, baseBeat, baseSubBeat, baseNote);

                    holdCounter++;
                }

            } else {
                baseNote.type = "Normal";
            }

            Debug.Log("hold length: " + Mathf.RoundToInt(((time - baseNote.time) / BeatManager.BeatLength(bpm)) * subdivisions));




            _buildDisplay.updateDisplay();
        }


    }

    public List<NoteDisplay> getIntervalNotes() {
        List<NoteDisplay> notes = new List<NoteDisplay>();
        int min = Mathf.FloorToInt(ChartSingleton.baseBeat);
        int max = Mathf.CeilToInt(ChartSingleton.baseBeat + 1);
        float epsilon = 0.0001f;
        HashSet<Note> holdDuplicates = new HashSet<Note>();
        for (int i = 0; i < 4; i++) {
            for (int j = min; j <= max; j++) {
                if (chartLanes[i].ContainsKey(j)) {
                    for (int k = 1; k <= 9; k++) {
                        if (chartLanes[i][j].ContainsKey(k)) {
                            if (chartLanes[i][j][k].type == "Hold"
                                || chartLanes[i][j][k].time >= (ChartSingleton.baseBeat - 2) * BeatManager.BeatLength(bpm) - epsilon
                                && chartLanes[i][j][k].time <= (ChartSingleton.baseBeat + 1) * BeatManager.BeatLength(bpm) + epsilon) {
                                if (chartLanes[i][j][k].type == "Hold") {
                                    if (!holdDuplicates.Contains(chartLanes[i][j][k])) {
                                        holdDuplicates.Add(chartLanes[i][j][k]);
                                    } else {
                                        continue;
                                    }
                                }

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


    public List<NoteDisplay> getIntervalNotesScroll() {
        List<NoteDisplay> notes = new List<NoteDisplay>();
        int min = Mathf.FloorToInt(ChartSingleton.baseBeat);
        int max = Mathf.CeilToInt(ChartSingleton.baseBeat + 3);
        float epsilon = 0.0001f;
        HashSet<Note> holdDuplicates = new HashSet<Note>();
        for (int i = 0; i < 4; i++) {
            for (int j = min; j <= max; j++) {
                if (chartLanes[i].ContainsKey(j)) {
                    for (int k = 1; k <= 9; k++) {
                        if (chartLanes[i][j].ContainsKey(k)) {
                            if (chartLanes[i][j][k].type == "Hold"
                                || chartLanes[i][j][k].time >= (ChartSingleton.baseBeat - 2) * BeatManager.BeatLength(bpm) - epsilon
                                && chartLanes[i][j][k].time <= (ChartSingleton.baseBeat + 2) * BeatManager.BeatLength(bpm) + epsilon) {
                                if (chartLanes[i][j][k].type == "Hold") {
                                    if (!holdDuplicates.Contains(chartLanes[i][j][k])) {
                                        holdDuplicates.Add(chartLanes[i][j][k]);
                                    } else {
                                        continue;
                                    }
                                }

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

    public void addNote(int currLane, int currBeat, int subBeat, Note note) {
        if (chartLanes[currLane].ContainsKey(currBeat)) {
            if (!chartLanes[currLane][currBeat].ContainsKey(subBeat)) {
                chartLanes[currLane][currBeat].Add(subBeat, note);
            }
        } else {
            chartLanes[currLane].Add(currBeat, new Dictionary<int, Note>());
            chartLanes[currLane][currBeat].Add(subBeat, note);
        }
    }

    public Note removeNote(int currLane, int currBeat, int subBeat) {
        Note removed = null;
        if (chartLanes[currLane].ContainsKey(currBeat)) {
            if (chartLanes[currLane][currBeat].ContainsKey(subBeat)) {
                removed = chartLanes[currLane][currBeat][subBeat];
                chartLanes[currLane][currBeat].Remove(subBeat);

            }
        }
        return removed;
    }


}



