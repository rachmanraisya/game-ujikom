using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private bool grounded;
    private float wallJumpCooldown;
    private float horizontalInput;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        
        // Flip player when facing left/right.
        if (horizontalInput > 0.1f)
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        else if (horizontalInput < -0.1f)
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);

        //Set animator parameters
        anim.SetBool("run", horizontalInput !=0);
        anim.SetBool("grounded", isGrounded());

        //wall jump logic
        if (wallJumpCooldown < 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;

             if (Input.GetKey(KeyCode.Space))
             {
                Jump();

                if (Input.GetKey(KeyCode.Space) && isGrounded ())
                    SoundManager.instance.PlaySound(jumpSound);
             }
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower); 
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
    {
        if (horizontalInput == 0)
        {
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
            transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x) * 0.5f, 0.5f, 0.5f); // Jaga agar tetap 0.5 atau -0.5
        }
        else
        {
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            wallJumpCooldown = 0;
        }
    }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider !=null; 
    }

      private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider !=null; 
    }

}