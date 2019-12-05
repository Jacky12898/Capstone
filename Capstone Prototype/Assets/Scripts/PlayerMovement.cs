using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public KeyCode left, right, jump;
    public float maxSpeed = 50;
    public float speed = 0;
    public float distance = 0.1f;
    private Rigidbody2D rb;
    private float horizontalMove = 0f;
    private bool grounded = false;
    private int jumps = 1;
    private int jumpsTemp = 1;
    private bool wallJump = false;
    private BoxCollider2D Collider;
    public PhysicsMaterial2D matFric;
    public PhysicsMaterial2D matNoFric;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();
        Collider.sharedMaterial = matNoFric;
    }

    void Update()
    {
        if (rb.velocity.y == 0)
            grounded = true;

        else
            grounded = false;

        horizontalMove = Input.GetAxisRaw("Horizontal") * maxSpeed;
        rb.velocity = new Vector2(horizontalMove * Time.fixedDeltaTime * 10f, rb.velocity.y);

        //if(Input.GetAxisRaw("Horizontal") != 0)
        //{
        //    if (speed < maxSpeed)
        //    {
        //        speed = speed + horizontalMove * 10 * Time.deltaTime;
        //        rb.velocity = new Vector2(speed, rb.velocity.y);
        //    }
        //}

        //else
        //{
        //    speed = 0;
        //    rb.velocity = new Vector2(0, rb.velocity.y);
        //}

        if (Input.GetButtonDown("Jump") && jumpsTemp > 0)
        {
            grounded = false;
            rb.velocity = new Vector2(rb.velocity.x, Time.fixedDeltaTime * 10f * maxSpeed);
            jumpsTemp--;
        }

        if (grounded)
        {
            jumpsTemp = jumps;
        }

        if (wallJump)
        {
            //JumpOffWall();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        Debug.Log(other.name);
        if(other.tag == "PowerUp"){
            switch (other.name) {
                case "Plunger":
                    wallJump = true;
                    Collider.sharedMaterial = matFric;
                    break;

                case "Balloon":
                    jumps = 2;
                    jumpsTemp = 2;
                    break;

                default:
                    Debug.Log("Unknown PowerUp");
                    break;
            }
            Destroy(obj);
        }
    }

    void JumpOffWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, distance);

        if (Input.GetButtonDown("Jump") && !grounded && (hit.collider != null || hit2.collider != null))
        {
            speed = 0;
            rb.velocity = new Vector2(-horizontalMove * Time.fixedDeltaTime * 80f * maxSpeed, Time.fixedDeltaTime * 10f * maxSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * distance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * transform.localScale.x * distance);
    }
}
