using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueLine
{
    public enum Cast
    {
        char1,
        char2,
        char3
    }

    public Cast curCharacter;
    [TextArea]
    public string theLine;
}
