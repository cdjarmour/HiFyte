using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNoteDisplay : MonoBehaviour {
    private Note note;
    private AudioSource song;
    private AudioClip hitSound;
    private float beatLength = BeatManager.BeatLength(180);
    private bool played = false;

    public void createDisplay(Note note, AudioSource song, AudioClip hitSound) {
        this.note = note;
        this.song = song;
        this.hitSound = hitSound;
    }

    void Update() {
        float timeRemaining = (note.time - song.time) / beatLength;
        transform.position = new Vector3(-1.2f + (note.lane * 0.8f), 0, timeRemaining * 2);

        if (!played && song.time >= note.time * 1.5f) {
            played = true;
            gameObject.SetActive(false);
        }
    }

    public float getTime() {
        return note.time;
    }
}