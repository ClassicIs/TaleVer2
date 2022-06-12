using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScript : QTEInWorldScript
{
    [SerializeField]
    DLDoor doorToOpen;
    //CypherScript cypherClass;
    [SerializeField]
    int panelCypher;

    public override void SuccessfullyUsed()
    {
        doorToOpen.OpenDoor();
    }

    public override void UnSuccessfullyUsed()
    {
        Debug.Log("Try again.");
    }

    protected override void ActivateQTE()
    {
        currentQTE = (CypherScript)QTEHolder.ActivateQTE(QTEHolder.TypesOfQTE.Cypher);
        currentQTE.Activate(panelCypher.ToString());
    }
}
