// 재료, 용기, 수저 같은 물건 한 종류의 이름, 모양, 온도 규칙을 정의하는 데이터
using UnityEngine;

namespace Dosirak.Data
{
    [CreateAssetMenu(menuName = "Dosirak/Item")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private Mesh mesh;
        [SerializeField] private Vector3 scale = Vector3.one * 0.1f;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private bool usesTemperature;
        [SerializeField] private bool startsHot;
        [SerializeField] private float holdSeconds = 20f;
        [SerializeField] private float coolPerSecond = 0.02f;
        [SerializeField] private CookProfileData cookProfile;
        [SerializeField] private ItemData slicedResult;

        public string DisplayName => displayName;
        public Mesh Mesh => mesh;
        public Vector3 Scale => scale;
        public Color Color => color;
        public bool UsesTemperature => usesTemperature;
        public bool StartsHot => startsHot;
        public float HoldSeconds => holdSeconds;
        public float CoolPerSecond => coolPerSecond;
        public CookProfileData CookProfile => cookProfile;
        public ItemData SlicedResult => slicedResult;
    }
}
