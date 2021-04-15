using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ObjectPooler objectPooler;

    private Rigidbody2D rigidBody;

    public float timeToLive = 10.0f;
    private float timer;

    private void Awake()
    {
        objectPooler = ObjectPooler.instance;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timer = timeToLive;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void Launch(Vector2 dir, float force)
    {
        rigidBody.AddForce(dir * force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // TODO: Change to SetActive(false) in case you do enemies via object pooling
            Destroy(collision.gameObject);
        }

        this.gameObject.SetActive(false);
    }
}
