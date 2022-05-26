using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLHatchController : MonoBehaviour
{
    [SerializeField]
    private DLHatch hatch;

    [SerializeField]
    private Gem redGem;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            // игрок активировал триггер
            hatch.OpenHatch();
            redGem.SetActive();
        }
    }
}
