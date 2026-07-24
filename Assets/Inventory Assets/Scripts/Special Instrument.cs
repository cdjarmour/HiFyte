using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Instrument/Create Instrument")]
public class SpecialInstrument : ScriptableObject
{
    public string InstrumentName;
    public Sprite InstrumentSprite;
    public int BaseSpeed;
    [Range(1,10)] public int HandlingDifficulty;

    [TextArea] public string Description;

    public Ability Ability;

    public int DecisionWindow;
    public int AttackWindow;
    public int HealingBonus;
    public int DamageBonus;
    public int AccuracyWindowBonus;

}
