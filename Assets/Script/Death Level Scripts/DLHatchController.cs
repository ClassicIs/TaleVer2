using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLHatchController : MonoBehaviour
{
    [SerializeField]
    private DLHatch hatch;

    [SerializeField]
    private Gem redGem;

    private bool wasOpened = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null && wasOpened == false)
        {
            // игрок активировал триггер
            hatch.OpenHatch();
            redGem.SetActive();
            wasOpened = true;
        }
    }
}
