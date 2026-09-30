using UnityEngine;
using BeastLinkBattle.Gameplay.Battle.Models;
using BeastLinkBattle.Core;

namespace BeastLinkBattle.Gameplay.Battle.Entities
{
    public class ProjectileEntity : MonoBehaviour
    {
        [Header("VFX Settings")]
        [Tooltip("Hiệu ứng khi đạn trúng mục tiêu")]
        [SerializeField] private ParticleSystem _hitVfx;

        private BaseEntityModel _target;
        private int _damage;
        private float _speed;
        private bool _isFired = false;

 // Tương lai có thể thêm các tính năng như xuyên giáp, hiệu ứng trạng thái, v.v. vào ProjectileEntity này

        // Phương thức này sẽ được gọi ngay sau khi Instantiate để thiết lập mục tiêu, sát thương và tốc độ của đạn
        public void Setup(BaseEntityModel target, int damage, float speed)
        {
            _target = target;
            _damage = damage;
            _speed = speed;
            _isFired = true;

            // Xoay đạn hướng về mục tiêu ngay khi bắn ra
            RotateTowardsTarget();
        }

        private void Update()
        {
            if (!_isFired) return;

            if (_target == null || !_target.IsAlive)
            {
                PoolManager.Instance.Release(gameObject);
                return;
            }
            Vector3 dir = _target.Position - transform.position;
            float distanceThisFrame = _speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
            }
            else
            {
                transform.Translate(dir.normalized * distanceThisFrame, Space.World);
                RotateTowardsTarget(); 
            }
        }

        private void HitTarget()
        {
            _isFired = false;
            if (_target != null && _target.IsAlive) _target.TakeDamage(_damage);

            if (_hitVfx != null)
            {
                _hitVfx.transform.SetParent(null);
                _hitVfx.Play();
                Destroy(_hitVfx.gameObject, _hitVfx.main.duration);
            }
            PoolManager.Instance.Release(gameObject);
        }

        private void RotateTowardsTarget()
        {
            if (_target == null) return;
            Vector3 dir = (_target.Position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}