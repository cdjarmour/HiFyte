using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChartUI : MonoBehaviour
{
    [SerializeField] private ChartSingleton _builderData;
    [SerializeField] private Button placeNormal;
    [SerializeField] private Button placeTap;
    [SerializeField] private Button placeRelease;
    [SerializeField] private Button placeHold;
    [SerializeField] private Button deleteNote;



    // Start is called before the first frame update
    void Start()
    {

        placeNormal.onClick.AddListener(delegate {
            ChartSingleton.buildState = "Normal";
        });

        placeTap.onClick.AddListener(delegate {
            ChartSingleton.buildState = "Tap";
        });

        placeHold.onClick.AddListener(delegate {
            ChartSingleton.buildState = "Hold";
        });
    }



}
