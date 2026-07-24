using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Ability : ScriptableObject
{
    public string AbilityName;
    public MusicalAttribute MusicalAttribute;
    public int SpeedChange;

    [TextArea] public string AbilityDescription;

    public float ModifySpeed(float speed) { return speed + SpeedChange; }
    public virtual void ModifyDamage(ref float multiplier) { }


}

[Serializable]
[CreateAssetMenu(menuName = "Abilities/Overdrive")]
public class Overdrive : Ability {
    public float HealthThreshold;
    public float DamageMult;

}

[Serializable]
[CreateAssetMenu(menuName = "Abilities/Fermata")]
public class Fermata : Ability {
    public float FeverRate;
}

public enum MusicalAttribute {
    Rock,
    Jazz,
    Ballad
}