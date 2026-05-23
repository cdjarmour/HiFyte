using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChartUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown subdivisions;
    [SerializeField] private TMP_InputField bpm;
    [SerializeField] private TMP_InputField name;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private AudioClip audioClip;

    private int subdivision;
    AudioSource song;
    AudioClip audioPath = null;

    // Start is called before the first frame update
    void Start()
    {
        subdivisions.onValueChanged.AddListener(delegate {
            onDropDownChanged(subdivisions);
        });


        loadButton.onClick.AddListener(delegate {
            if (ChartJSON.hasChart(name.text)) {
                Debug.Log("true");
            } else {
                Debug.Log("False");
            }
        });

        saveButton.onClick.AddListener(delegate {
            Debug.Log("good job bro bpm is " + bpm.text);
        });


        //song = GetComponent<AudioSource>();
        //song.clip = audioPath;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void onDropDownChanged(TMP_Dropdown dropdown) {
        subdivision = dropdown.value + 1;
    }
}
