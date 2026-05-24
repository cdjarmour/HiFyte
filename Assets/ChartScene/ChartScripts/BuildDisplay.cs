using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildDisplay : MonoBehaviour
{
    [SerializeField] private Vector2 _location;
    [SerializeField] private AudioClip _audio;
    [SerializeField] private BeatManager _beatManager;
    [SerializeField] private ChartSingleton _builderData;
    [SerializeField] private Lane _lane;

    [SerializeField] private Sprite lane8;
    [SerializeField] private Sprite lane9;


    //temp
    [SerializeField] private int totalBeats;
    //

    private float noteHeight;
    private bool playing = false;
    private List<NoteDisplay> notes = new List<NoteDisplay>();

    int previousBeat = 0;

    AudioSource song;



    // Start is called before the first frame update
    void Start()
    {
        notes = _lane.getIntervalNotes();
        song = _beatManager.GetComponent<AudioSource>();
        song.clip = _audio;


        transform.position = _location;
        noteHeight = 1080f / (_builderData.getSubdivisions() * 2);

        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R)) {  
            foreach (NoteDisplay nd in notes) nd.destroyVisual();
            notes = _lane.getIntervalNotes();
            Debug.Log(noteHeight);
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

        if (Input.GetKeyDown(KeyCode.P)) {
            foreach (NoteDisplay nd in notes) {
                nd.destroyVisual();
            }

            notes = _lane.getIntervalNotesScroll();

            song.Play();
            playing = true;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            /*
             * 
             *             song.Play();
             *             playing = true;
             *             
             */
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


        if (Input.GetKeyDown(KeyCode.F)) {
            updateDisplay();
        }

        if (playing) {
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
                nd.transform.position = new Vector2(nd.transform.position.x, nd.getY() + yOffset - 540f);
            }





        } else {
            if (transform.position.y - _location.y * 2 >= 0) {
                transform.position = _location;
            }

            if (transform.position.y + _location.y * 2 <= 1080) {
                transform.position = _location;
            }
        }
    }

    public void updateDisplay() {
        foreach (NoteDisplay nd in notes) {
            nd.destroyVisual();
        }

        notes = _lane.getIntervalNotes();
    }

}
