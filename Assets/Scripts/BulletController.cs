using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private int damage = 25;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ZombieController zombie = collision.gameObject.GetComponent<ZombieController>();

        if (zombie != null)
        {
            zombie.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
