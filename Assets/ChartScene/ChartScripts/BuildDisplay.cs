using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildDisplay : MonoBehaviour
{
    [SerializeField] private Vector2 _location;

    [SerializeField] private BeatManager _beatManager;
    [SerializeField] private ChartSingleton _builderData;
    [SerializeField] private Lane _lane;

    [SerializeField] private Sprite lane8;
    [SerializeField] private Sprite lane9;


    //temp
    private int totalBeats = 0;
    //

    private float noteHeight;
    private bool playing = false;
    private List<NoteDisplay> notes = new List<NoteDisplay>();

    int previousBeat = 0;

    AudioSource song;
    private AudioClip _audio;


    // Start is called before the first frame update
    void Start()
    {
        _audio = ChartSingleton.song;
        notes = _lane.getIntervalNotes();
        song = _beatManager.GetComponent<AudioSource>();
        song.clip = _audio;


        transform.position = _location;
        noteHeight = 1080f / (_builderData.getSubdivisions() * 2);
        totalBeats = Mathf.CeilToInt(_audio.length / BeatManager.BeatLength(ChartSingleton.bpm));
        Debug.Log("Total Beats:" + totalBeats);

        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P)) {
            if (!playing) {

                foreach (NoteDisplay nd in notes) {
                    nd.destroyVisual();
                }

                notes = _lane.getIntervalNotesScroll();
                int playStart = Mathf.FloorToInt(ChartSingleton.baseBeat);
                previousBeat = playStart - 1;

                _builderData.setBeat(playStart);


                song.time = previousBeat * BeatManager.BeatLength(ChartSingleton.bpm);

                song.Play();
                playing = true;
            } else {
                foreach (NoteDisplay nd in notes) {
                    nd.destroyVisual();
                }

                notes = _lane.getIntervalNotesScroll();
                song.Stop();
                playing = false;
                transform.position = new Vector2(transform.position.x, 540);



            }
        }

        if (!playing) {


                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll > 0f) {
                    if (ChartSingleton.baseBeat < totalBeats - 1) {
                        transform.position = new Vector2(transform.position.x, transform.position.y - noteHeight);
                        _builderData.BeatAdd();
                        updateDisplay();
                    }
                } else if (scroll < 0f) {
                    if (ChartSingleton.baseBeat > 1) {
                        transform.position = new Vector2(transform.position.x, transform.position.y + noteHeight);
                        _builderData.BeatRemove();
                        updateDisplay();
                    }
                }



            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                if (ChartSingleton.baseBeat > 1) {
                    transform.position = new Vector2(transform.position.x, transform.position.y + noteHeight);
                    _builderData.BeatRemove();
                }
                updateDisplay();

            }

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                if (ChartSingleton.baseBeat < totalBeats) {
                    transform.position = new Vector2(transform.position.x, transform.position.y - noteHeight);

                    _builderData.BeatAdd();
                }
                updateDisplay();
            }

            if (Input.GetKeyDown(KeyCode.Space)) {

                if (_builderData.getSubdivisions() == 8) {
                    this.GetComponent<Image>().sprite = lane9;
                    _builderData.setSubdivisions(9);
                } else {
                    this.GetComponent<Image>().sprite = lane8;
                    _builderData.setSubdivisions(8);
                }


                noteHeight = 1080f / (_builderData.getSubdivisions() * 2);
                float fixBeat = Mathf.Round((ChartSingleton.baseBeat - Mathf.FloorToInt(ChartSingleton.baseBeat)) / (1f / _builderData.getSubdivisions()));
                transform.position = new Vector2(transform.position.x, 540 - fixBeat * noteHeight);
                updateDisplay();
            }


            if (transform.position.y - _location.y * 2 >= 0) {
                transform.position = _location;
            }

            if (transform.position.y + _location.y * 2 <= 1080) {
                transform.position = _location;
            }


        } else {
            float beatsTraversed = _beatManager.getRawTime() / BeatManager.BeatLength(ChartSingleton.bpm);

            if (Mathf.FloorToInt(beatsTraversed) != previousBeat) {
                previousBeat = Mathf.FloorToInt(beatsTraversed);
                _builderData.setBeat(previousBeat + 1);
                foreach (NoteDisplay nd in notes) {
                    nd.destroyVisual();
                }

                notes = _lane.getIntervalNotesScroll();

                Debug.Log("test");
            }
            float beatFraction = Mathf.Repeat(beatsTraversed, 1f);
            float yOffset = (1f - beatFraction) * 540f;
            transform.position = new Vector2(_location.x, _location.y + yOffset);
            foreach (NoteDisplay nd in notes) {
                Transform root;
                if (nd.transform.parent != ChartSingleton.canvas.transform) {
                    root = nd.transform.parent;
                } else {
                    root = nd.transform;
                }
                root.position = new Vector2(root.position.x, nd.getY() + yOffset - 540f);
            }
        }
        
    }

    public void updateDisplay() {
        if (playing) { return; }
        foreach (NoteDisplay nd in notes) {
            nd.destroyVisual();
        }

        notes = _lane.getIntervalNotes();
    }

}
