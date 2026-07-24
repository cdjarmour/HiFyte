using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class SongSelect : MonoBehaviour
{
    [SerializeField] List<GameObject> SelectButtons;
    [SerializeField] GameObject DataDisplay;
    [SerializeField] SongPlayer player;


    public List<ChartData> SongData;
    public List<Sprite> SongCovers;
    public List<SongStats> SongStats;

    private int currIndex = -1;

    private int currInstrument = 1;

    private void Start() {
        int count = 0;
        List<string> names = SaveData.instance.SongNames;
        foreach (string name in names) {
            SongData.Add(ChartJSON.getMetaData(name));
            SongCovers.Add(Resources.Load<Sprite>("ChartData/" + SongData[count].imageFilePath));
            SongStats.Add(Resources.Load<SongStats>("ChartData/BattleStats/" + name));

            SelectButtons[count].transform.GetChild(0).GetComponent<Image>().sprite = SongCovers[count];
            SelectButtons[count].transform.GetChild(1).GetComponent<TMP_Text>().text = name;
            SelectButtons[count].transform.GetChild(2).GetComponent<TMP_Text>().text = "Difficulty: " + SongStats[count].difficulty;
            SelectButtons[count].transform.GetChild(3).GetComponent<TMP_Text>().text = "Health: " + SongStats[count].baseHealth;
            SelectButtons[count].transform.GetChild(4).GetComponent<TMP_Text>().text = "x0." + SongStats[count].comboMultMax;
            SelectButtons[count].transform.GetChild(5).GetComponent<TMP_Text>().text = "BPM: " + SongData[count].bpm;
            count++;
        }
        player.UpdateData(SongCovers[currInstrument], SongData[currInstrument]);
    }

    private void Update() {
        if (currIndex == -1) return;

        if (Input.GetMouseButtonDown(0)) {
            currInstrument = currIndex;
            player.UpdateData(SongCovers[currIndex], SongData[currIndex]);
        }
    }


    public void setIndex(int index) {
        currIndex = index;
    }


}
