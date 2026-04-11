using UnityEngine;

namespace Base.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data")]
    public class PlayerData : ScriptableObject
    {
        public float Speed;
        public int MaxHealth;
        public int Damage;
    }
}