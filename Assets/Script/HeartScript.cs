using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour
{
    Animator theHeartAnim;

    private void Start()
    {
        theHeartAnim = GetComponent<Animator>();    
    }
    private void OnDisable()
    {
        theHeartAnim.SetTrigger("Loss");
        
    }
}
