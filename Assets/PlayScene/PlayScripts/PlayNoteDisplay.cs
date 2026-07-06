using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayNoteDisplay : MonoBehaviour {
    protected Note note;
    protected float beatLength;
    protected bool played = false;
    protected NoteType notetype;
    protected float songTime;

    void Awake() {
        beatLength = Battle.BeatManager.instance.getBeatLength();
    }

    public void createDisplay(Note note) {
        this.note = note;
        notetype = note.type;
    }

    void Update() {
        UpdatePosition();
        CheckExpired(note.time + beatLength * 2);
    }

    protected void UpdatePosition() {
        songTime = Battle.BeatManager.instance.getTime();
        float timeRemaining = (note.time - songTime) / beatLength;
        transform.position = new Vector3(-1.2f + (note.lane * 0.8f), 0, timeRemaining * BattleData.instance.getSpeed());
    }

    protected void CheckExpired(float expireTime) {
        if (!played && songTime >= expireTime) {
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

    private float holdDuration;      // ((note.holdBeats + 1) / subdivision) * beatLength
    private float intervalLength;    // beatLength / floor(subdivision / 2)
    private int maxIntervalCount;    // floor(holdBeats / 2)

    void Start() {
        holdDuration = ((note.holdBeats + 1) / (float)note.subdivision) * beatLength;
        intervalLength = beatLength / Mathf.Floor(note.subdivision / 2f);
        maxIntervalCount = (int)Mathf.Floor(note.holdBeats / 2f);

        float displaySize = (note.holdBeats + 1) / ((float)note.subdivision / BattleData.instance.getSpeed());
        Transform noteSprite = transform.Find("NoteSprite");
        noteSprite.localScale = new Vector3(displaySize, noteSprite.localScale.y, noteSprite.localScale.z);
        noteSprite.localPosition = new Vector3(noteSprite.localPosition.x, noteSprite.localPosition.y, displaySize / 2f);

        count = 0;
        nextBeat = note.time + intervalLength;
    }

    void Update() {
        UpdatePosition();

        if (songTime >= nextBeat && count < maxIntervalCount) {
            passed = true;
            nextBeat += intervalLength;
            count++;
        } else {
            passed = false;
        }

        if (frontPressed) return;

        CheckExpired(note.time + holdDuration + beatLength * 2);
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