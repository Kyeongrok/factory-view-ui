# WpfLol

FactoryViewUI.dll의 컨트롤들을 WPF로 포팅하는 프로젝트입니다.

## 프로젝트 구조

```
WpfLol/
├── Controls/
│   └── RoundButton.cs        # WPF용 RoundButton 컨트롤
├── Types/
│   ├── ColorType.cs          # Blue, Orange, Green, Black, White
│   └── ButtonIconType.cs     # 아이콘 타입 (향후 확장용)
├── Themes/
│   └── Generic.xaml          # 버튼 스타일/템플릿
└── MainWindow.xaml           # 데모 화면
```

## 포팅된 컨트롤

### RoundButton

FactoryViewUI의 `RoundButton`을 WPF용으로 포팅한 컨트롤입니다.

#### 사용법

```xml
xmlns:controls="clr-namespace:WpfLol.Controls"
```

```xml
<!-- 기본 사용 -->
<controls:RoundButton Content="Search" ButtonColor="Blue" />
<controls:RoundButton Content="Add" ButtonColor="Green" />
<controls:RoundButton Content="Delete" ButtonColor="Orange" />
<controls:RoundButton Content="Cancel" ButtonColor="Black" />
<controls:RoundButton Content="White" ButtonColor="White" />

<!-- 모서리 반경 조절 -->
<controls:RoundButton Content="Rounded" CornerRadius="12" />

<!-- 비활성화 -->
<controls:RoundButton Content="Disabled" IsEnabled="False" />
```

#### 속성

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `ButtonColor` | `ColorType` | `Blue` | 버튼 색상 테마 |
| `ButtonIcon` | `ButtonIconType` | `None` | 버튼 아이콘 (향후 구현) |
| `CornerRadius` | `CornerRadius` | `4` | 모서리 반경 |

#### 색상 테마

| ColorType | Normal | Hover/Pressed |
|-----------|--------|---------------|
| Blue | `#104F89` | `#072E4F` |
| Green | `#8FC31F` | `#6B840F` |
| Orange | `#F39800` | `#BA7004` |
| Black | `#354856` | `#27353F` |
| White | `#FFFFFF` | `#D3D3D3` |

## 빌드 및 실행

```bash
dotnet build
dotnet run --project WpfLol/WpfLol.csproj
```

## 요구 사항

- .NET 8.0 (Windows)
- Visual Studio 2022 / Rider / VS Code

## 원본 소스

- `FactoryViewUI.dll` (FactoryView MES 클라이언트)
- 디컴파일 도구: ILSpyCmd

## 향후 계획

- [ ] ButtonIcon 지원 (아이콘 + 텍스트)
- [ ] MenuButton 포팅
- [ ] LabelTextEdit 등 LabelValueControl 포팅
- [ ] FvGridControl 포팅
