using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisualManager : MonoBehaviour {
    [SerializeField] private AudioSource _song;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject noteParent;
    [SerializeField] private TMP_Text textField;
    [SerializeField] private AudioClip hitSound;

    private const float BPM = 173f;
    private const int SPEED = 2;

    private bool playing = false;
    private List<Note>[] chart;
    private List<GameObject>[] chartDisplay = new List<GameObject>[4];
    private int[] laneIndex = new int[4];
    private int[] inputIndex = new int[4];
    private int combo = 0;


    void Start() {
        chart = ChartJSON.getSortedNotes("Catch me if you can");
        for (int i = 0; i < chart.Length; i++) {
            laneIndex[i] = 0;
            inputIndex[i] = 0;
            chartDisplay[i] = new List<GameObject>();
            foreach (Note note in chart[i]) {
                GameObject noteObject = Instantiate(notePrefab);
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

            while (inputIndex[i] < chart[i].Count &&
            _song.time - chart[i][inputIndex[i]].time >= BeatManager.BeatLength(173) / 4) {
                combo = 0;
                inputIndex[i]++;
            }

            if (laneIndex[i] >= chartDisplay[i].Count) continue;
            if (chartDisplay[i][laneIndex[i]].GetComponent<PlayNoteDisplay>().getTime() <= _song.time + BeatManager.BeatLength(BPM) * (10f / SPEED)) {
                chartDisplay[i][laneIndex[i]].SetActive(true);
                laneIndex[i]++;
            }
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            if (inputIndex[0] >= chart[0].Count) return;
            if (_song.time <= chart[0][inputIndex[0]].time + BeatManager.BeatLength(BPM) / 4
                && _song.time >= chart[0][inputIndex[0]].time - BeatManager.BeatLength(BPM) / 4) {
                Debug.Log(chart[0][inputIndex[0]].time - _song.time);
                chartDisplay[0][inputIndex[0]].SetActive(false);
                inputIndex[0]++;
                _song.PlayOneShot(hitSound);
                combo++;
            } else {
                combo = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            if (inputIndex[1] >= chart[1].Count) return;
            if (_song.time <= chart[1][inputIndex[1]].time + BeatManager.BeatLength(BPM) / 4
                && _song.time >= chart[1][inputIndex[1]].time - BeatManager.BeatLength(BPM) / 4) {
                Debug.Log(chart[1][inputIndex[1]].time - _song.time);
                chartDisplay[1][inputIndex[1]].SetActive(false);
                inputIndex[1]++;
                _song.PlayOneShot(hitSound);
                combo++;
            } else {
                combo = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            if (inputIndex[2] >= chart[2].Count) return;
            if (_song.time <= chart[2][inputIndex[2]].time + BeatManager.BeatLength(BPM) / 4
                && _song.time >= chart[2][inputIndex[2]].time - BeatManager.BeatLength(BPM) / 4) {
                Debug.Log(chart[2][inputIndex[2]].time - _song.time);
                chartDisplay[2][inputIndex[2]].SetActive(false);
                inputIndex[2]++;
                _song.PlayOneShot(hitSound);
                combo++;
            } else {
                combo = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            if (inputIndex[3] >= chart[3].Count) return;
            if (_song.time <= chart[3][inputIndex[3]].time + BeatManager.BeatLength(BPM) / 4
                && _song.time >= chart[3][inputIndex[3]].time - BeatManager.BeatLength(BPM) / 4) {
                Debug.Log(chart[3][inputIndex[3]].time - _song.time);
                chartDisplay[3][inputIndex[3]].SetActive(false);
                inputIndex[3]++;
                _song.PlayOneShot(hitSound);
                combo++;
            } else {
                combo = 0;
            }
        }
    }


}





