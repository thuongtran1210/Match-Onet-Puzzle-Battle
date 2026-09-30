using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

namespace BeastLinkBattle.InputSystem
{
    public class NewInputSystemProvider : MonoBehaviour, IInputProvider
    {
        public event Action<Vector3> OnPointerDown;
        private bool _isEnabled = true;

        [SerializeField] private Camera mainCamera;
        private GameInput _gameInput;

        private void Awake()
        {
            _gameInput = new GameInput();
            if (mainCamera == null) mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            _gameInput.Enable();
           
        }

        private void OnDisable()
        {
            _gameInput.Disable();
        }

        private void Update()
        {
            if (!_isEnabled || mainCamera == null) return;

            if (_gameInput.Gameplay.Click.WasPressedThisFrame())
            {
                if (!_isEnabled)
                {
                    Debug.LogWarning("CLICK BỊ CHẶN: Input đang bị disable bởi Game State!");
                    return;
                }

                if (IsPointerOverUI())
                {
                    Debug.LogWarning("CLICK BỊ CHẶN: Chuột đang nằm trên UI!");
                    return;
                }

                Vector2 screenPosition = _gameInput.Gameplay.Position.ReadValue<Vector2>();
                Vector3 mousePos = new Vector3(screenPosition.x, screenPosition.y, Mathf.Abs(mainCamera.transform.position.z));
                Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

                OnPointerDown?.Invoke(worldPos);
            }
        }

        public void EnableInput(bool isEnabled) => _isEnabled = isEnabled;

        /// <summary>
        /// Kiểm tra xem con trỏ hiện tại có đang nằm trên một phần tử UI hay không để tránh xung đột giữa tương tác UI và tương tác game world.
        /// </summary>
        private bool IsPointerOverUI()
        {
            if (EventSystem.current == null) return false;

            if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
            {
                int fingerId = Touchscreen.current.touches[0].touchId.ReadValue();
                return EventSystem.current.IsPointerOverGameObject(fingerId);
            }

            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}