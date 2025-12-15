using CommunityToolkit.Mvvm.Messaging;

namespace FactoryView.Api.System;

/// <summary>
/// 언어 설정 서비스 (Singleton)
/// </summary>
public class LanguageService
{
    private static LanguageService? _instance;
    private static readonly object _lock = new();

    public static LanguageService Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new LanguageService();
                }
            }
            return _instance;
        }
    }

    private string _currentLanguage = "KR";

    /// <summary>현재 언어 (KR, EN, CH, JP)</summary>
    public string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage != value)
            {
                _currentLanguage = value;
                // 언어 변경 메시지 발송
                WeakReferenceMessenger.Default.Send(new LanguageChangedMessage(value));
            }
        }
    }

    /// <summary>사용 가능한 언어 목록</summary>
    public static readonly List<LanguageOption> AvailableLanguages = new()
    {
        new LanguageOption { Code = "KR", DisplayName = "한국어", Flag = "🇰🇷" },
        new LanguageOption { Code = "EN", DisplayName = "English", Flag = "🇺🇸" },
        new LanguageOption { Code = "CH", DisplayName = "中文", Flag = "🇨🇳" },
        new LanguageOption { Code = "JP", DisplayName = "日本語", Flag = "🇯🇵" }
    };

    private LanguageService() { }
}

/// <summary>
/// 언어 옵션
/// </summary>
public class LanguageOption
{
    public string Code { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;

    public override string ToString() => $"{Flag} {DisplayName}";
}

/// <summary>
/// 언어 변경 메시지
/// </summary>
public class LanguageChangedMessage
{
    public string Language { get; }

    public LanguageChangedMessage(string language)
    {
        Language = language;
    }
}
