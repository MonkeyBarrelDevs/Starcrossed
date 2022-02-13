using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Animator faceAnim;
    [SerializeField] float animMoveThreshold;

    public ParticleSystem playerParticles;
    public Rigidbody2D playerRidge;
    public CircleCollider2D playerCollider;
    private EarthController earth;
    public int playerNumber;
    private int emissionRate = 10;
    GameController gameController;
    AudioManager audioManager;
    [SerializeField] int absorbScaleFactor;
    //bool canMove;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        audioManager = FindObjectOfType<AudioManager>();
        earth = FindObjectOfType<EarthController>();
        //canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D col)  {
        if (col.collider.gameObject.tag == "Asteroid") {
            FindObjectOfType<AudioManager>().Play(audioManager.sounds[Random.Range(2, 6)].name);
            Destroy(col.collider.gameObject);
            //changes the pull strength of BH based on absorbed asteroids
            playerCollider.radius += absorbScaleFactor;
            ParticleSystem.ShapeModule sm = playerParticles.shape;
            sm.radius += 0.2f;
            ParticleSystem.EmissionModule em = playerParticles.emission;
            emissionRate += 5;
            em.rateOverTime = emissionRate;
            playerParticles.startLifetime += 1;
        }

        
    }

    void FixedUpdate()
    {
        if (earth.gameObject.activeInHierarchy)
            faceAnim.SetBool("NearEarth", (earth.gameObject.transform.position - gameObject.transform.position).magnitude < playerCollider.radius);
        faceAnim.SetBool("Moving", playerRidge.velocity.magnitude > animMoveThreshold);

        if (gameController.MoveCheck()) {
            Vector2 playerInput = new Vector2(0, 0);
            if (playerNumber == 1)
            {
                playerInput = new Vector2(Input.GetAxis("Horizontal 1"), Input.GetAxis("Vertical 1"));
            }
            else if (playerNumber == 2)
            {
                playerInput = new Vector2(Input.GetAxis("Horizontal 2"), Input.GetAxis("Vertical 2"));
            }
            playerRidge.velocity = playerInput * 20;
        } else {
            playerRidge.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void EatEarth() 
    {
        anim.SetTrigger("EatEarth");
    }
}
