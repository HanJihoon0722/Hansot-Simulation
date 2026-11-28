// PoC 확인용으로 조준점, 대상 정보, 손에 든 물건, 주문과 결과를 IMGUI로 그리는 임시 HUD
using System.Collections.Generic;
using System.Linq;
using Dosirak.Cooking;
using Dosirak.Data;
using Dosirak.Orders;
using Dosirak.Player;
using UnityEngine;

namespace Dosirak.PoC
{
    public class PoCHud : MonoBehaviour
    {
        [SerializeField] private PlayerInteractor interactor;
        [SerializeField] private Customer customer;
        [SerializeField] private Camera cam;
        [SerializeField] private List<Collider> labelTargets = new List<Collider>();
        [SerializeField] private float labelMaxDistance = 8f;
        [SerializeField] private int fontSize = 20;
        [SerializeField] private int labelFontSize = 18;
        [SerializeField] private float referenceHeight = 1080f;

        private GUIStyle style;
        private GUIStyle centered;
        private GUIStyle labelStyle;

        private void OnGUI()
        {
            if (style == null)
            {
                style = new GUIStyle(GUI.skin.box) { fontSize = fontSize, alignment = TextAnchor.UpperLeft, wordWrap = true };
                centered = new GUIStyle(GUI.skin.label) { fontSize = fontSize, alignment = TextAnchor.MiddleCenter };
                labelStyle = new GUIStyle(GUI.skin.box) { fontSize = labelFontSize, alignment = TextAnchor.MiddleCenter };
            }

            // 1080p 기준으로 그리고 실제 해상도에 맞춰 확대한다.
            var scale = Screen.height / referenceHeight;
            GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1f));
            var w = Screen.width / scale;
            var h = referenceHeight;

            DrawWorldLabels(scale);
            GUI.Label(new Rect(w / 2f - 20, h / 2f - 20, 40, 40), "+", centered);

            var target = Describe(interactor.Target);
            if (target.Length > 0 || interactor.Prompt.Length > 0)
                GUI.Box(new Rect(w / 2f - 300, h - 120, 600, 90), $"{target}\n{interactor.Prompt}", style);

            GUI.Box(new Rect(20, 20, 380, 200), OrderText(), style);

            if (interactor.Held != null)
                GUI.Box(new Rect(w - 400, 20, 380, 90), $"손에 든 것\n{Describe(interactor.Held.gameObject)}", style);
        }

        // 기구와 상자 위에 한글 이름표를 띄운다.
        private void DrawWorldLabels(float scale)
        {
            foreach (var target in labelTargets)
            {
                var bounds = target.bounds;
                var top = bounds.center + Vector3.up * (bounds.extents.y + 0.08f);
                var screen = cam.WorldToScreenPoint(top);
                if (screen.z <= 0f || screen.z > labelMaxDistance) continue;

                var content = new GUIContent(target.name);
                var size = labelStyle.CalcSize(content);
                var x = screen.x / scale;
                var y = (Screen.height - screen.y) / scale;
                GUI.Box(new Rect(x - size.x / 2f, y - size.y, size.x, size.y), content, labelStyle);
            }
        }

        private string OrderText()
        {
            var text = customer.Order == null
                ? "다음 손님을 기다리는 중"
                : $"주문. {customer.Order.DisplayName} 1개\n담는 순서. {string.Join(" > ", customer.Order.Steps.Select(s => s.DisplayName))}\n포장. {string.Join(", ", customer.Order.Packaging.Select(p => $"{p.Item.DisplayName} {p.Count}"))}";
            if (customer.LastResult != null)
                text += $"\n\n지난 결과. {customer.LastResult}\n평균 별점 {customer.AverageStars:0.0} ({customer.Served}명)";
            return text;
        }

        private static string Describe(GameObject go)
        {
            if (go == null) return "";
            if (go.TryGetComponent<BowlContainer>(out var bowl))
                return bowl.IsComplete ? $"{bowl.Menu.DisplayName} 완성" : $"보울. 다음 재료는 {bowl.NextStep.DisplayName}";
            if (go.TryGetComponent<BagContainer>(out var bag))
                return bag.Contents.Count == 0 ? "봉투. 비어 있음" : $"봉투. {string.Join(", ", bag.Contents.Select(i => i.Data.DisplayName))}";
            if (go.TryGetComponent<Item>(out var item)) return DescribeItem(item);
            if (go.TryGetComponent<Fryer>(out var fryer))
                return $"튀김기. {fryer.Count}개, {(fryer.IsLowered ? "튀기는 중" : "바구니 올림")}";
            if (go.TryGetComponent<Slicer>(out var slicer)) return $"치킨 슬라이서. {slicer.Status}";
            if (go.TryGetComponent<Customer>(out _)) return "손님";
            return go.name;
        }

        private static string DescribeItem(Item item)
        {
            var text = item.Data.DisplayName;
            if (item.IsCookable) text += $". 익힘 {DonenessLabel(item.Doneness)} ({item.CookSeconds:0.0}초)";
            if (item.Data.UsesTemperature) text += $". 온도 {item.Temperature:P0}";
            return text;
        }

        private static string DonenessLabel(Doneness doneness) => doneness switch
        {
            Doneness.Raw => "덜 익음",
            Doneness.Good => "적당함",
            _ => "탐",
        };
    }
}
