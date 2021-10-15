using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptForLetter : MonoBehaviour
{
    [SerializeField]
    Animator theLetterAnim;    

    private void OnEnable()
    {
        theLetterAnim.SetBool("LetterOpen", true);
    }

    private void OnDisable()
    {
        theLetterAnim.SetBool("LetterOpen", false);
    }
}
