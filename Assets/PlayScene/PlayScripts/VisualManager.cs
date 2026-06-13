using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class VisualManager : MonoBehaviour
{

    [SerializeField] AudioSource _song;
    [SerializeField] GameObject notePrefab;
    [SerializeField] GameObject noteParent;
    [SerializeField] TMP_Text textField;
    private int speed = 4;
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
                noteObject.GetComponent<PlayNoteDisplay>().createDisplay(note, _song);
                chartDisplay[i].Add(noteObject);
            }
        }
    }


    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.P)) {
            _song.Play();
            playing = !playing;
        }

        if (!playing) return;

        textField.text = combo.ToString();


        for (int i = 0; i < chartDisplay.Length; i++) {
            if (laneIndex[i] >= chartDisplay[i].Count) continue;
            if (chartDisplay[i][laneIndex[i]].GetComponent<PlayNoteDisplay>().getTime() <= _song.time + BeatManager.BeatLength(174) * (10f / speed)) {
                chartDisplay[i][laneIndex[i]].SetActive(true);
                laneIndex[i]++;
            }
        }

        for (int i = 0; i < chart.Length; i++) {
            if (_song.time - chart[i][inputIndex[i]].time >= BeatManager.BeatLength(174) / 4) {
                inputIndex[i]++;
            }
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            if (_song.time <= chart[0][inputIndex[0]].time + BeatManager.BeatLength(174) / 4
                && _song.time >= chart[0][inputIndex[0]].time - BeatManager.BeatLength(174) / 4) {
                chartDisplay[0][inputIndex[0]].SetActive(false);
                inputIndex[0]++;
                combo++;
            } else {
                combo = 0;
            }

        }

        if (Input.GetKeyDown(KeyCode.F)) {
            if (_song.time <= chart[1][inputIndex[1]].time + BeatManager.BeatLength(174) / 4
                && _song.time >= chart[1][inputIndex[1]].time - BeatManager.BeatLength(174) / 4) {
                chartDisplay[1][inputIndex[1]].SetActive(false);
                inputIndex[1]++;
                combo++;
            } else {
                combo = 0;
            }

        }

        if (Input.GetKeyDown(KeyCode.H)) {
            if (_song.time <= chart[2][inputIndex[2]].time + BeatManager.BeatLength(174) / 4
                && _song.time >= chart[2][inputIndex[2]].time - BeatManager.BeatLength(174) / 4) {
                chartDisplay[2][inputIndex[2]].SetActive(false);
                inputIndex[2]++;
                combo++;
            } else {
                combo = 0;
            }

        }

        if (Input.GetKeyDown(KeyCode.J)) {
            if (_song.time <= chart[3][inputIndex[3]].time + BeatManager.BeatLength(174) / 4
                && _song.time >= chart[3][inputIndex[3]].time - BeatManager.BeatLength(174) / 4) {
                chartDisplay[3][inputIndex[3]].SetActive(false);
                inputIndex[3]++;
                combo++;
            } else {
                combo = 0;
            }

        }


    }


}
