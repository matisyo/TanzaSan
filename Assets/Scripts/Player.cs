using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField]
    private float movementSpeed;

    private bool facingRight;
    private bool attack;
    private bool slide;
    private bool jump;
    public bool onGround;
    public Vector2 jumpForce;
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;

	// Use this for initialization
	void Start () {
        facingRight = false;
        attack = false;
        slide = false;
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
	}

    // Update is called once per frame
    void FixedUpdate() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        HandleInput();
        HandleMovement(horizontal,vertical);
        HandleSlide();
        HandleAttacks();
        flip(horizontal);
        resetValues();
        
	}

    void Update()
    {
        HandleInput();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("ground"))
        {
            onGround = true;
        }
    }


    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            attack = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            slide = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(onGround)
                jump = true;
            
        }
       
        
    }
    
    private void HandleMovement(float horizontal,float vertical)
    {
        if (!myAnimator.GetBool("slide") && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            myRigidBody.velocity = new Vector2(horizontal * movementSpeed, myRigidBody.velocity.y); //x = -1 y = 0
            
            if (jump && onGround) {
                
                jump = false;
                onGround = false;
                myRigidBody.AddForce(new Vector2(jumpForce.x, jumpForce.y), ForceMode2D.Impulse);
                
                
            }
        }



            myAnimator.SetFloat("speed",Mathf.Abs(horizontal));
    }

    private void HandleSlide()
    {
        if (slide && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("slide"))
        {
            myAnimator.SetBool("slide", true);
            
        }
        else if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("slide"))
        {
            myAnimator.SetBool("slide", false);
        }


        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("slide"))
        {
            float aux = -3;
            if (facingRight)
                aux = 3;
            myRigidBody.velocity = new Vector2(aux * movementSpeed, myRigidBody.velocity.y);
        }
    }

    private void HandleAttacks()
    {
        if (attack && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            myAnimator.SetTrigger("attack");
            myRigidBody.velocity = Vector2.zero;
        }
    }



    private void flip(float horizontal)
    {
        if (!(this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack") && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("slide")))
            if (horizontal > 0 && !facingRight || horizontal<0 && facingRight)
            {
                facingRight = !facingRight;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
    }

    private void resetValues()
    {
        slide = false;
        attack = false;
    }
}
