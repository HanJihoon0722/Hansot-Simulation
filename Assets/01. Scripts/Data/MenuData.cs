// 메뉴 하나의 담는 순서와 포장 구성(수저, 물티슈 개수)을 정의하는 데이터
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dosirak.Data
{
    [Serializable]
    public struct ItemCount
    {
        [SerializeField] private ItemData item;
        [SerializeField] private int count;

        public ItemData Item => item;
        public int Count => count;
    }

    [CreateAssetMenu(menuName = "Dosirak/Menu")]
    public class MenuData : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private List<ItemData> steps = new List<ItemData>();
        [SerializeField] private List<ItemCount> packaging = new List<ItemCount>();

        public string DisplayName => displayName;
        public IReadOnlyList<ItemData> Steps => steps;
        public IReadOnlyList<ItemCount> Packaging => packaging;
    }
}
