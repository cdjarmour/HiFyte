using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNoteDisplay : MonoBehaviour
{
    Note note;
    AudioSource song;
    float timeRemaining;
    float beatLength = BeatManager.BeatLength(174);


    public void createDisplay(Note note, AudioSource song) {
        this.note = note;
        this.song = song;
    }


    // Update is called once per frame
    void Update() {
        timeRemaining = (note.time - song.time) / beatLength;
        transform.position = new Vector3(-1.2f + (note.lane * 0.8f), 0, timeRemaining * 4);


        if (timeRemaining <= -(beatLength / 2f)) {
            gameObject.SetActive(false);
        }
    }

    public float getTime() {
        return note.time;
    }
}
