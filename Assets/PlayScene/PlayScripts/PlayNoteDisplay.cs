using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayNoteDisplay : MonoBehaviour {
    protected Note note;
    protected AudioSource song;
    protected AudioClip hitSound;
    protected float beatLength = BeatManager.BeatLength(173);
    protected bool played = false;

    protected NoteType notetype;
    public void createDisplay(Note note, AudioSource song, AudioClip hitSound) {
        this.note = note;
        this.song = song;
        this.hitSound = hitSound;
        notetype = note.type;
    }

    void Update() {
        float timeRemaining = (note.time - song.time) / beatLength;
        transform.position = new Vector3(-1.2f + (note.lane * 0.8f), 0, timeRemaining * VisualManager.SPEED);

        if (!played && song.time >= note.time + beatLength * 2) {
            played = true;
            gameObject.SetActive(false);
        }
    }

    public float getTime() {
        return note.time;
    }
}

public class PlayHoldNoteDisplay : PlayNoteDisplay {
    protected bool frontPressed = false;
    protected float nextBeat = 0;
    private int count = 0;
    private bool passed = false;


    void Start() {
        float displaySize = (note.holdBeats + 1) / ( (float) note.subdivision / VisualManager.SPEED);
        Transform noteSprite = transform.Find("NoteSprite");
        noteSprite.localScale = new Vector3(displaySize, noteSprite.localScale.y, noteSprite.localScale.z);
        noteSprite.localPosition = new Vector3(noteSprite.localPosition.x, noteSprite.localPosition.y, displaySize / 2f);

        count = 0;
        nextBeat = note.time + beatLength / Mathf.Floor(note.subdivision / 2f);

    }


    void Update() {
        float timeRemaining = (note.time - song.time) / beatLength;
        transform.position = new Vector3(-1.2f + (note.lane * 0.8f), 0, timeRemaining * VisualManager.SPEED);

        if (song.time >= nextBeat && count < Mathf.Floor(note.holdBeats / 2)) {
            passed = true;
            nextBeat = nextBeat + beatLength / Mathf.Floor(note.subdivision / 2f); ;
            count++;
        } else passed = false;



        if (frontPressed) return;

    }


    public void pressFront() {
        frontPressed = true;
    }

    public bool getPressed() {
        return frontPressed;
    }

    public bool nextInterval() {
        return passed;
    }

}