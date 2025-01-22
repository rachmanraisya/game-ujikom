using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private Animator anim;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Flip player when facing left/right.
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);

        if (Input.GetKey(KeyCode.Space) && isGrounded())
            Jump();

            //Set animator parameters
            anim.SetBool("run", horizontalInput !=0);
            anim.SetBool("grounded", isGrounded());
    
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed); 
        anim.SetTrigger("jump");

    private void OnCollisionEnter2D(Collision2D collision)
    { 
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast()
        return false; 
    }

   
} 