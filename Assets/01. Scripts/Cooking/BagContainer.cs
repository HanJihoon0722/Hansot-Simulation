// 완성된 메뉴와 수저, 물티슈를 담는 포장 봉투 컴포넌트
using System.Collections.Generic;
using System.Linq;
using Dosirak.Data;
using Dosirak.Interaction;
using UnityEngine;

namespace Dosirak.Cooking
{
    public class BagContainer : MonoBehaviour, IItemReceiver
    {
        private readonly List<Item> contents = new List<Item>();

        public IReadOnlyList<Item> Contents => contents;

        public bool CanReceive(Item item)
        {
            if (item.GetComponent<BagContainer>() != null) return false;
            var bowl = item.GetComponent<BowlContainer>();
            return bowl == null || bowl.IsComplete;
        }

        public void Receive(Item item)
        {
            item.PutInto(transform, Vector3.zero);
            item.HideAll();
            contents.Add(item);
        }

        public int CountOf(ItemData itemData) => contents.Count(i => i.Data == itemData);
    }
}
