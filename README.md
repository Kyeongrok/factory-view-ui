# WpfLol

FactoryViewUI.dll의 컨트롤들을 WPF로 포팅하는 프로젝트입니다.

## 프로젝트 구조

```
WpfLol/
├── Controls/
│   ├── RoundButton.cs        # 둥근 버튼 컨트롤
│   ├── MenuButton.cs         # 상단 메뉴 버튼 컨트롤
│   ├── MenuLabel.cs          # 소메뉴 라벨 컨트롤
│   ├── MenuLabelControl.cs   # 중메뉴 라벨 컨트롤
│   └── MenuPanel.cs          # 드롭다운 메뉴 패널
├── Types/
│   ├── ColorType.cs          # Blue, Orange, Green, Black, White
│   ├── ButtonIconType.cs     # 버튼 아이콘 타입
│   └── MenuType.cs           # 메뉴 타입 (System, Production, Quality 등)
├── Themes/
│   └── Generic.xaml          # 컨트롤 스타일/템플릿
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

---

### MenuButton

FactoryViewUI의 `MenuButton`을 WPF용으로 포팅한 컨트롤입니다. 상단 메뉴바에서 사용됩니다.

#### 사용법

```xml
<controls:MenuButton Content="System" MenuType="System" IsActive="True">
    <controls:MenuButton.Icon>
        <TextBlock Text="⚙" FontSize="24" Foreground="#B9C2D3"/>
    </controls:MenuButton.Icon>
    <controls:MenuButton.ActiveIcon>
        <TextBlock Text="⚙" FontSize="24" Foreground="White"/>
    </controls:MenuButton.ActiveIcon>
</controls:MenuButton>
```

#### 속성

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `MenuType` | `MenuType` | `System` | 메뉴 타입 |
| `IsActive` | `bool` | `false` | 활성화 상태 |
| `Icon` | `object` | `null` | 비활성 상태 아이콘 |
| `ActiveIcon` | `object` | `null` | 활성 상태 아이콘 |

#### MenuType 종류

| 주요 타입 | 설명 |
|----------|------|
| `System` | 시스템 |
| `Production` | 생산 |
| `Quality` | 품질 |
| `Material` | 자재 |
| `Purchase` | 구매 |
| `Equipment` | 설비 |
| `Sales` | 영업 |
| `Master` | 기준정보 |
| `Outsourcing` | 외주 |
| `Tool` | 공구 |

#### 색상

| 상태 | 배경색 | 글자색 |
|------|--------|--------|
| Normal | `#1F303A` | `#B9C2D3` |
| Hover/Active | `#13437C` | `#FFFFFF` |

---

### MenuLabelControl (중메뉴 라벨)

메뉴 카테고리를 표시하는 라벨입니다.

#### 사용법

```xml
<controls:MenuLabelControl Content="사용자 관리"/>
<controls:MenuLabelControl Content="권한 관리"/>
```

#### 속성

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `HoverForeground` | `Brush` | `White` | 호버 시 글자색 |
| `NormalForeground` | `Brush` | `#A99B8C` | 기본 글자색 |
| `HasChildren` | `bool` | `false` | 하위 메뉴 존재 여부 |

---

### MenuLabel (소메뉴 라벨)

드롭다운 메뉴의 개별 항목입니다.

#### 사용법

```xml
<controls:MenuLabel Content="사용자 등록"/>
<controls:MenuLabel Content="사용자 조회"/>
```

#### 속성

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `HoverForeground` | `Brush` | `#00A3FF` | 호버 시 글자색 (하늘색) |
| `NormalForeground` | `Brush` | `White` | 기본 글자색 |

---

### MenuPanel (메뉴 패널)

드롭다운 메뉴의 컨테이너입니다.

#### 사용법

```xml
<controls:MenuPanel IsOpen="True">
    <StackPanel>
        <controls:MenuLabel Content="메뉴 항목 1"/>
        <controls:MenuLabel Content="메뉴 항목 2"/>
    </StackPanel>
</controls:MenuPanel>
```

#### 속성

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `IsOpen` | `bool` | `false` | 패널 표시 여부 |

#### 색상

- 배경색: `#1F303A`
- 테두리: `#2A3F4D`

---

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
- [x] MenuButton 포팅
- [x] MenuLabel, MenuLabelControl, MenuPanel 포팅
- [ ] LabelTextEdit 등 LabelValueControl 포팅
- [ ] FvGridControl 포팅
