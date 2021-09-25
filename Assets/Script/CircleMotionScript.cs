using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMotionScript : MonoBehaviour
{
    Transform toMakeCircleMotion;

    [SerializeField]
    private float radOfCircle;
    
    [SerializeField]
    private float speed;

    private float movePosition;
    private Vector2 strPos;


    // Start is called before the first frame update
    void Awake()
    {
        strPos = transform.position;
        DontDestroyOnLoad(gameObject);
        toMakeCircleMotion = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        movePosition += Time.deltaTime * speed;

        float theX = Mathf.Sin(movePosition) * radOfCircle;
        float theY = Mathf.Cos(movePosition) * radOfCircle;
        toMakeCircleMotion.position = new Vector2 (theX + strPos.x, theY + strPos.y);
    }
}
