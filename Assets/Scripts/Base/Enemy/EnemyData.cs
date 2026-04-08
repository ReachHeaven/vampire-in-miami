using UnityEngine;

namespace Base.Enemy
{
    public class EnemyData : ScriptableObject
    {
        public float Speed;
        public int MaxHealth;
        public int Damage;
        public Enemy EnemyPrefab;
    }
}