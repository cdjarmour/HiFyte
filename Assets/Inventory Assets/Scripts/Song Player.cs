using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongPlayer : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;


    [SerializeField] private Image image;
    [SerializeField] private TMP_Text songName;
    [SerializeField] private TMP_Text songAuthor;

    [SerializeField] private Button play;

    private bool playing = false;

    private void Start() {
        play.onClick.AddListener(StartStopSong);
        Debug.Log("huh");
    }


    private void StartStopSong() {
        if (!playing) {
            if (_audioSource.time > 0) {
                _audioSource.UnPause();
            } else {
                _audioSource.Play();
            }
        } else {
            _audioSource.Pause();
        }
        playing = !playing;
    }

    public void UpdateData(Sprite sprite, ChartData data) {
        image.sprite = sprite;
        songName.text = data.name;
        songAuthor.text = data.composer;


        _audioSource.clip = Resources.Load<AudioClip>("ChartData/"  + data.audioFilePath);
    }
}
