using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/ New dialogue")]
public class DialogueText : ScriptableObject
{
    public string speakerName;

    [TextArea(5,10)]
    public string Info;

    public string[] options = new string[4];
}
