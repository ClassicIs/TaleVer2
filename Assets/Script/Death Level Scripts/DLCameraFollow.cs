using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLCameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        //_cameraOffset = transform.position - playerPos.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
