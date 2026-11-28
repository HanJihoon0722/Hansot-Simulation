// 손에 들 수 있는 물건 하나의 상태(익힘 시간, 온도)와 위치 이동을 관리하는 컴포넌트
using Dosirak.Data;
using Dosirak.Interaction;
using UnityEngine;

namespace Dosirak.Cooking
{
    [RequireComponent(typeof(BoxCollider))]
    public class Item : MonoBehaviour, IGrabbable
    {
        [SerializeField] private ItemData data;
        [SerializeField] private Transform visual;

        private BoxCollider box;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private CookProfileData profile;
        private float hotTimer;

        public ItemData Data => data;
        public float CookSeconds { get; private set; }
        public float Temperature { get; private set; }
        public bool IsCookable => profile != null;
        public Doneness Doneness => profile != null ? profile.Evaluate(CookSeconds) : Doneness.Good;

        private void Awake()
        {
            box = GetComponent<BoxCollider>();
            meshFilter = visual.GetComponent<MeshFilter>();
            meshRenderer = visual.GetComponent<MeshRenderer>();
            if (data != null) Init(data);
        }

        public void Init(ItemData itemData)
        {
            CookSeconds = 0f;
            hotTimer = 0f;
            Temperature = itemData.StartsHot ? 1f : 0f;
            profile = itemData.CookProfile;
            ApplyData(itemData);
        }

        // 슬라이스처럼 종류만 바뀔 때 쓴다. 익힘과 온도는 그대로 유지한다.
        public void ChangeData(ItemData itemData)
        {
            if (itemData.CookProfile != null) profile = itemData.CookProfile;
            ApplyData(itemData);
        }

        public void Cook(float deltaTime)
        {
            CookSeconds += deltaTime;
            Temperature = 1f;
            hotTimer = 0f;
            RefreshColor();
        }

        public Item Grab() => this;

        public void Hold(Transform holdPoint)
        {
            transform.SetParent(holdPoint, false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            box.enabled = false;
        }

        public void PlaceAt(Vector3 point)
        {
            transform.SetParent(null, true);
            var bottomOffset = box.size.y * 0.5f - box.center.y;
            transform.SetPositionAndRotation(point + Vector3.up * bottomOffset, Quaternion.identity);
            box.enabled = true;
        }

        public void PutInto(Transform parent, Vector3 localPosition)
        {
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localRotation = Quaternion.identity;
            box.enabled = false;
        }

        public void HideAll()
        {
            foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = false;
        }

        private void Update()
        {
            if (data == null || !data.UsesTemperature || Temperature <= 0f) return;
            hotTimer += Time.deltaTime;
            if (hotTimer > data.HoldSeconds)
                Temperature = Mathf.Max(0f, Temperature - data.CoolPerSecond * Time.deltaTime);
        }

        private void ApplyData(ItemData itemData)
        {
            data = itemData;
            name = itemData.DisplayName;
            meshFilter.sharedMesh = itemData.Mesh;
            visual.localScale = itemData.Scale;
            var bounds = itemData.Mesh.bounds;
            box.center = Vector3.Scale(bounds.center, itemData.Scale);
            box.size = Vector3.Scale(bounds.size, itemData.Scale);
            RefreshColor();
        }

        private void RefreshColor()
        {
            meshRenderer.material.color = profile != null ? profile.ColorAt(CookSeconds) : data.Color;
        }
    }
}
