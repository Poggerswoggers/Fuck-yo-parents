using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Routegiver : MonoBehaviour
{
    //Ask Panel
    [SerializeField] GameObject askPanel;

    [SerializeField] List<string> responses;

    Vector3 startPos;

    //Ref
    SpriteRenderer sr;

    private void Start()
    {
        startPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    public void AskForDirection(string destinationName)
    {
        askPanel.SetActive(true);
        askPanel.GetComponentInChildren<TextMeshProUGUI>().text = $"How can I get to <u><color=orange>{destinationName}</u></color>?";
        startPos = transform.position;
    }

    public IEnumerator PostAssistance()
    {
        int randResponse = Random.Range(0, responses.Count - 1);
        askPanel.GetComponentInChildren<TextMeshProUGUI>().text = responses[randResponse];
        responses.RemoveAt(randResponse);
        yield return new WaitForSeconds(1f);
        askPanel.SetActive(false);

        sr.flipX = !sr.flipX;
        LeanTween.moveLocalX(gameObject, 8, 1f);
    }

    public void ResetPosition()
    {
        transform.position = startPos;
        sr.flipX= !sr.flipX;
    }
}
