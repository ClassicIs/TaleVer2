using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private LayerMask need;
    RaycastHit2D theCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        RaycastHit2D tmpColider = theCollider;
        
        theCollider = Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0f, Vector2.zero, 1f, need);
        
        if(tmpColider != theCollider)
        {
            if(theCollider)
                Debug.Log("Is colliding with ground...");
            else
            {
                Debug.Log("Is not colliding with the ground!");
            }
        }
        
    }
}
