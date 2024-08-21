using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMoving : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TrainMove()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.mrtPass, 0.3f);
        }
    }
}
