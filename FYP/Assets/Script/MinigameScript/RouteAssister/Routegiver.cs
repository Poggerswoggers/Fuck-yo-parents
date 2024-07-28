using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class Routegiver : MonoBehaviour
{
    //Ask Panel
    [SerializeField] GameObject askPanel;

    [SerializeField] List<string> responses;

    public void AskForDirection(string destinationName)
    {
        askPanel.SetActive(true);
        askPanel.GetComponentInChildren<TextMeshProUGUI>().text = $"How can I get to <u><color=green>{destinationName}</u></color>?";
    }

    public async Task PostAssistance()
    {
        int randResponse = Random.Range(0, responses.Count - 1);
        askPanel.GetComponentInChildren<TextMeshProUGUI>().text = responses[randResponse];
        responses.RemoveAt(randResponse);
        await Task.Delay(1000);
        askPanel.SetActive(false);

        GetComponent<SpriteRenderer>().flipX = false;
        LeanTween.moveLocalX(gameObject, 8, 1f);
    }
}
