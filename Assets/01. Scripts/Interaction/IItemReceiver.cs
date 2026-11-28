// 손에 든 물건을 받아 담는 대상의 인터페이스
using Dosirak.Cooking;

namespace Dosirak.Interaction
{
    public interface IItemReceiver
    {
        bool CanReceive(Item item);
        void Receive(Item item);
    }
}
