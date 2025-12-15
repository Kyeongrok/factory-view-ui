using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using FactoryView.Api.System;

namespace FactoryView.Forms.UI.Views;

/// <summary>
/// 설정 팝업 창
/// </summary>
public class SettingsPopup : Window
{
    private ComboBox? _languageComboBox;
    private string _selectedLanguage;

    static SettingsPopup()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SettingsPopup),
            new FrameworkPropertyMetadata(typeof(SettingsPopup)));
    }

    public SettingsPopup()
    {
        // AllowsTransparency는 Window 표시 전에 설정해야 함
        WindowStyle = WindowStyle.None;
        AllowsTransparency = true;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        ResizeMode = ResizeMode.NoResize;
        ShowInTaskbar = false;
        _selectedLanguage = LanguageService.Instance.CurrentLanguage;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // Close button
        if (GetTemplateChild("PART_CloseButton") is Button closeButton)
        {
            closeButton.Click += (s, e) => Close();
        }

        // OK button
        if (GetTemplateChild("PART_OkButton") is Button okButton)
        {
            okButton.Click += (s, e) =>
            {
                LanguageService.Instance.CurrentLanguage = _selectedLanguage;
                DialogResult = true;
                Close();
            };
        }

        // Cancel button
        if (GetTemplateChild("PART_CancelButton") is Button cancelButton)
        {
            cancelButton.Click += (s, e) =>
            {
                DialogResult = false;
                Close();
            };
        }

        // Language ComboBox
        if (GetTemplateChild("PART_LanguageComboBox") is ComboBox languageComboBox)
        {
            _languageComboBox = languageComboBox;
            _languageComboBox.ItemsSource = LanguageService.AvailableLanguages;
            _languageComboBox.DisplayMemberPath = "";

            // 현재 언어 선택
            var currentLang = LanguageService.AvailableLanguages
                .FirstOrDefault(l => l.Code == LanguageService.Instance.CurrentLanguage);
            if (currentLang != null)
            {
                _languageComboBox.SelectedItem = currentLang;
            }

            _languageComboBox.SelectionChanged += (s, e) =>
            {
                if (_languageComboBox.SelectedItem is LanguageOption option)
                {
                    _selectedLanguage = option.Code;
                }
            };
        }
    }
}
