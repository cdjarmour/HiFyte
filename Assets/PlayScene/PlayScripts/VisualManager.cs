using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class VisualManager : MonoBehaviour {
    [SerializeField] private AudioSource _song;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject noteParent;
    [SerializeField] private TMP_Text textField;
    [SerializeField] private AudioClip hitSound;

    private const float BPM = 173f;
    public static int SPEED = 3;

    private bool playing = false;
    private List<Note>[] chart;
    private List<GameObject>[] chartDisplay = new List<GameObject>[4];
    private int[] laneIndex = new int[4];
    private int[] inputIndex = new int[4];
    public static int combo = 0;

    private float previouslyHeld = 0;


    void Start() {
        chart = ChartJSON.getSortedNotes("Catch me if you can");
        for (int i = 0; i < chart.Length; i++) {
            laneIndex[i] = 0;
            inputIndex[i] = 0;
            chartDisplay[i] = new List<GameObject>();
            foreach (Note note in chart[i]) {
                GameObject noteObject = Instantiate(notePrefab);
                switch (note.type) {
                    case NoteType.Normal : noteObject.AddComponent<PlayNoteDisplay>();
                    break;

                    case NoteType.Hold: noteObject.AddComponent<PlayHoldNoteDisplay>();
                    break;
                }
                noteObject.SetActive(false);
                noteObject.transform.SetParent(noteParent.transform, true);
                noteObject.GetComponent<PlayNoteDisplay>().createDisplay(note, _song, hitSound);
                chartDisplay[i].Add(noteObject);
            }
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            _song.Play();
            playing = !playing;
        }

        if (!playing) return;

        textField.text = combo.ToString();


        for (int i = 0; i < chartDisplay.Length; i++) {

            if (inputIndex[i] < chart[i].Count) {
                Note frontNote = chart[i][inputIndex[i]];
                if (frontNote.type == NoteType.Normal && _song.time >= frontNote.time + BeatManager.BeatLength(BPM) / 4) {
                    combo = 0;
                    inputIndex[i]++;
                } else if (frontNote.type == NoteType.Hold && !chartDisplay[i][inputIndex[i]].GetComponent<PlayHoldNoteDisplay>().getPressed()
                    && _song.time >= frontNote.time + BeatManager.BeatLength(BPM) / 4) {
                    combo = 0;
                    inputIndex[i]++;
                    Debug.Log("passed");
                }
            }

            if (laneIndex[i] >= chartDisplay[i].Count) continue;
            if (chartDisplay[i][laneIndex[i]].GetComponent<PlayNoteDisplay>().getTime() <= _song.time + BeatManager.BeatLength(BPM) * (10f / SPEED)) {
                chartDisplay[i][laneIndex[i]].SetActive(true);
                laneIndex[i]++;
            }
        }


        KeyInput(KeyCode.D, 0);
        KeyInput(KeyCode.F, 1);
        KeyInput(KeyCode.H, 2);
        KeyInput(KeyCode.J, 3);
    }
    //_song.time >= frontNote.time + ((frontNote.holdBeats + 1) / (float)frontNote.subdivision) * BeatManager.BeatLength(BPM)

    private void KeyInput(KeyCode key, int lane) {
        if (inputIndex[lane] >= chart[lane].Count) return;
        Note currNote = chart[lane][inputIndex[lane]];

        if (currNote.type == NoteType.Normal) {
            if (Input.GetKeyDown(key)) {
                if (_song.time <= currNote.time + BeatManager.BeatLength(BPM) / 4
                && _song.time >= currNote.time - BeatManager.BeatLength(BPM) / 4) {
                    chartDisplay[lane][inputIndex[lane]].SetActive(false);
                    inputIndex[lane]++;
                    _song.PlayOneShot(hitSound);
                    combo++;
                } else {
                    combo = 0;
                }
            }
        } else if (currNote.type == NoteType.Hold) {
            PlayHoldNoteDisplay currDisplay = chartDisplay[lane][inputIndex[lane]].GetComponent<PlayHoldNoteDisplay>();

            if (currDisplay.getPressed()) {
                if (Input.GetKey(key)) {
                    if (currDisplay.nextInterval() && _song.time <= currNote.time + ((currNote.holdBeats + 1) / (float)currNote.subdivision) * BeatManager.BeatLength(BPM)) {
                        combo++;
                        Debug.Log("holding");
                    } else if (_song.time >= currNote.time + ((currNote.holdBeats + 1) / (float)currNote.subdivision) * BeatManager.BeatLength(BPM)) {
                        inputIndex[lane]++;
                    }
                } else {
                    combo = 0;
                    inputIndex[lane]++;
                }
                return;
            }

            if (Input.GetKeyDown(key)) {
                if (_song.time <= currNote.time + BeatManager.BeatLength(BPM) / 4
                && _song.time >= currNote.time - BeatManager.BeatLength(BPM) / 4 && !currDisplay.getPressed()) {
                    currDisplay.pressFront();
                    combo++;
                    _song.PlayOneShot(hitSound);
                }
            }


        }

    }
}   





