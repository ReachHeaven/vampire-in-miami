using Game.Core;
using UnityEngine;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Config/Enemy")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private float speed;
        public float Speed => speed;
        [SerializeField] private int health;
        public int Health => health;
        [SerializeField] private int damage;
        public int Damage => damage;
        [SerializeField] private float attackInterval;
        public float AttackInterval => attackInterval;
    }
}