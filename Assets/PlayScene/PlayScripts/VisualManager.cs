using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VisualManager : MonoBehaviour
{

    [SerializeField] AudioSource _song;
    [SerializeField] GameObject note;
    float bpm = 97;
    float speed = 8;
    float beatLength;
    float timeNote;
    float timeNeeded;
    bool playing = false;
    IReadOnlyList<Note> lane1;
    IReadOnlyList<Note> lane2;

    void Start() {
        beatLength = 60f / bpm;
        playing = false;

    }


    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.P)) {
            _song.time = beatLength * 16;
            _song.Play();
            timeNote = _song.time * 2;
            playing = true;
        }

        if (!playing) return;

        if (_song.time >= timeNote) {
            Debug.Log("Bruh");
        }

        timeNeeded = (timeNote - _song.time) / beatLength;

        note.transform.position = new Vector3(note.transform.position.x, 0, timeNeeded * speed);

    
    
    }

}
