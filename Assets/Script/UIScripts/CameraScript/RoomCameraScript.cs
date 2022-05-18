using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomCameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObject virtualCam;

    [SerializeField]
    private CinemachineVirtualCamera vc;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (virtualCam.activeInHierarchy)
        {
            vc.Follow = player.transform;
        }

        else
        {
            //vc.Follow = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(false);
        }
    }
}