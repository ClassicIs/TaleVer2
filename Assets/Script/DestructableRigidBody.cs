using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableRigidBody : MonoBehaviour
{
    [SerializeField]
    Vector2 forceDirection;

    [SerializeField]
    float torque;

    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        float randTorque = UnityEngine.Random.Range(-10, 10);
        float randForceX = UnityEngine.Random.Range(forceDirection.x - 50, forceDirection.x + 50);
        float randForceY = UnityEngine.Random.Range(forceDirection.y - 50, forceDirection.y + 50);

        forceDirection.x = randForceX;
        forceDirection.y = randForceY;

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(forceDirection);
        rb2d.AddTorque(randTorque);
        StartCoroutine(WaitToDestroy());
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(2f);
        Color tmpColor = spriteRenderer.color;
        tmpColor.a = 1;
        while (tmpColor.a > 0.1f)
        {
            tmpColor.a -= 0.05f;
            spriteRenderer.color = tmpColor;
            yield return null;
        }
        tmpColor.a = 0f;
        spriteRenderer.color = tmpColor;
        Destroy(gameObject);
    }

}
