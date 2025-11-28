using System.Collections.ObjectModel;
using System.Windows;

namespace WpfLol;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        LoadSampleData();
    }

    private void LoadSampleData()
    {
        var sampleData = new ObservableCollection<SampleEmployee>
        {
            new() { Id = 1, Name = "홍길동", Department = "개발팀", Position = "과장", HireDate = "2020-03-15" },
            new() { Id = 2, Name = "김영희", Department = "영업팀", Position = "대리", HireDate = "2021-07-01" },
            new() { Id = 3, Name = "이철수", Department = "개발팀", Position = "사원", HireDate = "2022-01-10" },
            new() { Id = 4, Name = "박민수", Department = "품질팀", Position = "차장", HireDate = "2018-05-20" },
            new() { Id = 5, Name = "정수진", Department = "생산팀", Position = "부장", HireDate = "2015-11-03" },
        };
        demoGrid.ItemsSource = sampleData;
    }
}

public class SampleEmployee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string HireDate { get; set; } = string.Empty;
}