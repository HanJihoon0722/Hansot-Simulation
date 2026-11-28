// 메뉴의 담는 순서대로만 재료를 받아 쌓는 보울 컴포넌트
using System.Collections.Generic;
using Dosirak.Data;
using Dosirak.Interaction;
using UnityEngine;

namespace Dosirak.Cooking
{
    public class BowlContainer : MonoBehaviour, IItemReceiver
    {
        [SerializeField] private MenuData menu;
        [SerializeField] private float baseHeight = 0.04f;
        [SerializeField] private float layerHeight = 0.02f;

        private readonly List<Item> contents = new List<Item>();

        public MenuData Menu => menu;
        public IReadOnlyList<Item> Contents => contents;
        public bool IsComplete => contents.Count >= menu.Steps.Count;
        public ItemData NextStep => IsComplete ? null : menu.Steps[contents.Count];

        public bool CanReceive(Item item) => !IsComplete && item.Data == NextStep;

        public void Receive(Item item)
        {
            item.PutInto(transform, Vector3.up * (baseHeight + layerHeight * contents.Count));
            contents.Add(item);
        }
    }
}
