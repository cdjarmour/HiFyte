using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioStarter : MonoBehaviour
{

    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _startTime;

    private bool _isPlaying;
    private bool loaded;
    AudioSource song;
    //Test
    AudioClip audioPath = null;

    // Update is called once per frame
    private void Start()
    {
        //song = GetComponent<AudioSource>();
        //song.clip = _audioClip;
        //song.time = _startTime;
        _isPlaying = false;
        loaded = true;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!loaded) {
                if (!_isPlaying) {
                    song.Play();
                    _isPlaying = true;
                } else if (_isPlaying) {
                    song.Pause();
                    _isPlaying = false;
                }

            } else if (loaded) {
                Chart chart = new Chart();
                ChartData cd = new ChartData("Focus", 165f, 198f, "Artcore V3");
                Note note1 = new Note(1, 2, 1);
                chart.metaData = cd;
                chart.notes.Add(note1);
                ChartJSON.ConvertToJSON(chart);
                Debug.Log("2");

                audioPath = Resources.Load<AudioClip>(ChartJSON.ConvertToChart("Focus").metaData.audioFilePath);
                Debug.Log("2");
                song = GetComponent<AudioSource>();
                song.clip = audioPath;
                song.time = _startTime;
                loaded = false;

            }
        }
    }
}
