using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeGetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GetComponent<SpriteRenderer>().size);        
    }

}
