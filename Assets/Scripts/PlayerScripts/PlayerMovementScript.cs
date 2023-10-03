
using Assets.Scripts.MonsterFolder;
using Assets.Scripts.ScriptHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerMovementScript : NetworkBehaviour
{
    [SerializeField]
    AudioClip getHit_AudioClip;

    [SerializeField]
    AudioClip levelUp_AudioClip;



    [SerializeField]
    private float playerRunSpeed = 10f;

    [SerializeField]
    private float playerWalkSpeed = 5f;






    public float attackRange = 3f;  // Дальность атаки
    public float attackAngle = 20f; // Угол "конуса" атаки в градусах
    public int raysCount = 3;       // Количество лучей в "конусе"
    public float offset = 0.3f;     // Смещение начальной точки райкаста
    public int damage = 10;         // Урон от атаки


    private int _health = 100;

    private int _damage = 5;

    private int blockResistance = 10;

    [SerializeField]
    public float jumpForce = 5f;

    private float movementX;
    private float movementY;
    private bool isJumping = false, block = false;
    public bool isFacingRight = true;

    [SerializeField]
    GameObject gun;


    Rigidbody2D myRigidBody;
    Animator animator;
    Canvas UI;
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        audioSource  = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //UI = GameObject.Find("UI").GetComponent<Canvas>() ;

        //UI.worldCamera = Camera.main;
    }



    private void Update()
    {
        if (/*InventoryScript.isInventoryOpen *//*||*/!IsOwner) return;

        Move();
        HandlePlayerAttack();
        HandlePlayerBlock();
        HandlePlayerJump();

    }


    void Move()
    {
        float movementX = Input.GetAxisRaw("Horizontal");
        HandlePlayerTurn(movementX);
        if (Input.GetKey(KeyCode.LeftControl))
        {

            HandlePlayerRun(movementX);
        }
        else 
        {

            HandlePlayerWalk(movementX);
        }
  
    }
    private void HandlePlayerWalk(float movementX)
    {


        transform.position += new Vector3(movementX, 0f, 0f) * Time.deltaTime * playerWalkSpeed;

        if (movementX != 0)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsRunning", false);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

    }

    private void HandlePlayerRun(float movementX)
    { 

        transform.position += new Vector3(movementX, 0f, 0f) * Time.deltaTime * playerRunSpeed;

        if (movementX != 0)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    void HandlePlayerTurn(float horizontal)
    {
        if (horizontal == 0) return;

        Vector2 direction = new Vector2(horizontal, 1f);
        transform.localScale *= direction;



        if (horizontal > 0)
        {
            transform.localScale = new Vector2(0.35f, 0.35f);
            isFacingRight = true;
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector2(-0.35f, 0.35f);
            isFacingRight = false;
        }

    }


    void HandlePlayerAttack()
    {
        if (Input.GetMouseButton(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("attackAnimation"))
        {
            animator.SetBool("IsAttacking", true);
            PerformAttack();

        }
        else
        {
            animator.SetBool("IsAttacking", false);
        }
    }
    void PerformAttack()
    {
        HashSet<GameObject> hitObjects = new HashSet<GameObject>();
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        RayCastThrower.color = Color.white;
        hitObjects.AddRange(RayCastThrower.ThrowRayCasts(transform.position.x, transform.position.y, attackRange, attackAngle, raysCount, direction));

        var hitObject = hitObjects.FirstOrDefault(i => i.CompareTag("Player") || i.CompareTag("Monster"));

        if (hitObject == null) return;

        if (hitObject.CompareTag("Monster"))
            hitObject.GetComponent<Monster>().TakeDamage(damage);

        if (hitObject.CompareTag("Player"))
            hitObject.GetComponent<PlayerMovementScript>().GetDamage(damage);
    }


    void HandlePlayerBlock()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerWalkSpeed = 0f;
            playerRunSpeed = 0f;

            animator.SetBool("IsBlocking", true);

            block = true;



        }
        else if (Input.GetMouseButtonUp(1))
        {

            animator.SetBool("IsBlocking", false);

            block = false;

            playerRunSpeed = 5f;
            playerWalkSpeed = 3f;


        }
    }
    private void HandlePlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            myRigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            isJumping = true;
        }
    }

    void HandlePlayerDie()
    {
        StartCoroutine(DieCoroutine());
    }
    IEnumerator DieCoroutine()
    {
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }


    public void GetDamage(int damage)
    {
        audioSource.clip = getHit_AudioClip;
        audioSource.Play();
        StartCoroutine(DamageEffect());

        Debug.Log("Player get hit");

        if (block)
        {
            damage /= blockResistance;

        }

        _health -= damage;
        Debug.Log("_health: " + _health);


        if (_health < 0)
        {
            Destroy(gameObject);
            Debug.Log("Player killed");
        }
    }

    IEnumerator DamageEffect()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.color = Color.white;

    }

    // Проверка, касается ли игрок земли
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }


}