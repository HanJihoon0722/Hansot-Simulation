// 바구니를 기름에 내리고 올려 재료를 튀기는 튀김기 컴포넌트
using System.Collections.Generic;
using Dosirak.Interaction;
using UnityEngine;

namespace Dosirak.Cooking
{
    public class Fryer : MonoBehaviour, IGrabbable, IItemReceiver, IInteractable
    {
        [SerializeField] private Transform basket;
        [SerializeField] private int capacity = 4;
        [SerializeField] private float lowerDepth = 0.25f;
        [SerializeField] private float itemSpacing = 0.1f;
        [SerializeField] private float itemHeight = 0.04f;

        private readonly List<Item> items = new List<Item>();
        private Vector3 raisedPosition;

        public bool IsLowered { get; private set; }
        public int Count => items.Count;
        public string UseLabel => IsLowered ? "바구니 올리기" : "바구니 내리기";

        private void Awake()
        {
            raisedPosition = basket.localPosition;
        }

        private void Update()
        {
            if (!IsLowered) return;
            foreach (var item in items) item.Cook(Time.deltaTime);
        }

        public void Use()
        {
            IsLowered = !IsLowered;
            basket.localPosition = raisedPosition + Vector3.down * (IsLowered ? lowerDepth : 0f);
        }

        public bool CanReceive(Item item) =>
            !IsLowered && items.Count < capacity && item.Data.CookProfile != null;

        public void Receive(Item item)
        {
            items.Add(item);
            Layout();
        }

        public Item Grab()
        {
            if (IsLowered || items.Count == 0) return null;
            var item = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            return item;
        }

        private void Layout()
        {
            for (var i = 0; i < items.Count; i++)
            {
                var x = (i % 2 - 0.5f) * itemSpacing;
                var z = (i / 2 - 0.5f) * itemSpacing;
                items[i].PutInto(basket, new Vector3(x, itemHeight, z));
            }
        }
    }
}
