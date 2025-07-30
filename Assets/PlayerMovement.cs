using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    public float speed;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = new Vector2(
            Input.GetAxis("Horizontal") * speed,
            body.velocity.y
            );
    }
}
