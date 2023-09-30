
using Assets.Scripts.ScriptHelper;
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

        CapsuleCollider2D myCollider;

        Rigidbody2D myRigidBody;

        Animator myAnimator;

        public float offset = 0.2f; 



        public Guid Id { get; set; }

        public int Health = 0;

        public int Damage = 0;

        public Loot Loot;


        bool isPlayerAtTheRight;
        private bool isAttacking = false;

        /// <summary>
        /// //монстр не поварачивается потому что нужно оффсет юзать нужно  а   .transform.localScale 
        /// </summary>


        private void Awake()
        {
            // Находим нужные объекты
            Transform body = transform.Find("Body");
            Transform leftHand = body.Find("Arm Left");
            Transform leftLeg = transform.Find("Leg Left");

            SpriteRenderer handRenderer = leftHand.GetComponent<SpriteRenderer>();
            SpriteRenderer legRenderer = leftLeg.GetComponent<SpriteRenderer>();

            handRenderer.sortingOrder = 0;
            legRenderer.sortingOrder = 0;


            myCollider = GetComponent<CapsuleCollider2D>();
            myRigidBody = GetComponent<Rigidbody2D>();
            myAnimator = GetComponent<Animator>();

            if (Health == 0)
            {
                Health = 15;
            }
            if (Damage == 0)
            {
                Damage = 1;
            }
        }

        private void LateUpdate()
        {
            StartCoroutine(FacePlayer());

            if (!isAttacking)
            {
                isAttacking = true;

            StartCoroutine(attackPlayer());

            }
        }



        IEnumerator FacePlayer()
        {

            yield return new WaitForSeconds(0.5f);

            HashSet<GameObject> hitObjects = new HashSet<GameObject>();

            GameObject gameobj;

            RayCastThrower.color = Color.red;
            hitObjects.AddRange(RayCastThrower.ThrowRayCast(transform.position.x, transform.position.y, 5, 40, 3, Vector2.left));
            hitObjects.AddRange(RayCastThrower.ThrowRayCast(transform.position.x, transform.position.y, 5, 40, 3, Vector2.right));


            gameobj = hitObjects.FirstOrDefault(i => i.tag == "Player");

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


        //void StartChasingPlayer()
        //{
        //    HashSet<GameObject> hitObjects = new HashSet<GameObject>();

        //    Vector2 rotation = isPlayerAtTheRight ? Vector2.right : Vector2.left;

        //    GameObject playerObj, gameObj;
        //    hitObjects.AddRange(RayCastThrower.ThrowRayCast(transform.position.x, transform.position.y, 3, 40, 3, rotation));

        //    playerObj = hitObjects.FirstOrDefault(i => i.tag == "Player");

        //    if (playerObj == null) return;

        //    if (playerObj.tag != "Player") return;

        //    hitObjects.Clear();

        //    hitObjects.AddRange(RayCastThrower.ThrowRayCast(transform.position.x, transform.position.y, 1, 0, 1, rotation));


        //    transform.position += new Vector3(2 * rotation.x, 0f);









        //    if (gameobj != null)
        //    {
        //        gameobj.GetComponent<PlayerMovementScript>().GetDamage(Damage);
        //    }

        //}


        IEnumerator attackPlayer()
        {

            yield return new WaitForSeconds(0.5f);

            HashSet<GameObject> hitObjects = new HashSet<GameObject>();

            Vector2 rotation = isPlayerAtTheRight ? Vector2.left : Vector2.right;

            GameObject gameobj;
            RayCastThrower.color = Color.yellow;
            hitObjects.AddRange( RayCastThrower.ThrowRayCast(transform.position.x, transform.position.y, 1, 40, 3, rotation));

            gameobj = hitObjects.FirstOrDefault(i => i.tag == "Player");

            if (gameobj != null)
            {
                gameobj.GetComponent<PlayerMovementScript>().GetDamage(Damage);
            }

            isAttacking = false;

        }




        protected virtual void Move()
        {

        }




        public virtual Loot TakeDamage(int damage)
        {
            Debug.Log("damage: " + damage  +"         health   " +Health );

            Debug.Log("Monster get hit");
            Health -= damage;

            if (Health < 0)
            {
                Destroy(gameObject);
                Debug.Log("Monster killed");

                return Loot;
            }

            return null;
        }



        protected virtual int Attack()
        {
            myAnimator.SetTrigger("attack");

            return Damage;
        }
    }
}
