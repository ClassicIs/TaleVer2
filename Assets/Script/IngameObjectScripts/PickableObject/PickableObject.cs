using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public ItemScript thisItem;
    public void OnPlayerCollision()
    {
        transform.gameObject.SetActive(false);
    }
}
