// 주문대에서 봉투를 받아 익힘, 온도, 포장을 평가해 별점을 매기는 손님 컴포넌트
using System.Linq;
using Dosirak.Cooking;
using Dosirak.Data;
using Dosirak.Interaction;
using UnityEngine;

namespace Dosirak.Orders
{
    public class Customer : MonoBehaviour, IItemReceiver
    {
        [SerializeField] private MenuData order;
        [SerializeField] private float nextCustomerDelay = 3f;
        [SerializeField] private float donenessWeight = 0.5f;
        [SerializeField] private float temperatureWeight = 0.3f;
        [SerializeField] private float packagingWeight = 0.2f;
        [SerializeField] private float rawScore = 0.4f;
        [SerializeField] private float burntScore = 0.2f;

        private bool waiting = true;
        private int served;
        private int totalStars;

        public MenuData Order => waiting ? order : null;
        public string LastResult { get; private set; }
        public int Served => served;
        public float AverageStars => served > 0 ? (float)totalStars / served : 0f;

        public bool CanReceive(Item item) => waiting && item.GetComponent<BagContainer>() != null;

        public void Receive(Item item)
        {
            var stars = Evaluate(item.GetComponent<BagContainer>(), out var detail);
            LastResult = $"별 {stars}개. {detail}";
            served++;
            totalStars += stars;
            Destroy(item.gameObject);
            waiting = false;
            Invoke(nameof(NextCustomer), nextCustomerDelay);
        }

        private void NextCustomer() => waiting = true;

        private int Evaluate(BagContainer bag, out string detail)
        {
            var bowl = bag.Contents
                .Select(i => i.GetComponent<BowlContainer>())
                .FirstOrDefault(b => b != null && b.Menu == order && b.IsComplete);
            if (bowl == null)
            {
                detail = "주문한 메뉴가 봉투에 없다.";
                return 1;
            }

            float doneSum = 0f, tempSum = 0f;
            int doneCount = 0, tempCount = 0;
            foreach (var food in bowl.Contents)
            {
                if (food.IsCookable)
                {
                    doneCount++;
                    doneSum += food.Doneness == Doneness.Good ? 1f : food.Doneness == Doneness.Raw ? rawScore : burntScore;
                }
                if (food.Data.UsesTemperature)
                {
                    tempCount++;
                    tempSum += food.Temperature;
                }
            }

            var doneScore = doneCount > 0 ? doneSum / doneCount : 1f;
            var tempScore = tempCount > 0 ? tempSum / tempCount : 1f;
            var packOk = order.Packaging.All(p => bag.CountOf(p.Item) == p.Count);
            var total = doneScore * donenessWeight + tempScore * temperatureWeight + (packOk ? packagingWeight : 0f);

            detail = $"익힘 {doneScore:P0}, 온도 {tempScore:P0}, 포장 {(packOk ? "정확" : "틀림")}";
            return 1 + Mathf.RoundToInt(Mathf.Clamp01(total) * 4f);
        }
    }
}
