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
    private float scrollPos;

    private int previousDivision;

    AudioSource song;



    // Start is called before the first frame update
    void Start()
    {
        notes = _lane.getIntervalNotes();
        song = _beatManager.GetComponent<AudioSource>();
        song.clip = _audio;


        transform.position = _location;
        noteHeight = 1080f / (_builderData.getSubdivisions() * 2);

        previousDivision = _builderData.getSubdivisions();
        
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
            foreach (NoteDisplay nd in notes) {
                nd.destroyVisual();
            }

            notes = _lane.getIntervalNotes();

        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (ChartSingleton.baseBeat < totalBeats) {
                transform.position = new Vector2(transform.position.x, transform.position.y - noteHeight);

                _builderData.BeatAdd();
            }
            foreach (NoteDisplay nd in notes) {
                nd.destroyVisual();
            }

            notes = _lane.getIntervalNotes();
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
            foreach (NoteDisplay nd in notes) {
                nd.destroyVisual();
            }

            notes = _lane.getIntervalNotes();

        }


        if (Input.GetKeyDown(KeyCode.F)) {
            foreach (NoteDisplay nd in notes) {
                nd.destroyVisual();
            }

            notes = _lane.getIntervalNotes(-1, 533);


        }

        if (playing) {
            float totalBeats = _beatManager.getRawTime() / BeatManager.BeatLength(117f);
            float beatFraction = Mathf.Repeat(totalBeats, 1f);
            float yOffset = (1f - beatFraction) * 540f;
            transform.position = new Vector2(_location.x, _location.y + yOffset);
        } else {
            if (transform.position.y - _location.y * 2 >= 0) {
                transform.position = _location;
            }

            if (transform.position.y + _location.y * 2 <= 1080) {
                transform.position = _location;
            }
        }
    }

}
