using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }

    protected void Awake() {
        
        if (Instance != null) Destroy(Instance);
        Instance = this;
    }
}
