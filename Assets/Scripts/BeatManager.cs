using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    [SerializeField] float _bpm;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] Intervals[] _intervals;

    private int currBeat;
    private float rawTime;

    void Update()
    {
        if (_audioSource == null || _audioSource.clip == null) return;

        foreach (Intervals interval in _intervals) {
            float sampledTime = (_audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetIntervalLength(_bpm)));
            interval.checkForNextInterval(sampledTime);
        }
        currBeat = _intervals[0].checkForNextInterval(_audioSource.timeSamples / (_audioSource.clip.frequency * _intervals[0].GetIntervalLength(_bpm)));

        rawTime = (float) _audioSource.timeSamples / (float) _audioSource.clip.frequency;
    }


    public static float BeatLength(float bpm) {
        return Intervals.GetIntervalLength(bpm, 1);
    }

    public int getCurrBeat() {
        return currBeat;
    }


    public float getRawTime()
    {
        return rawTime;
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] float _subdivision;
    [SerializeField] UnityEvent _event;
    [SerializeField] float _beatoffset;
    private int _lastInterval;
    private int curr;

    public static float GetIntervalLength(float bpm, float subdivision) {
        return 60f / (bpm * subdivision);
    }

    public float GetIntervalLength(float bpm)
    {
        return GetIntervalLength(bpm, _subdivision);
    }


    public int checkForNextInterval(float interval)
    {
        curr = Mathf.FloorToInt(interval + _beatoffset * _subdivision);
        if (curr != _lastInterval)
        {
            _lastInterval = curr;
            _event.Invoke();
        }

        return curr;
    }
}