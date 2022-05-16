using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QTEHolder : MonoBehaviour
{
    [SerializeField] private LockCanvasScript theLock;
    [SerializeField] private QTESliderScript theSlider;
    [SerializeField] private CypherScript the—ypher;
    [SerializeField] private isQtePassed theSimpleQTE;
    
    public enum TypesOfQTE
    {
        Lock,
        Slider,
        Cypher,
        Simple

    } 

    public QTEObject ActivateQTE(TypesOfQTE theQTE)
    {
        QTEObject NewQTE;

        switch (theQTE)
        {
            case TypesOfQTE.Lock:
                NewQTE = theLock;
                break;
            case TypesOfQTE.Slider:
                NewQTE = theSlider;                
                break;
            case TypesOfQTE.Simple:
                NewQTE = theSimpleQTE;
                break;
            case TypesOfQTE.Cypher:
                NewQTE = the—ypher;
                break;
            default:
                NewQTE = null;
                break;
        }
        return NewQTE;      
    }   

    void Start()
    {
        //ActivateQTE(TypesOfQTE.Slider).Activate(QTEObject.HardVariety.easy);

        /*
        CypherScript ThisCypher = (CypherScript)ActivateQTE(TypesOfQTE.Cypher);
        ThisCypher.Activate("1234");*/
    }
}
