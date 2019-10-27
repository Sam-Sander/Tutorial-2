using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    public Text winText;
    public Text liveText;
    private int lives;
    public AudioSource musicSource;
    public AudioClip musicClipBG;
    public AudioClip musicClipWin;
    Animator anim;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        winText.text = "";
        lives = 3;
        liveText.text = "";
        SetLiveText();
        anim = GetComponent<Animator>();
        musicSource.clip = musicClipBG;
        musicSource.Play();
        musicSource.loop = true;
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            CheckCountText();
        }
        else if (collision.collider.tag == "Enemy")
        {
            lives = lives - 1;
            SetLiveText();
            Destroy(collision.collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                anim.SetInteger("State", 2);
            }
        }
        if (collision.collider.tag != "Ground")
        {
            if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetInteger("State", 0);
            }
        }
    }
    void SetLiveText()
    {
        liveText.text = "Lives: " + lives.ToString();
        if (lives == 0)
        {
            Destroy(this);
            winText.text = "You lose!";
        }
    }
    void CheckCountText()
    {
        if (scoreValue == 4)
        {
            transform.position = new Vector2(63f, 0.99f);
            lives = 3;
            liveText.text = "";
            SetLiveText();
        }
        if (scoreValue == 8)
        {
            musicSource.Stop();
            musicSource.loop = false;
            musicSource.clip = musicClipWin;
            musicSource.Play();
            musicSource.loop = false;
            winText.text = "You win! Game created by Samantha Sander!";
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