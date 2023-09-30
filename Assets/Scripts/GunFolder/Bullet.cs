using Assets.Scripts.MonsterFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.GunFolder
{
    public class Bullet:NetworkBehaviour
    {
        [SerializeField]
        private int damage;

        [SerializeField]
        private float speed = 20f;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            var mousePosition = Input.mousePosition;

            var mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

            var direction = (new Vector2(mouseWorldPosition.x, mouseWorldPosition.y) - new Vector2(transform.position.x, transform.position.y)).normalized;

            GetComponent<Rigidbody2D>().velocity = direction * speed;
        }



        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player") return;

            if (other.gameObject.tag == "Monster")
            {
                Monster monster = other.gameObject.GetComponent<Monster>();

                monster.TakeDamage(damage);
            }

            Destroy(gameObject);


        }

    }
}
