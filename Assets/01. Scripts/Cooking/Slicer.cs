// 튀긴 치킨을 넣으면 일정 시간 뒤 슬라이스로 바꿔 주는 소형 슬라이서 컴포넌트
using Dosirak.Interaction;
using UnityEngine;

namespace Dosirak.Cooking
{
    public class Slicer : MonoBehaviour, IGrabbable, IItemReceiver
    {
        [SerializeField] private Transform slot;
        [SerializeField] private float sliceSeconds = 1.5f;

        private Item current;
        private float timer;

        public string Status => current == null ? "비어 있음" : timer > 0f ? "자르는 중" : "완료";

        private void Update()
        {
            if (current == null || timer <= 0f) return;
            timer -= Time.deltaTime;
            if (timer <= 0f) current.ChangeData(current.Data.SlicedResult);
        }

        public bool CanReceive(Item item) => current == null && item.Data.SlicedResult != null;

        public void Receive(Item item)
        {
            current = item;
            timer = sliceSeconds;
            item.PutInto(slot, Vector3.zero);
        }

        public Item Grab()
        {
            if (current == null || timer > 0f) return null;
            var item = current;
            current = null;
            return item;
        }
    }
}
