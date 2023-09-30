using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.GunFolder
{
    public class Gun : NetworkBehaviour
    {
        public Guid Id { get; set; }

        [SerializeField]
        private GameObject bulletPrefab;

        [SerializeField]
        private GameObject bulletSpawnPoint;





        [ServerRpc(RequireOwnership =false)]
        public virtual void SpawnBulletsServerRpc()
        {

            var spawnPos = bulletSpawnPoint.GetComponent<Transform>();

            
            GameObject bullet = Instantiate(bulletPrefab, spawnPos.position,Quaternion.identity);
            
            bullet.transform.GetComponent<NetworkObject>().Spawn(false);
        }


    }
}
