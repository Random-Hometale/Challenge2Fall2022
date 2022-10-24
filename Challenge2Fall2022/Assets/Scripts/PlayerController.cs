using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rd2d;
    
    public float speed;
    public float jump;
    
    public Text score;
    public Text livesText;
    public GameObject winTextObject;
    public GameObject loseTextObject;

    //audio
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioClip musicClipThree;
    public AudioSource musicSource;
    
    


    private int scoreValue = 0;
    private int livesValue = 3;


    //animation
    Animator anim;
    private bool facingRight = true;

    //ground check
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;


    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        score.text = "Score: " + scoreValue.ToString();
        livesText.text = "Lives: " + livesValue.ToString();

        rd2d = GetComponent<Rigidbody2D>();

        musicSource.clip = musicClipOne;
        musicSource.Play();


        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        SetCountText();

    }

    void SetCountText()
    {
        score.text = "Score: "+ scoreValue.ToString();

        if(scoreValue == 4)
        {
            transform.position = new Vector2(-15.55f, 49.43f);
            livesValue = 3;
        }

        if (scoreValue >= 8)
        {
            winTextObject.SetActive(true);
            musicSource.clip = musicClipOne;
            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = false;
            speed = 0;
        }

        livesText.text = "Lives: " + livesValue.ToString();
        if (livesValue == 0)
        {
            loseTextObject.SetActive(true);
            musicSource.clip = musicClipOne;
            musicSource.Stop();
            musicSource.clip = musicClipThree;
            musicSource.Play();
            musicSource.loop = false;
            speed = 0;
        }

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        //ground check
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);


        //animation flip
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }

        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }



    }
    
    void Update()
    {    //animation
        if (Input.GetKeyDown(KeyCode.A))
            {
                anim.SetInteger("State", 1);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                anim.SetInteger("State", 0);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                anim.SetInteger("State", 1);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                anim.SetInteger("State", 0);
            }

            //jump
            if (Input.GetKeyDown(KeyCode.W))
            {
                anim.SetInteger("State", 2);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetInteger("State", 0);
            }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            SetCountText();
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            SetCountText();
            Destroy(collision.collider.gameObject);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
            }
        }
    }

    void Flip()
    {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
    }



}
