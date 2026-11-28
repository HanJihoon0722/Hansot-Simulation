// 바라보고 사용 키로 조작하는 기구의 인터페이스
namespace Dosirak.Interaction
{
    public interface IInteractable
    {
        string UseLabel { get; }
        void Use();
    }
}
