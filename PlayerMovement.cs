using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
  
    public Animator animator;
    public CharacterController2D controller;
    private float dashTime;
    public float dashSpeed;
    public float startDashTime;
    private Rigidbody2D rb;
    private int direction;
    public float runSpeed = 40f;
    bool jump = false;
    float horizontalMove = 0f;
    public float energy;
    public bool powerUpEffect = false;

     void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        energy = 10.0f;
    }
    // Update is called once per frame
    void Update()
    {
        gainEnergy();
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }
        if (direction == 0)
        {
            if (Input.GetKey(KeyCode.D))
            {
                direction = 1;
                if (direction == 1 && Input.GetMouseButton(1) && energy >= 2.0f)
                {
                    rb.velocity = Vector2.right * dashSpeed;
                    direction = 0;
                    dashTime = startDashTime;
                    animator.SetTrigger("Dash");
                    energy -= 1;
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                direction = 2;
                
                if (direction == 2 && Input.GetMouseButton(1) && energy >= 2.0f)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                    direction = 0;
                    dashTime = startDashTime;
                    animator.SetTrigger("Dash");
                    energy -= 1.0f;
                }
            }

        }

        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                
              
            }
            dashTime -= Time.deltaTime;
        }

        

    }
    
    void powerUp()
    {
        energy = 100.0f;
        runSpeed = 100.0f;
        powerUpEffect = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickup"))
        {
            powerUp();
            
        }
    }
    void gainEnergy()
    {
        if (energy < 25.0f)
        {
            energy += Time.deltaTime;
        }
        else if (energy >= 10.0f && powerUpEffect == false)
        {
            energy = 25.0f;
        }
    }

    
    public void OnLanding()
    {
        
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
        
    }
    
}
