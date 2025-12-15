using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FactoryView.Api.Data;
using FactoryView.Api.Entities;
using FactoryView.Api.System;
using FactoryView.Main.Local.Models;

namespace FactoryView.Main.Local.ViewModels;

public partial class LabelInfoViewModel : ObservableObject
{
    private readonly FactoryDbContext _dbContext;
    private readonly LabelInfoApi _labelInfoApi;

    [ObservableProperty]
    private ObservableCollection<SYS100Label> _labelList = new();

    [ObservableProperty]
    private SYS100Label? _selectedLabel;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _searchKeyword = string.Empty;

    [ObservableProperty]
    private string _selectedLabelType = "ALL";

    public List<string> LabelTypes { get; } = new() { "ALL", "MENU", "BUTTON", "COLUMN", "MESSAGE" };

    public LabelInfoViewModel()
    {
        _dbContext = DbContextFactory.CreateSqliteContext();
        _labelInfoApi = new LabelInfoApi(_dbContext);

        // 초기 데이터 로드
        _ = SearchAsync();
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        IsLoading = true;
        try
        {
            List<SYS100_LABELS> labels;

            if (!string.IsNullOrWhiteSpace(SearchKeyword))
            {
                labels = await _labelInfoApi.SearchAsync(SearchKeyword);
            }
            else if (SelectedLabelType != "ALL")
            {
                labels = await _labelInfoApi.GetByTypeAsync(SelectedLabelType);
            }
            else
            {
                labels = await _labelInfoApi.GetAllAsync();
            }

            LabelList.Clear();
            foreach (var entity in labels)
            {
                LabelList.Add(EntityToModel(entity));
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void Add()
    {
        var newLabel = new SYS100Label
        {
            LabelCode = $"LBL_{DateTime.Now:HHmmss}",
            LabelType = SelectedLabelType == "ALL" ? "MENU" : SelectedLabelType,
            IsNew = true
        };
        LabelList.Add(newLabel);
        SelectedLabel = newLabel;
    }

    [RelayCommand]
    private void Delete()
    {
        if (SelectedLabel == null) return;
        LabelList.Remove(SelectedLabel);
        SelectedLabel = null;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        IsLoading = true;
        try
        {
            var modifiedLabels = LabelList
                .Where(l => l.IsNew || l.IsModified)
                .Select(ModelToEntity)
                .ToList();

            if (modifiedLabels.Any())
            {
                await _labelInfoApi.SaveBatchAsync(modifiedLabels);

                // 저장 후 플래그 초기화
                foreach (var label in LabelList)
                {
                    label.IsNew = false;
                    label.IsModified = false;
                }
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    private SYS100Label EntityToModel(SYS100_LABELS entity)
    {
        return new SYS100Label
        {
            LabelCode = entity.LabelCode,
            LabelType = entity.LabelType,
            LabelKR = entity.LabelKR,
            LabelEN = entity.LabelEN,
            LabelCH = entity.LabelCH,
            LabelJP = entity.LabelJP,
            Description = entity.Description,
            IsNew = false,
            IsModified = false
        };
    }

    private SYS100_LABELS ModelToEntity(SYS100Label model)
    {
        return new SYS100_LABELS
        {
            LabelCode = model.LabelCode,
            LabelType = model.LabelType,
            LabelKR = model.LabelKR,
            LabelEN = model.LabelEN,
            LabelCH = model.LabelCH,
            LabelJP = model.LabelJP,
            Description = model.Description
        };
    }
}
