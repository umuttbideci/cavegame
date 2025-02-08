using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{

    public float lifetime = 2f;

    void Start()
    {
        Destroy(gameObject, lifetime);
        BoxCollider2D col = GetComponent<BoxCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }


    private IEnumerator CollisionType(float time)
    {
       yield return new WaitForSeconds(time);

    }
}
