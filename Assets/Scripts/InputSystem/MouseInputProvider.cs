using UnityEngine;
using System;

namespace BeastLinkBattle.InputSystem
{
    // MouseInputProvider là một MonoBehaviour thực hiện giao diện IInputProvider để cung cấp đầu vào chuột cho trò chơi.
    // Nó lắng nghe sự kiện nhấn chuột trái và chuyển đổi vị trí chuột từ tọa độ màn hình sang tọa độ thế giới, 
    // sau đó phát ra sự kiện OnPointerDown với vị trí thế giới đó.
    // Ngoài ra, nó cũng có khả năng bật hoặc tắt việc nhận đầu vào thông qua phương thức EnableInput, 
    // cho phép linh hoạt kiểm soát khi nào người chơi có thể tương tác với trò chơi bằng chuột.
    public class MouseInputProvider : MonoBehaviour, IInputProvider
    {
        public event Action<Vector3> OnPointerDown;
        private bool _isEnabled = true;

        [SerializeField] private Camera mainCamera;

        private void Start()
        {
            if (mainCamera == null) mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!_isEnabled) return;

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Mathf.Abs(mainCamera.transform.position.z);
                Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

                OnPointerDown?.Invoke(worldPos);
            }
        }

        public void EnableInput(bool isEnabled) => _isEnabled = isEnabled;
    }
}