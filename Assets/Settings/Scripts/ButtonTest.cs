using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    [SerializeField] private Sprite pressImage;
    private Sprite baseImage;

    private Image icon;

    // Start is called before the first frame update
    public void Start()
    {
        baseImage = GetComponent<Image>().sprite;
        icon = GetComponent<Image>();
        icon.sprite = baseImage;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            icon.sprite = pressImage;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            icon.sprite = baseImage;
        }
    }
}


