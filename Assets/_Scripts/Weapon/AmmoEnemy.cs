using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoEnemy : MonoBehaviour
{

    public float lifetime;

    void Start()
    {
        //Destroy(gameObject, lifetime);
        //BoxCollider2D col = GetComponent<BoxCollider2D>();
        StartCoroutine(CollisionType(lifetime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator CollisionType(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
