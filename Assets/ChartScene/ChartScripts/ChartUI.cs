using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.UI;

public class ChartUI : MonoBehaviour
{

    [Header("Chart Builder")] 
    [SerializeField] private NoteInput _input;
    [SerializeField] private BuildDisplay _buildDisplay;
    [SerializeField] private Button save;
    [SerializeField] private Button exit;
    [Header("Note Types")]
    [SerializeField] private Button placeNormal;
    [SerializeField] private Button placeTap;
    [SerializeField] private Button placeRelease;
    [SerializeField] private Button placeHold;
    [Header("New Chart")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField composerInput;
    [SerializeField] private TMP_InputField bpmInput;
    [SerializeField] private TMP_InputField sampleInput;
    [SerializeField] private Button createChart;
    [Header("Data Display")]
    [SerializeField] private Image imageDisplay;
    [SerializeField] private TMP_Text dataDisplay;
    [SerializeField] private AudioSource _audioSource;

    private List<string> chartNames = new List<string>();
    private int index = 0;
    ChartData metaData;
    private bool building = false;
    private float start;


    // Start is called before the first frame update
    void Start()
    {
        TextAsset[] chartFiles = Resources.LoadAll<TextAsset>("ChartData/MetaData");
        foreach (TextAsset t in chartFiles) {
            chartNames.Add(t.name);
        }



        placeNormal.onClick.AddListener(delegate {
            ChartSingleton.instance.setState("Normal");
        });

        placeTap.onClick.AddListener(delegate {
            ChartSingleton.instance.setState("Tap");
        });

        placeHold.onClick.AddListener(delegate {
            ChartSingleton.instance.setState("Hold");
        });

        save.onClick.AddListener(delegate {
            ChartJSON.convertNotes(metaData, _input.getNotes());
            UnityEditor.AssetDatabase.Refresh();
        });

        exit.onClick.AddListener(delegate {
            _input.enabled = false;
            _buildDisplay.enabled = false;
            ChartSingleton.instance.resetData();
            save.gameObject.SetActive(false);
            exit.gameObject.SetActive(false);
            placeNormal.gameObject.SetActive(false);
            placeTap.gameObject.SetActive(false);
            placeRelease.gameObject.SetActive(false);
            placeHold.gameObject.SetActive(false);
            nameInput.gameObject.SetActive(true);
            composerInput.gameObject.SetActive(true);
            bpmInput.gameObject.SetActive(true);
            sampleInput.gameObject.SetActive(true);
            createChart.gameObject.SetActive(true);
            imageDisplay.gameObject.SetActive(true);
            dataDisplay.gameObject.SetActive(true);

            building = false;
            _audioSource.time = start;
            _audioSource.Play();
        });


        createChart.onClick.AddListener(delegate {
            if (ChartJSON.hasChart(nameInput.text)) { return; }
            ChartJSON.createChart(nameInput.text, composerInput.text, int.Parse(bpmInput.text), float.Parse(sampleInput.text));
            Debug.Log("bruh");
        });

        metaData = ChartJSON.getMetaData(chartNames[index]);
        imageDisplay.sprite = Resources.Load<Sprite>("ChartData/" + metaData.imageFilePath);
        dataDisplay.text = $"Name: {metaData.name}\n" +
                           $"Composer: {metaData.composer}\n" +
                           $"BPM: {metaData.bpm}";
        start = metaData.samplePos;
        _audioSource.clip = Resources.Load<AudioClip>("ChartData/" + metaData.audioFilePath);
        _audioSource.time = start;
        _audioSource.Play();
    }


    private void Update() {
        if (building) return;

        if (_audioSource.time > start + 30) {
            _audioSource.time = start;
        }




        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            index = (index - 1 + chartNames.Count) % chartNames.Count;
            metaData = ChartJSON.getMetaData(chartNames[index]);
            imageDisplay.sprite = Resources.Load<Sprite>("ChartData/" + metaData.imageFilePath);
            dataDisplay.text = $"Name: {metaData.name}\n" +
                               $"Composer: {metaData.composer}\n" +
                               $"BPM: {metaData.bpm}";
            start = metaData.samplePos;
            _audioSource.clip = Resources.Load<AudioClip>("ChartData/" + metaData.audioFilePath);
            _audioSource.time = start;
            _audioSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            index = (index + 1) % chartNames.Count;
            metaData = ChartJSON.getMetaData(chartNames[index]);
            imageDisplay.sprite = Resources.Load<Sprite>("ChartData/" + metaData.imageFilePath);
            dataDisplay.text = $"Name: {metaData.name}\n" +
                               $"Composer: {metaData.composer}\n" +
                               $"BPM: {metaData.bpm}";
            start = metaData.samplePos;
            _audioSource.clip = Resources.Load<AudioClip>("ChartData/" + metaData.audioFilePath);
            _audioSource.time = start;
            _audioSource.Play();
        }


        if (Input.GetKeyDown(KeyCode.Return)) {
            building = true;
            ChartSingleton.instance.setBPM(metaData.bpm);
            _input.setLanes(ChartJSON.getNotes(metaData));
            Debug.Log(ChartSingleton.instance.getBPM());

            nameInput.gameObject.SetActive(false);
            composerInput.gameObject.SetActive(false);
            bpmInput.gameObject.SetActive(false);
            sampleInput.gameObject.SetActive(false);
            createChart.gameObject.SetActive(false);
            imageDisplay.gameObject.SetActive(false);
            dataDisplay.gameObject.SetActive(false);
            _audioSource.time = 0;
            _audioSource.Stop();
            _input.enabled = true;
            _buildDisplay.enabled = true;
            save.gameObject.SetActive(true);
            exit.gameObject.SetActive(true);
            placeNormal.gameObject.SetActive(true);
            placeTap.gameObject.SetActive(true);
            placeRelease.gameObject.SetActive(true);
            placeHold.gameObject.SetActive(true);

        }
    }


}
