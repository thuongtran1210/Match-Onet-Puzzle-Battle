using UnityEngine;
using BeastLinkBattle.Gameplay.Battle.Models;
using System;
using BeastLinkBattle.Gameplay.Data;
using DG.Tweening;
using BeastLinkBattle.UI.Views;
using BeastLinkBattle.Core;
using BeastLinkBattle.Gameplay.Data.Skills;

namespace BeastLinkBattle.Gameplay.Battle.Entities
{
    public class BeastEntity : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ParticleSystem _attackVfx;
        [SerializeField] private Animator _animator;

        [Header("Visual Feedback Settings")]
        [Tooltip("SpriteRenderer của Thú")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Tooltip("Prefab cho hiệu ứng hiển thị sát thương (nên là một TextMeshPro hoặc UI element)")]
        [SerializeField] private GameObject _damageTextPrefab;
        [Tooltip("Vị trí spawn hiệu ứng sát thương so với vị trí của Thú (ví dụ: Vector3(0, 1.5f, 0) để hiển thị trên đầu)")]
        [SerializeField] private Vector3 _textOffset = new Vector3(0, 1.5f, 0);
        [Tooltip("Điểm spawn hiệu ứng số (Floating Text)")]
        [SerializeField] private Transform _floatingTextSpawnPoint;

        private BaseEntityModel _model;
        private Vector3 _smoothVelocity;
        private Color _originalColor = Color.white; 

        public void Bind(BaseEntityModel model)
        {
            _model = model;
            _model.OnAttack += HandleAttack;
            _model.OnTakeDamage += HandleHitAnim;
            _model.OnDie += HandleDeath;
            if (_model is PlayerBeastModel playerBeast)
            {
                playerBeast.OnAutoSkillReady += HandleSkillCast;
            }
            if (_spriteRenderer != null) _originalColor = _spriteRenderer.color;
            ApplyElementVisuals(_model.Element);
            ApplyRoleSpecificLogic(_model.Role);
        }
        private void Update()
        {
            if (_model == null || !_model.IsAlive) return;
            transform.position = Vector3.SmoothDamp(
                transform.position,
                _model.Position,
                ref _smoothVelocity,
                0.1f
            );
            if (_model.CurrentTarget != null)
            {
                Vector3 lookDir = _model.CurrentTarget.Position - _model.Position;
                if (lookDir.x < -0.1f) transform.localScale = new Vector3(-1, 1, 1);
                else if (lookDir.x > 0.1f) transform.localScale = new Vector3(1, 1, 1);
            }
            else if (Mathf.Abs(_smoothVelocity.x) > 0.05f)
            {
                if (_smoothVelocity.x < -0.05f) transform.localScale = new Vector3(-1, 1, 1);
                else if (_smoothVelocity.x > 0.05f) transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                if (_model.MoveDirection.x < -0.1f) transform.localScale = new Vector3(-1, 1, 1);
                else if (_model.MoveDirection.x > 0.1f) transform.localScale = new Vector3(1, 1, 1);
            }
            if (_animator)
            {
                bool isMoving = _model.CurrentState == EntityState.Moving || _model.CurrentState == EntityState.Retreating;
                _animator.SetBool("IsMoving", isMoving);
            }
        }
        private void HandleSkillCast(SkillDefinition skill, PlayerBeastModel model)
        {
            if (_animator) _animator.SetTrigger("CastSkill"); // Tên trigger trong Animator

            // Nếu Skill có vfxPrefab, bạn có thể Instantiate nó tại đây
            if (skill.vfxPrefab != null)
            {
                PoolManager.Instance.Get(skill.vfxPrefab, transform.position, Quaternion.identity);
            }

            Debug.Log($"Beast {model.Definition.displayName} tung chiêu {skill.skillName}!");
        }
        private void ApplyElementVisuals(ElementType element) { /* Tùy ch?n h? */ }
        private void ApplyRoleSpecificLogic(RoleType role) { /* Tùy ch?n Role */ }

        private void HandleAttack()
        {
            if (_animator) _animator.SetTrigger("Attack");
            if (_attackVfx) _attackVfx.Play();
        }
        private void HandleHitAnim(int damage)
        {
            if (_damageTextPrefab != null)
            {
                Vector3 spawnPos = transform.position + _textOffset;

                GameObject textObj = PoolManager.Instance.Get(_damageTextPrefab, spawnPos, Quaternion.identity);

                if (textObj.TryGetComponent(out DamagePopup popup))
                {
                    popup.Setup(damage, Color.red);
                }
            }

            if (_spriteRenderer != null)
            {
                _spriteRenderer.DOKill(); 
                _spriteRenderer.color = Color.red;
                _spriteRenderer.DOColor(_originalColor, 0.2f).SetEase(Ease.OutCubic);
            }

            float pushBackX = transform.localScale.x > 0 ? -0.2f : 0.2f;
            transform.GetChild(0).DOKill(true);
            transform.GetChild(0).DOPunchPosition(new Vector3(pushBackX, 0, 0), 0.25f, 10, 1f);
        }

        private void HandleDeath()
        {
            if (_animator) _animator.SetTrigger("Die");

            _model.OnAttack -= HandleAttack;
            _model.OnTakeDamage -= HandleHitAnim;
            _model.OnDie -= HandleDeath;

            if (_spriteRenderer != null)
            {
                _spriteRenderer.DOFade(0f, 1.0f).SetEase(Ease.OutQuad);
            }
            if (_model is PlayerBeastModel playerBeast)
            {
                playerBeast.OnAutoSkillReady -= HandleSkillCast;
            }

            Destroy(gameObject, 1.5f);
        }
    }
}