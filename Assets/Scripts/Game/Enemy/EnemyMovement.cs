using UnityEngine;

namespace Game.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private int damage = 30;
        [SerializeField] private float attackInterval = 1f;

        public EnemyModel Model { get; private set; }

        private Transform _target;
        private Rigidbody2D _rb;

        public void Init(Transform target)
        {
            _target = target;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            Model = new EnemyModel(speed, damage, attackInterval);
        }

        private void FixedUpdate()
        {
            if (!_target) return;
            Vector2 direction = ((Vector2)_target.position - (Vector2)transform.position).normalized;
            _rb.MovePosition(_rb.position + direction * (Model.Speed * Time.fixedDeltaTime));
        }
    }
}
