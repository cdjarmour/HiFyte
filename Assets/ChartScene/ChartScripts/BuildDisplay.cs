using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildDisplay : MonoBehaviour
{
    [SerializeField] private Vector2 _location;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private NoteInput _input;

    [SerializeField] private Sprite lane8;
    [SerializeField] private Sprite lane9;


    //temp
    private int totalBeats = 0;
    //

    private float noteHeight;
    private bool playing = false;
    private List<NoteDisplay> notes = new List<NoteDisplay>();

    int previousBeat = 0;



    // Start is called before the first frame update
    void OnEnable()
    {



        transform.position = _location;
        noteHeight = 1080f / (ChartSingleton.instance.getSubdivisions() * 2);
        totalBeats = Mathf.CeilToInt(_audioSource.clip.length / BeatManager.BeatLength(ChartSingleton.instance.getBPM()));
        Debug.Log("Total Beats:" + totalBeats);
        updateDisplay();

        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P)) {
            if (!playing) {

                foreach (NoteDisplay nd in notes) {
                    nd.destroyVisual();
                }

                notes = _input.getIntervalNotes(2);
                int playStart = Mathf.FloorToInt(ChartSingleton.instance.getBaseBeat());
                previousBeat = playStart - 1;

                ChartSingleton.instance.setBaseBeat(playStart);
                _audioSource.time = previousBeat * BeatManager.BeatLength(ChartSingleton.instance.getBPM());

                _audioSource.Play();
                playing = true;
            } else {
                foreach (NoteDisplay nd in notes) {
                    nd.destroyVisual();
                }

                notes = _input.getIntervalNotes(1);
                _audioSource.Stop();
                playing = false;
                transform.position = new Vector2(transform.position.x, 540);



            }
        }

        if (!playing) {


                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll > 0f) {
                    if (ChartSingleton.instance.getBaseBeat() < totalBeats - 1) {
                        transform.position = new Vector2(transform.position.x, transform.position.y - noteHeight);
                        ChartSingleton.instance.BeatAdd();
                        updateDisplay();
                    }
                } else if (scroll < 0f) {
                    if (ChartSingleton.instance.getBaseBeat() > 1) {
                        transform.position = new Vector2(transform.position.x, transform.position.y + noteHeight);
                        ChartSingleton.instance.BeatRemove();
                        updateDisplay();
                    }
                }



            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                if (ChartSingleton.instance.getBaseBeat() > 1) {
                    transform.position = new Vector2(transform.position.x, transform.position.y + noteHeight);
                    ChartSingleton.instance.BeatRemove();
                }
                updateDisplay();

            }

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                if (ChartSingleton.instance.getBaseBeat() < totalBeats - 1) {
                    transform.position = new Vector2(transform.position.x, transform.position.y - noteHeight);

                    ChartSingleton.instance.BeatAdd();
                }
                updateDisplay();
            }

            if (Input.GetKeyDown(KeyCode.Space)) {

                if (ChartSingleton.instance.getSubdivisions() == 8) {
                    this.GetComponent<Image>().sprite = lane9;
                    ChartSingleton.instance.setSubdivisions(9);
                } else {
                    this.GetComponent<Image>().sprite = lane8;
                    ChartSingleton.instance.setSubdivisions(8);
                }


                noteHeight = 1080f / (ChartSingleton.instance.getSubdivisions() * 2);
                float fixBeat = Mathf.Round((ChartSingleton.instance.getBaseBeat() - Mathf.FloorToInt(ChartSingleton.instance.getBaseBeat())) / (1f / ChartSingleton.instance.getSubdivisions()));
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
            float beatsTraversed = _audioSource.time / BeatManager.BeatLength(ChartSingleton.instance.getBPM());

            if (Mathf.FloorToInt(beatsTraversed) != previousBeat) {
                previousBeat = Mathf.FloorToInt(beatsTraversed);
                ChartSingleton.instance.setBaseBeat(previousBeat + 1);
                foreach (NoteDisplay nd in notes) {
                    nd.destroyVisual();
                }

                notes = _input.getIntervalNotes(2);

                Debug.Log("test");
            }
            float beatFraction = Mathf.Repeat(beatsTraversed, 1f);
            float yOffset = (1f - beatFraction) * 540f;
            transform.position = new Vector2(_location.x, _location.y + yOffset);
            foreach (NoteDisplay nd in notes) {
                Transform root;
                if (nd.transform.parent != ChartSingleton.instance.getCanvas().transform) {
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

        notes = _input.getIntervalNotes(1);
    }

    private void OnDisable() {
        foreach (NoteDisplay nd in notes) {
            nd.destroyVisual();
        }
        notes.Clear();
        transform.position = _location;
    }

}
