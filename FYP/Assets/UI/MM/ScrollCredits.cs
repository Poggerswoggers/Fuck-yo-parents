using UnityEngine;

public class ScrollCredits : MonoBehaviour {
    public float scrollSpeed = 30f; 
    public float stopPositionY = 100f; 

    private RectTransform rectTransform;
    private float initialPositionY;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        initialPositionY = rectTransform.anchoredPosition.y;
    }

    void Update() {

        float newY = rectTransform.anchoredPosition.y + scrollSpeed * Time.deltaTime;

        // Stop scrolling if the position reaches or exceeds the stop position
        if (newY >= stopPositionY) {
            newY = stopPositionY;
            enabled = false; 
        }

        // Apply the new position
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
    }
}
