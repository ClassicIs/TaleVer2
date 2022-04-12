using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueLine
{
    public Character theSpeaker;
    [TextArea]
    public string theLine;
}
