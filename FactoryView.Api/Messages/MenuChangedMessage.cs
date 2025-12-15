using CommunityToolkit.Mvvm.Messaging.Messages;

namespace FactoryView.Api.Messages;

/// <summary>
/// 메뉴 변경 알림 메시지
/// </summary>
public class MenuChangedMessage : ValueChangedMessage<bool>
{
    public MenuChangedMessage() : base(true)
    {
    }
}
