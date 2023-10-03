
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MonsterFolder
{
    [CreateAssetMenu(fileName = "New Loot", menuName = "Loot")]
    public class Loot : ScriptableObject
    {

        [SerializeField] private int experience;

        [SerializeField] private int gold;

        [SerializeField] private GameObject gun;

     
    }
}
