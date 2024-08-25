using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextWaveEffect : MonoBehaviour
{
    public TMP_Text text;

    Mesh mesh;

    private void Update()
    {
        text.ForceMeshUpdate();
        mesh = text.mesh;


    }
}
