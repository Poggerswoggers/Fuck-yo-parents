using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countText;

   
    private void OnEnable()
    {
        TakeASeatGm.vulnerableMatch += UpdateCount;
    }

    private void OnDisable()
    {
        TakeASeatGm.vulnerableMatch -= UpdateCount;
    }

    void UpdateCount(int count)
    {
        countText.text = ":" + count.ToString();
    }
}
