// 시선 레이캐스트로 대상을 찾고 집기, 넣기, 내려놓기, 사용을 처리하는 컴포넌트
using System.Collections.Generic;
using Dosirak.Cooking;
using Dosirak.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dosirak.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private InputActionAsset actions;
        [SerializeField] private Camera cam;
        [SerializeField] private Transform holdPoint;
        [SerializeField] private PlayerController controller;
        [SerializeField] private float reach = 2.5f;
        [SerializeField] private float placeableNormalY = 0.7f;

        private InputAction grab;
        private InputAction use;
        private readonly List<string> promptParts = new List<string>();

        public Item Held { get; private set; }
        public GameObject Target { get; private set; }
        public string Prompt { get; private set; } = "";

        private void Awake()
        {
            var map = actions.FindActionMap("Player", true);
            grab = map.FindAction("Grab", true);
            use = map.FindAction("Use", true);
        }

        private void Update()
        {
            Target = null;
            Prompt = "";
            if (!controller.IsActive) return;
            if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out var hit, reach)) return;

            Target = hit.collider.gameObject;
            var grabbable = hit.collider.GetComponentInParent<IGrabbable>();
            var receiver = hit.collider.GetComponentInParent<IItemReceiver>();
            var interactable = hit.collider.GetComponentInParent<IInteractable>();
            var canPlace = hit.normal.y > placeableNormalY;

            BuildPrompt(grabbable, receiver, interactable, canPlace);

            if (grab.WasPressedThisFrame()) HandleGrab(grabbable, receiver, canPlace, hit.point);
            if (use.WasPressedThisFrame()) interactable?.Use();
        }

        private void HandleGrab(IGrabbable grabbable, IItemReceiver receiver, bool canPlace, Vector3 point)
        {
            if (Held == null)
            {
                var item = grabbable?.Grab();
                if (item == null) return;
                Held = item;
                Held.Hold(holdPoint);
            }
            else if (receiver != null && receiver.CanReceive(Held))
            {
                var item = Held;
                Held = null;
                receiver.Receive(item);
            }
            else if (canPlace)
            {
                Held.PlaceAt(point);
                Held = null;
            }
        }

        private void BuildPrompt(IGrabbable grabbable, IItemReceiver receiver, IInteractable interactable, bool canPlace)
        {
            promptParts.Clear();
            var grabKey = grab.GetBindingDisplayString();
            if (Held == null)
            {
                if (grabbable != null) promptParts.Add($"[{grabKey}] 집기");
            }
            else if (receiver != null && receiver.CanReceive(Held)) promptParts.Add($"[{grabKey}] 넣기");
            else if (canPlace) promptParts.Add($"[{grabKey}] 내려놓기");

            if (interactable != null) promptParts.Add($"[{use.GetBindingDisplayString()}] {interactable.UseLabel}");
            Prompt = string.Join("    ", promptParts);
        }
    }
}
