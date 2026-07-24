using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Song Data/Set Stats")]
public class SongStats : ScriptableObject
{
    public int baseHealth;
    public int comboMultMax;
    [Range(1, 10)] public int difficulty;
}
