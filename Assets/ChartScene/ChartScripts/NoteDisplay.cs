using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class NoteDisplay : MonoBehaviour
{
    private const float GRID_HEIGHT = 1080;
    private const float NOTE_WIDTH = 160;
    private const float NOTE_HEIGHT = 24;

    private Image image;
    private RectTransform rt;
    private float yPos;
    GameObject holdDisplay;

    public void Awake() {
        image = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
    }

    public void setNote(Note note) {
        float gridBaseBeat = ChartSingleton.instance.getBaseBeat();
        float gridSubdivisions = ChartSingleton.instance.getSubdivisions();
        float gridBPM = ChartSingleton.instance.getBPM();

        if (note.type == "Hold") image.sprite = Resources.Load<Sprite>("Notes/Normal");
        else image.sprite = Resources.Load<Sprite>("Notes/" + note.type);

        transform.SetParent(ChartSingleton.instance.getCanvas().transform, false);
        transform.SetSiblingIndex(1);
        rt.sizeDelta = new Vector2(NOTE_WIDTH, GRID_HEIGHT / NOTE_HEIGHT);
        rt.pivot = new Vector2(0.5f, 0f);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(0, 0);
        float x = note.lane * NOTE_WIDTH + (NOTE_WIDTH / 2f);
        float y;
        if (gridSubdivisions == note.subdivision) {
            y = Mathf.RoundToInt(((note.time / BeatManager.BeatLength(gridBPM))
            - gridBaseBeat + 1) * note.subdivision) * (GRID_HEIGHT / (note.subdivision * 2));
        } else {
            y = Mathf.RoundToInt((note.time / BeatManager.BeatLength(gridBPM)
            - Mathf.FloorToInt(gridBaseBeat) + 1) * note.subdivision) * (GRID_HEIGHT / (note.subdivision * 2));
            Debug.Log("BaseBeat: " + gridBaseBeat);
            Debug.Log("BASE: " + Mathf.FloorToInt(gridBaseBeat));
            y -= ((gridBaseBeat - Mathf.FloorToInt(gridBaseBeat)) * gridSubdivisions) * (1080f / (gridSubdivisions * 2));
        }
        rt.transform.position = new Vector2(x, y);
        yPos = y;

        if (note.type == "Hold") {
            holdDisplay = new GameObject("Hold Note");
            holdDisplay.AddComponent<Image>().sprite = Resources.Load<Sprite>("Notes/Hold");
            RectTransform hrt = holdDisplay.GetComponent<RectTransform>();
            hrt.transform.SetParent(ChartSingleton.instance.getCanvas().transform, false);
            hrt.transform.SetSiblingIndex(1);
            hrt.sizeDelta = new Vector2(NOTE_WIDTH, ((GRID_HEIGHT / (note.subdivision * 2)) * note.holdBeats) + ((GRID_HEIGHT / (note.subdivision * 2)) * (1 % (note.holdBeats + 1))));
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
