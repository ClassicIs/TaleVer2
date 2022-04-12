using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEHolder : MonoBehaviour
{
    private LockCanvasScript theLock;
    private QTESliderScript theSlider;
    private CypherScript the—ypher;
    private isQtePassed theSimpleQTE;


    public enum TypesOfQTE
    {
        Lock,
        Slider,
        Cypher,
        Simple

    } 


    public void ActivateQTE(TypesOfQTE theQTE)
    {
        switch (theQTE)
        {
            case TypesOfQTE.Lock:
                theLock.Activate();
                break;
            case TypesOfQTE.Slider:
                theSlider.Activate();
                break;
            case TypesOfQTE.Cypher:
                the—ypher.Activate();
                break;
            case TypesOfQTE.Simple:
                theSimpleQTE.Activate();
                break;

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
