// 재료 상자나 용기 더미처럼 집을 때마다 새 물건을 꺼내 주는 컴포넌트
using Dosirak.Data;
using Dosirak.Interaction;
using UnityEngine;

namespace Dosirak.Cooking
{
    public class Dispenser : MonoBehaviour, IGrabbable
    {
        [SerializeField] private Item itemPrefab;
        [SerializeField] private ItemData itemData;

        public ItemData ItemData => itemData;

        public Item Grab()
        {
            var item = Instantiate(itemPrefab);
            item.Init(itemData);
            return item;
        }
    }
}
