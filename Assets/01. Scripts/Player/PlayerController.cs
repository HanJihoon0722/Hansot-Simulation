// 1인칭 이동, 시선 회전, 커서 잠금을 처리하는 플레이어 컴포넌트
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dosirak.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset actions;
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float lookSensitivity = 0.1f;
        [SerializeField] private float gravity = -9.81f;

        private CharacterController controller;
        private InputActionMap map;
        private InputAction move;
        private InputAction look;
        private InputAction toggleCursor;
        private float pitch;
        private float verticalSpeed;

        public bool IsActive => Cursor.lockState == CursorLockMode.Locked;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            map = actions.FindActionMap("Player", true);
            move = map.FindAction("Move", true);
            look = map.FindAction("Look", true);
            toggleCursor = map.FindAction("ToggleCursor", true);
        }

        private void OnEnable()
        {
            map.Enable();
            SetCursorLocked(true);
        }

        private void OnDisable()
        {
            map.Disable();
            SetCursorLocked(false);
        }

        private void Update()
        {
            if (toggleCursor.WasPressedThisFrame()) SetCursorLocked(!IsActive);
            if (!IsActive) return;

            var lookDelta = look.ReadValue<Vector2>() * lookSensitivity;
            transform.Rotate(0f, lookDelta.x, 0f);
            pitch = Mathf.Clamp(pitch - lookDelta.y, -85f, 85f);
            cameraRoot.localEulerAngles = new Vector3(pitch, 0f, 0f);

            var input = move.ReadValue<Vector2>();
            var velocity = (transform.right * input.x + transform.forward * input.y) * moveSpeed;
            verticalSpeed = controller.isGrounded ? -1f : verticalSpeed + gravity * Time.deltaTime;
            velocity.y = verticalSpeed;
            controller.Move(velocity * Time.deltaTime);
        }

        private static void SetCursorLocked(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }
    }
}
