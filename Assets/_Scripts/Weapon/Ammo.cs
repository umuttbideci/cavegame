using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{

    public float lifetime = 0.1f;

    void Start()
    {
        //Destroy(gameObject, lifetime);
        //BoxCollider2D col = GetComponent<BoxCollider2D>();
        StartCoroutine(CollisionType(0.2f));
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
