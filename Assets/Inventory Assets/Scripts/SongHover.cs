using UnityEngine;
using UnityEngine.EventSystems;

public class SongHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SongSelect songSelect;
    [SerializeField] private int index;

    private static int speed = 200;

    private bool hovering = false;
    private Vector2 hoverTarget;
    private Vector2 defaultTarget;

    private Vector2 target;

    void Awake() {
        hoverTarget = new Vector2(transform.position.x + 50, transform.position.y);
        defaultTarget = transform.position;

        target = defaultTarget;
    }


    public void OnPointerEnter(PointerEventData eventData) {
        target = hoverTarget;
        songSelect.setIndex(index);
    }

    public void OnPointerExit(PointerEventData eventData) {
        target = defaultTarget;
        songSelect.setIndex(-1);
    }

    private void Update() {
        transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
        

    }



}
