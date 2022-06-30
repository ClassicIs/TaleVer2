using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScript : QTEInWorldScript
{

    [SerializeField]
    int panelCypher;

    [SerializeField]
    DLDoor[] doorsToOpen;

    public override void SuccessfullyUsed()
    {
        foreach (DLDoor door in doorsToOpen)
        {
            door.OpenDoor();
        }
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
