// 빈손으로 집었을 때 손에 들 물건을 내주는 대상의 인터페이스
using Dosirak.Cooking;

namespace Dosirak.Interaction
{
    public interface IGrabbable
    {
        // 손에 들 물건을 돌려준다. 지금 줄 수 없으면 null.
        Item Grab();
    }
}
