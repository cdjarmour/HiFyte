using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveData  : MonoBehaviour
{
    public static SaveData instance { get; private set; }

    public List<SpecialInstrument> Instruments;
    public List<string> SongNames;

    private void Awake() {
        instance = this;
    }
}
