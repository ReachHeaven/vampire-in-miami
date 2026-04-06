using Game.Core;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        private EnemyModel _model;

        private void Start()
        {
            _model = GetComponent<EnemyMovement>().Model;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (!_model.TryAttack(Time.deltaTime)) return;
            if (!other.collider.TryGetComponent(out Health health)) return;
            health.TakeDamage(_model.Damage);
        }
    }
}
