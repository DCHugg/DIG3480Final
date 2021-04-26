using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    
    public int maxHealth = 5;
    public int ammo = 5;
    
    public GameObject projectilePrefab;
    public GameObject healthParticle;
    public GameObject ouchParticle;

    public static int level = 1;

    public int scoreValue=0;
    public int bonusValue=0;
    
    public Text ammoUI;
    public Text bonusUI;
    public Text scoreUI;
    public Text winText;
    public Text tutorialUI;


   public bool gameOver = false;
   public bool tutorial = false;
   public bool ready = false;

   public AudioSource musicSource;
   public AudioClip musicClip1;
   public AudioClip background;
   public AudioClip musicClip2;
   public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip bonusSound;
    public AudioClip jambiSound;
    public AudioClip cogSound;
    public AudioClip teleportSound;
 
   
    
    public int health { get { return currentHealth; }}
    int currentHealth;
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

     
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        
        currentHealth = maxHealth;
        winText.text ="";
        scoreValue=0;
         scoreUI.text = scoreValue.ToString() + " / 5 Fixed Robots";
         musicSource.clip = background;
                musicSource.Play();
        bonusUI.text = bonusValue.ToString() + " / 3 Cats Found";
        
            
    }










    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

            if(gameOver == true )
            {
              if(level ==2 )
                 {
                     if (Input.GetKeyDown(KeyCode.R))
                        {
                            SceneManager.LoadScene("MainScene"); // this loads the currently active scene
                        }
                 }

                if(level == 1)
                 {
                    if (Input.GetKeyDown(KeyCode.R))
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene
                     }
                }
            }



            if (Input.GetKeyDown(KeyCode.T))
            {
                tutorial=true;
                PlaySound(teleportSound);
                tutorialUI.text="Tutorial Mode";
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                tutorial=false;
                PlaySound(teleportSound);
                tutorialUI.text="Ready to go!";
            }



            if (tutorial == false)
            {
                if (Input.GetKeyDown(KeyCode.N))
                        {
                            SceneManager.LoadScene("MainScene"); // this loads the currently active scene
                        }
            }

             if (tutorial == true)
            {
                if (Input.GetKeyDown(KeyCode.M))
                        {
                            
                            SceneManager.LoadScene("Level 3"); // this loads the currently active scene
                        }
            }



        if (Input.GetKey("escape"))
    {

Application.Quit();
    }
        
         animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayableCharacter character = hit.collider.GetComponent<NonPlayableCharacter>();
                if (character != null)
                {
                    if(scoreValue == 5)
                        {
                            levelChange();
                        }
                    character.DisplayDialog();
                    PlaySound(jambiSound);

                }
            }
        }

ammoUI.text = "Cogs: " + ammo.ToString();


    }


    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

 private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Ammo")
        {
            ammo += 1;
            ammoUI.text = "Cogs: " + ammo.ToString();
            Destroy(collision.collider.gameObject);
            PlaySound(cogSound);
        }
    }




    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");            
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            Losehealthfx();
            PlaySound(hitSound);

            if(currentHealth == 0)
            {
                musicSource.Stop();
                musicSource.clip = musicClip1;
                musicSource.Play();
                speed = 0;
                winText.text = "You LOSE! Press R to Restart";
                gameOver = true;
            }
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if(amount > 0)
        {
            Gainhealth();
        }
    }

    public void ChangeScore(int f)
    {
        if (f > 0)
        {
            scoreValue += 1;
             scoreUI.text = scoreValue.ToString() + " / 5 Fixed Robots";


             if (level == 1)
             {
                    if (scoreValue == 5 )
                    {
                         musicSource.Stop();
                        musicSource.clip = musicClip1;
                        musicSource.Play();
                        winText.text = "You Win. Talk to Jambi to Advance";
                    }

                /**/
             }
        }
    }

    public void ChangeBonus(int e)
    {
        if (e > 0)
        {
            bonusValue += 1;
             bonusUI.text = bonusValue.ToString() + " / 3 Pets Found";
             PlaySound(bonusSound);

             if (bonusValue == 3 && scoreValue ==5 && level ==2)
             {
                 gameOver = true;
                    winText.text = "You Win. Press R to restart Game";
                        if (gameOver == true)
                            {
                                if (Input.GetKeyDown(KeyCode.R))
                                    {
                                    SceneManager.LoadScene("MainScene"); 
                                    }

                                 musicSource.Stop();
                                 musicSource.clip = musicClip2;
                                musicSource.Play();
                            }
             }

        }

    }

    void Launch()
    {
        if (ammo > 0)
        {
            PlaySound(throwSound);
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        ammo -= 1;
        
        }

        
    }

void Losehealthfx()
{
   Instantiate(ouchParticle, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
}

void Gainhealth()
{
    Instantiate(healthParticle, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
}


void levelChange()
{
    SceneManager.LoadScene("Level 2");
    level = 2;
    scoreValue = 0;
}



public void PlaySound(AudioClip clip)
{
    musicSource.PlayOneShot(clip);
}


}
