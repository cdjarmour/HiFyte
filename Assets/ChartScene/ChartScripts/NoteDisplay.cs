using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class NoteDisplay : MonoBehaviour
{
    private Image image;
    private RectTransform rt;
    private Note note;
    private float yPos;
    GameObject holdDisplay;

    public void Awake() {
        image = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
    }

    public void setNote(Note note) {
        this.note = note;

        if (note.type == "Hold") image.sprite = Resources.Load<Sprite>("Notes/Normal");
        else image.sprite = Resources.Load<Sprite>("Notes/" + note.type);

        transform.SetParent(ChartSingleton.canvas.transform, false);
        transform.SetSiblingIndex(1);
        rt.sizeDelta = new Vector2(160f, 1080f / (24));
        rt.pivot = new Vector2(0.5f, 0f);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(0, 0);
        float x = note.lane * 160 + 80;
        float y;
        if (ChartSingleton.subdivisions == note.subdivision) {
            y = (Mathf.RoundToInt(((note.time / BeatManager.BeatLength(ChartSingleton.bpm))
            - ChartSingleton.baseBeat + 1) * note.subdivision) * (1080f / (note.subdivision * 2)));
        } else {
            y = (Mathf.RoundToInt(((note.time / BeatManager.BeatLength(ChartSingleton.bpm))
            - Mathf.FloorToInt(ChartSingleton.baseBeat) + 1) * note.subdivision) * (1080f / (note.subdivision * 2)));
            Debug.Log("BaseBeat: " + ChartSingleton.baseBeat);
            Debug.Log("BASE: " + Mathf.FloorToInt(ChartSingleton.baseBeat));
            y -= ((ChartSingleton.baseBeat - Mathf.FloorToInt(ChartSingleton.baseBeat)) * ChartSingleton.subdivisions) * (1080f / (ChartSingleton.subdivisions * 2));
        }
        rt.transform.position = new Vector2(x, y);
        yPos = y;

        if (note.type == "Hold") {
            holdDisplay = new GameObject("Hold Note");
            holdDisplay.AddComponent<Image>().sprite = Resources.Load<Sprite>("Notes/Hold");
            RectTransform hrt = holdDisplay.GetComponent<RectTransform>();
            hrt.transform.SetParent(ChartSingleton.canvas.transform, false);
            hrt.transform.SetSiblingIndex(1);
            hrt.sizeDelta = new Vector2(160f, ((1080f / (note.subdivision * 2)) * note.holdBeats) + ((1080f / (note.subdivision * 2)) * (1 % (note.holdBeats + 1))));
            hrt.pivot = new Vector2(0.5f, 0f);
            hrt.anchorMin = new Vector2(0, 0);
            hrt.anchorMax = new Vector2(0, 0);
            rt.pivot = new Vector2(0, 0f);
            rt.transform.position = new Vector2(0, 0);
            hrt.transform.position = new Vector2(x, y);
            transform.SetParent(holdDisplay.transform, false);
        }



    }

    public void destroyVisual() {

        if (holdDisplay != null) {
            Object.Destroy(holdDisplay.gameObject);
        } else Object.Destroy(this.gameObject);

    }

    public float getY() {
        return yPos;
    }


}
