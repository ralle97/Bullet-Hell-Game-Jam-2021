using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;

    private float horizontal;
    private float vertical;

    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidBody.position;
        position.x += horizontal * speed * Time.deltaTime;
        position.y += vertical * speed * Time.deltaTime;

        rigidBody.MovePosition(position);
    }
}
