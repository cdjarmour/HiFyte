using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/Create Move")]
public class AttackAction : ScriptableObject
{
    public string movename;
    public List<StatusEffect> statusEffects =  new List<StatusEffect>();
    public int damage;
    public int baseWeight;
    public AttackType attackType;
}

public enum StatusEffect {

    //***GLOBAL STATUSES**//

    Nothing,
    OffBeat,


    //*** PLAYER STATUSES**//

    //Buff
    Quantized,
    Serenade,
    Retrograde,

    //Debuff

    //*** ENEMY STATUSES ***//

    //Buff

    //Debuff
    Deafen,
    Overclocked,

}


public enum AttackType {
    //Attack Type
    Melodic,
    //Healing, Attack Buffs
    Harmonic,
    //Changes Player Inputs
    Rhythmic,
    //Audio Alteration
    Dynamic
}