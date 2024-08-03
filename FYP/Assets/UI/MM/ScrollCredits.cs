using UnityEngine;

public class ScrollCredits : MonoBehaviour {
    public float scrollSpeed = 30f; // Adjust the speed as needed
    public float stopPositionY = 100f; // Y position where scrolling should stop

    private RectTransform rectTransform;
    private float initialPositionY;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        initialPositionY = rectTransform.anchoredPosition.y;
    }

    void Update() {
        // Calculate the new position
        float newY = rectTransform.anchoredPosition.y + scrollSpeed * Time.deltaTime;

        // Stop scrolling if the position reaches or exceeds the stop position
        if (newY >= stopPositionY) {
            newY = stopPositionY;
            enabled = false; // Disable this script to stop further updates
        }

        // Apply the new position
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
    }
}
