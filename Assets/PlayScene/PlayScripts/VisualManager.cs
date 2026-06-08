using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VisualManager : MonoBehaviour
{

    [SerializeField] AudioSource _song;
    [SerializeField] GameObject note;
    float bpm = 97;
    float speed = 10;
    float startPos;
    float beatLength;
    float timeNote;
    bool playing = false;
    IReadOnlyList<Note> lane1;
    IReadOnlyList<Note> lane2;

    void Start() {
        beatLength = 60f / bpm;
        _song.time = beatLength * 10;
        _song.Play();
        timeNote = (_song.time * 2) + beatLength;
        playing = true;



        Debug.Log("Song Time:" + _song.time);
        Debug.Log("Time: " + timeNote);
    }


    // Update is called once per frame
    void Update()
    {
        if (!playing) return;
        note.transform.position = new Vector3(note.transform.position.x, note.transform.position.y, ((timeNote - _song.time) / beatLength) * speed);

        if (((timeNote - _song.time) / beatLength) <= 0) {
            playing = false;
            note.transform.position = new Vector3(note.transform.position.x, transform.position.y, 0);
        }
    }
}
