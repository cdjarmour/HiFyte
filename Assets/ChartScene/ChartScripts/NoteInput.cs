using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class NoteInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    [SerializeField] private BuildDisplay _buildDisplay;
    private const int MAX_SUBDIVISIONS = 9;
    private const int LANES = 4;
    private const int BEAT_HEIGHT = 540;
    private const int LANE_WIDTH = 160;

    private int subdivisions;

    private Note baseNote;
    private int baseBeat;
    private int baseSubBeat;

    private Dictionary<int, Dictionary<int, Note>>[] chartLanes;

    void OnEnable() {
        subdivisions = ChartSingleton.instance.getSubdivisions();
    }





    public void OnPointerDown(PointerEventData eventData) {
        subdivisions = ChartSingleton.instance.getSubdivisions();

        float beatClicked = ChartSingleton.instance.getBaseBeat() + (float)(eventData.position.y / BEAT_HEIGHT);
        int currLane = Mathf.FloorToInt(eventData.position.x / LANE_WIDTH);
        int currBeat = Mathf.FloorToInt(beatClicked);

        int subBeat = Mathf.FloorToInt((beatClicked - (float)currBeat) * subdivisions) + 1;

        float time = (currBeat - 1 + (float)(subBeat - 1) / subdivisions) * BeatManager.BeatLength(ChartSingleton.instance.getBPM());

        Debug.Log("relBeat: " + subBeat);
        Debug.Log("CurrBeat: " + currBeat);
        Debug.Log("Time: " + time);
        Debug.Log("Lane: " + currLane);

        if (eventData.button == PointerEventData.InputButton.Left) {
            addNote(currLane, currBeat, subBeat, new Note(time, currLane, subdivisions, ChartSingleton.instance.getState()));

            if (ChartSingleton.instance.getState() == "Hold") {
                ChartSingleton.instance.setState("Extending");
                baseNote = chartLanes[currLane][currBeat][subBeat];
                baseBeat = currBeat;
                baseSubBeat = subBeat;
            }
        } else if (eventData.button == PointerEventData.InputButton.Right) {

            Note clickedNote = removeNote(currLane, currBeat, subBeat);

            if (clickedNote != null && clickedNote.type == "Hold") {
                int holdCounter = 0;
                int holdBase = Mathf.FloorToInt(clickedNote.time / BeatManager.BeatLength(ChartSingleton.instance.getBPM())) + 1;
                int holdSub = Mathf.RoundToInt((((clickedNote.time / BeatManager.BeatLength(ChartSingleton.instance.getBPM())) + 1) - holdBase) * clickedNote.subdivision);
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
        if (ChartSingleton.instance.getState() == "Extending") {
            subdivisions = ChartSingleton.instance.getSubdivisions();
            ChartSingleton.instance.setState("Hold");
            float beatClicked = ChartSingleton.instance.getBaseBeat() + (float)(eventData.position.y / BEAT_HEIGHT);
            int currBeat = Mathf.FloorToInt(beatClicked);

            int subBeat = Mathf.FloorToInt((beatClicked - (float)currBeat) * subdivisions) + 1;

            float time = (currBeat - 1 + (float)(subBeat - 1) / subdivisions) * BeatManager.BeatLength(ChartSingleton.instance.getBPM());
            int holdBeats = Mathf.RoundToInt(((time - baseNote.time) / BeatManager.BeatLength(ChartSingleton.instance.getBPM())) * subdivisions);

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

            Debug.Log("hold length: " + Mathf.RoundToInt(((time - baseNote.time) / BeatManager.BeatLength(ChartSingleton.instance.getBPM()) * subdivisions)));




            _buildDisplay.updateDisplay();
        }


    }

    public List<NoteDisplay> getIntervalNotes(int upper) {
        List<NoteDisplay> notes = new List<NoteDisplay>();
        int min = Mathf.FloorToInt(ChartSingleton.instance.getBaseBeat());
        int max = Mathf.CeilToInt(ChartSingleton.instance.getBaseBeat() + upper);




        float epsilon = 0.0001f;
        HashSet<Note> holdDuplicates = new HashSet<Note>();
        for (int i = 0; i < 4; i++) {
            for (int j = min; j <= max; j++) {
                if (chartLanes[i].ContainsKey(j)) {
                    for (int k = 1; k <= MAX_SUBDIVISIONS; k++) {
                        if (chartLanes[i][j].ContainsKey(k)) {
                            if (chartLanes[i][j][k].type == "Hold"
                                || (chartLanes[i][j][k].time >= (ChartSingleton.instance.getBaseBeat() - 2) * BeatManager.BeatLength(ChartSingleton.instance.getBPM()) - epsilon
                                && chartLanes[i][j][k].time <= (ChartSingleton.instance.getBaseBeat() + upper) * BeatManager.BeatLength(ChartSingleton.instance.getBPM()) + epsilon)) {
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

    public Dictionary<int, Dictionary<int, Note>>[] getNotes() {
        return chartLanes;
    }

    public void setLanes (Dictionary<int, Dictionary<int, Note>>[] notes) {
        chartLanes = notes;
    }


}



