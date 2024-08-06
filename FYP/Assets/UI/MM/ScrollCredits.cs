using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollCredits : MonoBehaviour, IDragHandler, IEndDragHandler {
    public float scrollSpeed = 30f;
    public float stopPositionY = 100f;

    private RectTransform rectTransform;
    private float initialPositionY;
    private bool isDragging = false;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        initialPositionY = rectTransform.anchoredPosition.y;
    }

    void Update() {
        if (!isDragging) {
            float newY = rectTransform.anchoredPosition.y + scrollSpeed * Time.deltaTime;

            if (newY >= stopPositionY) {
                newY = stopPositionY;
                enabled = false;
            }

            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
        }
    }

    public void OnDrag(PointerEventData eventData) {
        isDragging = true;
        Vector2 newPos = rectTransform.anchoredPosition + new Vector2(0, eventData.delta.y);
        newPos.y = Mathf.Clamp(newPos.y, initialPositionY, stopPositionY);
        rectTransform.anchoredPosition = newPos;
    }

    public void OnEndDrag(PointerEventData eventData) {
        isDragging = false;
    }
}
