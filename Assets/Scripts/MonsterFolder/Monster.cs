
using Assets.Scripts.ScriptHelper;
using Mono.Cecil.Rocks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Assets.Scripts.MonsterFolder
{
    public class Monster : MonoBehaviour
    {
        [SerializeField]
        AudioClip getHit_AudioClip;


        CapsuleCollider2D myCollider;
        Rigidbody2D myRigidBody;
        Animator myAnimator;
        AudioSource myAudioSource;


        public float offset = 0.2f;
        public float JumpPower = 2f;



        public Guid Id { get; set; }

        public int Health = 0;

        public int Damage = 0;

        public int TouchDamage = 1;


        public Loot Loot;


        bool isPlayerAtTheRight;
        private bool isAttacking = false;
        private bool isJumping = false;

        /// <summary>
        /// //монстр не поварачивается потому что нужно оффсет юзать нужно  а   .transform.localScale 
        /// </summary>
        SpriteRenderer handRenderer;
        SpriteRenderer legRenderer;
        SpriteRenderer bodyRender;
        private void Awake()
        {
            // Находим нужные объекты
            Transform body = transform.Find("Body");
            Transform leftHand = body.Find("Arm Left");
            Transform leftLeg = transform.Find("Leg Left");

             handRenderer = leftHand.GetComponent<SpriteRenderer>();
             legRenderer = leftLeg.GetComponent<SpriteRenderer>();
             bodyRender = body.GetComponent<SpriteRenderer>();

            bodyRender.sortingOrder = 1;
            handRenderer.sortingOrder = 0;
            legRenderer.sortingOrder = 0;


            myCollider = GetComponent<CapsuleCollider2D>();
            myRigidBody = GetComponent<Rigidbody2D>();
            myAnimator = GetComponent<Animator>();
            myAudioSource = GetComponent<AudioSource>();

            if (Health == 0)
            {
                Health = 20;
            }
            if (Damage == 0)
            {
                Damage = 5;
            }
        }

        private void LateUpdate()
        {
            FacePlayer();

            StartChasingPlayer();

            if (!isAttacking)
            {
                isAttacking = true;

                StartCoroutine(Attack(0.8f, 1f, 35, 3, Damage));

            }


        }



        void FacePlayer()
        {
            HashSet<GameObject> hitObjects = new HashSet<GameObject>();

            GameObject gameobj;

            RayCastThrower.color = Color.red;
            hitObjects.AddRange(RayCastThrower.ThrowRayCasts(transform.position.x, transform.position.y, 5, 40, 3, Vector2.left));
            hitObjects.AddRange(RayCastThrower.ThrowRayCasts(transform.position.x, transform.position.y, 5, 40, 3, Vector2.right));


            gameobj = hitObjects.FirstOrDefault(i => i.CompareTag("Player"));

            if (gameobj != null)
            {
                if (transform.position.x - gameobj.transform.position.x > 0)
                {
                    transform.localScale = new Vector2(0.35f, 0.35f);
                    isPlayerAtTheRight = true;

                }

                if (transform.position.x - gameobj.transform.position.x < 0)
                {
                    transform.localScale = new Vector2(-0.35f, 0.35f);
                    isPlayerAtTheRight= false;
                }

            }

        }


        void StartChasingPlayer()
        {
            myAnimator.SetBool("isMoving", false);




            HashSet<GameObject> hitObjects = new HashSet<GameObject>();

            Vector2 rotation = isPlayerAtTheRight ? Vector2.left : Vector2.right;



            GameObject playerObj;

            RayCastThrower.color = Color.blue;
            hitObjects.AddRange(RayCastThrower.ThrowRayCasts(transform.position.x, transform.position.y, 2, 30, 3, rotation));

            playerObj = hitObjects.FirstOrDefault(i => i.CompareTag("Player"));
            if (playerObj == null) return;



            RayCastThrower.color = Color.black;
            var groundObj =RayCastThrower.ThrowRayCast(transform.position.x, transform.position.y, 1f,new Vector2(rotation.x,-1f));

            if (groundObj == null || !groundObj.CompareTag("Ground")) return;
            
                //if (!isJumping)
                //JumpOverVoid(rotation.x * 2);
            
                transform.position += new Vector3(rotation.x, 0f, 0f) * 1 * Time.deltaTime;


            myAnimator.SetBool("isMoving",true);
        }



        //void JumpOverVoid(float x)
        //{
        //    isJumping = true;

        //    myRigidBody.velocity = new Vector2(1 * x, JumpPower);
        //}




            IEnumerator Attack(float timeDelay,float rayDistance,float rayAngle,float RayCount,float damage)
        {
            yield return new WaitForSeconds(timeDelay);

            HashSet<GameObject> hitObjects = new HashSet<GameObject>();

            Vector2 rotation = isPlayerAtTheRight ? Vector2.left : Vector2.right;

            GameObject gameobj;
            RayCastThrower.color = Color.green;
            hitObjects.AddRange(RayCastThrower.ThrowRayCasts(transform.position.x, transform.position.y, rayDistance, rayAngle, RayCount, rotation));

            gameobj = hitObjects.FirstOrDefault(i => i.CompareTag("Player"));

            if (gameobj != null)
            {
                myAnimator.SetTrigger("attack");
                gameobj.GetComponent<PlayerMovementScript>().GetDamage((int)damage);
            }

            isAttacking = false;
        }
        




        public virtual Loot TakeDamage(int damage)
        {
            myAudioSource.clip = getHit_AudioClip;
            myAudioSource.Play();
         StartCoroutine(   DamageEffect());


            Health -= damage;

            if (Health <= 0)
            {
                StartCoroutine(DestroyMonster());

                return Loot;
            }



            return null;
        }

        IEnumerator DamageEffect()
        {
            bodyRender .color = Color.red;

            yield return new WaitForSeconds(0.5f);

            bodyRender.color = Color.white;

        }


        IEnumerator DestroyMonster()
        {
            yield return new WaitForSeconds(0.2f);

            Destroy(gameObject);
        }


        protected virtual int Attack()
        {
            myAnimator.SetTrigger("attack");

            return Damage;
        }



        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag( "Player"))
            {
                collision.gameObject.GetComponent<PlayerMovementScript>().GetDamage(TouchDamage);
            }
            if (collision.gameObject.CompareTag("Ground"))
            {
                isJumping = false;
            }
        }

    }
}
