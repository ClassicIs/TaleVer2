/*using System.Collections;
using System.Collections.Generic;
*/
using UnityEngine;

public class SpawnObjects
{
    public Vector3 thePosition;
    public string nameOfPrefab;

    public SpawnObjects(string theName, Vector3 theCurPos)
    {
        this.nameOfPrefab = theName;
        this.thePosition = theCurPos;        
    }
}
