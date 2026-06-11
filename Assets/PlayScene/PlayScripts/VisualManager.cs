using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VisualManager : MonoBehaviour
{

    [SerializeField] AudioSource _song;
    [SerializeField] GameObject notePrefab;
    private int speed = 5;
    private bool playing = false;
    private List<Note>[] chart;
    private List<GameObject>[] chartDisplay = new List<GameObject>[4];
    private int[] laneIndex = new int[4];

    void Start() {
        chart = ChartJSON.getSortedNotes("lumivoyage");
        for (int i = 0; i < chart.Length; i++) {
            laneIndex[i] = 0;
            chartDisplay[i] = new List<GameObject>();
            foreach (Note note in chart[i]) {
                GameObject noteObject = Instantiate(notePrefab);
                noteObject.SetActive(false);
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

        for (int i = 0; i < chartDisplay.Length; i++) {
            if (laneIndex[i] >= chartDisplay[i].Count) continue;
            if (chartDisplay[i][laneIndex[i]].GetComponent<PlayNoteDisplay>().getTime() <= _song.time + BeatManager.BeatLength(97) * (10f / speed)) {
                chartDisplay[i][laneIndex[i]].SetActive(true);
                laneIndex[i]++;
            }
        }

    
    }

}
