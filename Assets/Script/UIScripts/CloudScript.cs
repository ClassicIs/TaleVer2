using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudScript : MonoBehaviour
{
    RectTransform theCloud;
    [SerializeField] private float speedOfCloud;
    public float newPos;
    public float endPos;
    public float startPos;

    // Start is called before the first frame update
    void Start()
    {       
        theCloud = GetComponent<RectTransform>();
        //speedOfCloud = .5f;
        startPos = theCloud.position.x;
        newPos = startPos;
        endPos = (-1) * startPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Approximately(newPos, endPos))
        {
            newPos = startPos;
        }

        newPos = Mathf.MoveTowards(newPos, endPos, speedOfCloud / 100f);        
        theCloud.position = new Vector2(newPos, theCloud.position.y);
    }
}
