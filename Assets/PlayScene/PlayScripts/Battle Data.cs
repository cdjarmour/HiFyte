using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    public static BattleData instance { get; private set; }

    //Temp placeholder will be obtained from player and a startup script
    private string song = "Catch me if you can";
    private ChartData chartData;


    private int bpm;
    private string songName;

    private int speed;
    private int combo;

    private void Awake() {
        instance = this;
        chartData = ChartJSON.getMetaData(song);
        bpm = chartData.bpm;
        songName = chartData.name;
        combo = 0;
        speed = 3;
    }


    public int getBPM() {
        return bpm;
    }

    public int getSpeed() {
        return speed;
    }

    public int getCombo() {
        return combo;
    }

    public string getName() {
        return songName;
    }

    public ChartData getChartData() {
        return chartData;
    }

    public void setSpeed(int s) {
        speed = s;
    }

    public void setCombo(int c) {
        combo = c;
    }


}
