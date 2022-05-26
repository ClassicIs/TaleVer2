using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLHatch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenHatch()
    {
        gameObject.GetComponent<Animator>().SetBool("IsOpened", true);
    }

    public void CloseHatch()
    {
        gameObject.GetComponent<Animator>().SetBool("IsOpened", false);
    }

}
