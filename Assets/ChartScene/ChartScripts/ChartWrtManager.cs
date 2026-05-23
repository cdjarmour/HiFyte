using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;



namespace ChartWriter {


    [System.Serializable]
    public class ChartArray {
        public string songName;
        public float bpm;
        public float duration;
        public List<NoteObject> notes;

        public ChartArray(string songName, float bpm, float duration) {
            this.songName = songName;
            this.bpm = bpm;
            this.duration = duration;
        }

        public void addNote(NoteObject note) {
            notes.Add(note);
        }
    }

    [System.Serializable]
    public class NoteObject {
        public int beat;
        public bool hold;
        public int holdEnd;
        public bool trace;

        public NoteObject(int beat, bool hold, int holdEnd, bool trace) {
            this.beat = beat;
            this.hold = hold;
            this.holdEnd = holdEnd;
            this.trace = trace;
        }
    }

}
