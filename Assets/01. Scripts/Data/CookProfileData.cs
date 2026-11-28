// 조리 시간에 따른 익힘 구간(덜 익음, 적당함, 탐)과 색 변화를 정의하는 데이터
using UnityEngine;

namespace Dosirak.Data
{
    public enum Doneness { Raw, Good, Burnt }

    [CreateAssetMenu(menuName = "Dosirak/Cook Profile")]
    public class CookProfileData : ScriptableObject
    {
        [SerializeField] private float goodFrom = 6f;
        [SerializeField] private float burntFrom = 12f;
        [SerializeField] private float burnFadeSeconds = 4f;
        [SerializeField] private Color rawColor = new Color(0.93f, 0.85f, 0.75f);
        [SerializeField] private Color goodColor = new Color(0.79f, 0.54f, 0.24f);
        [SerializeField] private Color burntColor = new Color(0.25f, 0.15f, 0.08f);

        public Doneness Evaluate(float cookSeconds)
        {
            if (cookSeconds < goodFrom) return Doneness.Raw;
            return cookSeconds < burntFrom ? Doneness.Good : Doneness.Burnt;
        }

        public Color ColorAt(float cookSeconds)
        {
            if (cookSeconds < goodFrom) return Color.Lerp(rawColor, goodColor, cookSeconds / goodFrom);
            if (cookSeconds < burntFrom) return goodColor;
            return Color.Lerp(goodColor, burntColor, (cookSeconds - burntFrom) / burnFadeSeconds);
        }
    }
}
