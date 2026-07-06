using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisualManager : MonoBehaviour {
    [SerializeField] private AudioSource _song;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject noteParent;
    [SerializeField] private AudioClip hitSound;

    #region Note Storage
    private List<Note>[] chart;
    private List<GameObject>[] chartDisplay = new List<GameObject>[4];
    private List<PlayNoteDisplay>[] displayComponents = new List<PlayNoteDisplay>[4];
    private int[] printIndex = new int[4];
    private int[] inputIndex = new int[4];
    #endregion Note Storage

    private const int ACCURACY_WINDOW = 6;

    private float hitWindow;
    private float beatLength;
    private float songTime;
    private float endPos;
    private float startPos;

    private BattleState state;
    private BattleState previousState;

    void Start() {
        chart = ChartJSON.getSortedNotes(BattleData.instance.getName());
        createNoteObjects();
        beatLength = Battle.BeatManager.instance.getBeatLength();
        hitWindow = beatLength / ACCURACY_WINDOW;
        BattleManager.OnBattleStateChange += OnStateUpdate;
        Battle.BeatManager.OnCountDownEnd += OnAttackStart;
        Battle.BeatManager.OnSongLoop += ResetIndecies;
    }

    void OnDisable() {
        BattleManager.OnBattleStateChange -= OnStateUpdate;
    }

    public void OnStateUpdate(BattleState newState) {
        previousState = state;
        if (newState != BattleState.PlayerAttack) {
            state = newState;
        }
    }

    public void ResetIndecies() {
        for (int i = 0; i < inputIndex.Length; i++) {
            inputIndex[i] = 0;
            printIndex[i] = 0;
        }
    }

    public void OnAttackStart() {
        state = BattleState.PlayerAttack;
        if (state == BattleState.PlayerAttack && previousState != BattleState.PlayerAttack) {
            Debug.Log("what");
            for (int i = 0; i < inputIndex.Length; i++) {
                inputIndex[i] = printIndex[i];
            }
            endPos = Battle.BeatManager.instance.getEndPos();
            startPos = Battle.BeatManager.instance.getStartPos();
        }
    }
    


    void Update() {
        songTime = Battle.BeatManager.instance.getTime();

        for (int i = 0; i < chartDisplay.Length; i++) {
            if (inputIndex[i] < chart[i].Count) {
                Note frontNote = chart[i][inputIndex[i]];
                if (frontNote.type == NoteType.Normal && songTime >= frontNote.time + hitWindow) {
                    if (state == BattleState.PlayerAttack) BattleData.instance.setCombo(0);
                    inputIndex[i]++;
                } else if (frontNote.type == NoteType.Hold && !chartDisplay[i][inputIndex[i]].GetComponent<PlayHoldNoteDisplay>().getPressed()
                    && songTime >= frontNote.time + hitWindow) {
                    if (state == BattleState.PlayerAttack) BattleData.instance.setCombo(0);
                    inputIndex[i]++;
                }
            }
            while (printIndex[i] < chartDisplay[i].Count) {
                float displayTime = displayComponents[i][printIndex[i]].getTime();
                if (state == BattleState.PlayerAttack &&
                    displayTime <= songTime + beatLength * (10f / BattleData.instance.getSpeed())
                    && displayTime <= endPos - hitWindow) {
                    if (startPos <= displayTime) chartDisplay[i][printIndex[i]].SetActive(true);
                    printIndex[i]++;
                } else {
                    break; // <-- required, or the loop never terminates on a false condition
                }
            }
        }

        if (state != BattleState.PlayerAttack) return;

        KeyInput(KeyCode.A, 0);
        KeyInput(KeyCode.S, 1);
        KeyInput(KeyCode.K, 2);
        KeyInput(KeyCode.L, 3);
    }

    private void KeyInput(KeyCode key, int lane) {
        if (inputIndex[lane] >= chart[lane].Count) return;
        Note currNote = chart[lane][inputIndex[lane]];

        if (currNote.type == NoteType.Normal) {
            if (Input.GetKeyDown(key)) {
                if (songTime <= currNote.time + hitWindow
                && songTime >= currNote.time - hitWindow) {
                    chartDisplay[lane][inputIndex[lane]].SetActive(false);
                    inputIndex[lane]++;
                    _song.PlayOneShot(hitSound);
                    BattleData.instance.setCombo(BattleData.instance.getCombo() + 1);
                } else {
                    BattleData.instance.setCombo(0);
                }
            }
        } else if (currNote.type == NoteType.Hold) {
            PlayHoldNoteDisplay currDisplay = (PlayHoldNoteDisplay)displayComponents[lane][inputIndex[lane]];

            if (currDisplay.getPressed()) {
                if (Input.GetKey(key)) {
                    if (currDisplay.nextInterval() && songTime <= currNote.time + ((currNote.holdBeats + 1) / (float)currNote.subdivision) * beatLength) {
                        BattleData.instance.setCombo(BattleData.instance.getCombo() + 1);
                    } else if (songTime >= currNote.time + ((currNote.holdBeats + 1) / (float)currNote.subdivision) * beatLength) {
                        inputIndex[lane]++;
                    }
                } else {
                    BattleData.instance.setCombo(0);
                    inputIndex[lane]++;
                }
                return;
            }

            if (Input.GetKeyDown(key)) {
                if (songTime <= currNote.time + hitWindow
                && songTime >= currNote.time - hitWindow && !currDisplay.getPressed()) {
                    currDisplay.pressFront();
                    BattleData.instance.setCombo(BattleData.instance.getCombo() + 1);
                    _song.PlayOneShot(hitSound);
                }
            }
        }
    }

    public void createNoteObjects() {
        for (int i = 0; i < chart.Length; i++) {
            printIndex[i] = 0;
            inputIndex[i] = 0;
            chartDisplay[i] = new List<GameObject>();
            displayComponents[i] = new List<PlayNoteDisplay>();
            foreach (Note note in chart[i]) {
                PlayNoteDisplay display = null;
                GameObject noteObject = Instantiate(notePrefab);
                switch (note.type) {
                    case NoteType.Normal:
                    display = noteObject.AddComponent<PlayNoteDisplay>();
                    break;

                    case NoteType.Hold:
                    display = noteObject.AddComponent<PlayHoldNoteDisplay>();
                    break;
                }
                noteObject.SetActive(false);
                noteObject.transform.SetParent(noteParent.transform, true);
                noteObject.GetComponent<PlayNoteDisplay>().createDisplay(note);
                chartDisplay[i].Add(noteObject);
                displayComponents[i].Add(display);
            }
        }
    }

}