using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class NoteDisplay : MonoBehaviour
{
    private Image image;
    private RectTransform rt;
    private Note note;

    public void Awake() {
        image = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
    }

    public void setNote(Note note) {
        this.note = note;
        image.sprite = Resources.Load<Sprite>("Notes/Tap");
        transform.SetParent(ChartSingleton.canvas.transform, false);
        rt.sizeDelta = new Vector2(160f, 1080f / (24));
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(0, 0);
        float x = note.lane * 160 + 80;
        float y = (Mathf.RoundToInt(((note.time / BeatManager.BeatLength(ChartSingleton.bpm)) 
            - ChartSingleton.baseBeat + 1) * note.subdivision) * (1080f / (note.subdivision * 2))) + (1080f / (note.subdivision * 8f));
        rt.transform.position = new Vector2(x, y);

    }

    public void destroyVisual() {

        Object.Destroy(this.gameObject);

    }


}
