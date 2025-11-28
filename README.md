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
│   ├── MenuPanel.cs          # 드롭다운 메뉴 패널
│   ├── LabelControlBase.cs   # LabelValueControl 공통 베이스
│   ├── LabelTextEdit.cs      # 라벨 + 텍스트 입력
│   ├── LabelLookUpEdit.cs    # 라벨 + 콤보박스
│   ├── LabelDateEdit.cs      # 라벨 + 날짜 선택
│   ├── LabelCheckEdit.cs     # 라벨 + 체크박스
│   ├── LabelRadioGroup.cs    # 라벨 + 라디오버튼 그룹
│   ├── LabelDateEditBetween.cs # 라벨 + 시작/종료 날짜
│   ├── FvGridControl.cs      # 데이터 그리드 컨트롤
│   └── AccordionControl.cs   # 아코디언 메뉴 컨트롤
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

### LabelValueControl 시리즈

FactoryViewUI의 `LabelCommon` 기반 컨트롤들을 WPF로 포팅한 입력 컨트롤입니다.
라벨 + 입력 컨트롤 조합으로, 일관된 레이아웃을 제공합니다.

#### 공통 속성 (LabelControlBase)

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `Caption` | `string` | `""` | 라벨 텍스트 |
| `CaptionWidth` | `double` | `160` | 라벨 너비 |
| `IsRequired` | `bool` | `false` | 필수 입력 여부 (* 표시) |
| `FieldName` | `string` | `""` | 필드 이름 (바인딩용) |
| `EditValue` | `object` | `null` | 입력 값 |
| `EditText` | `string` | `""` | 입력 텍스트 |

#### LabelTextEdit (라벨 + 텍스트 입력)

```xml
<controls:LabelTextEdit Caption="사용자 ID" IsRequired="True" CaptionWidth="120" Width="400"/>
<controls:LabelTextEdit Caption="비고" IsReadOnly="True"/>
```

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `IsReadOnly` | `bool` | `false` | 읽기 전용 여부 |
| `MaxLength` | `int` | `0` | 최대 입력 길이 (0 = 무제한) |
| `Placeholder` | `string` | `""` | 플레이스홀더 텍스트 |

#### LabelLookUpEdit (라벨 + 콤보박스)

```xml
<controls:LabelLookUpEdit Caption="부서" IsRequired="True"
                          ItemsSource="{Binding Departments}"
                          DisplayMemberPath="Name"
                          ValueMemberPath="Code"/>
```

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `ItemsSource` | `IEnumerable` | `null` | 아이템 소스 |
| `DisplayMemberPath` | `string` | `""` | 표시 멤버 경로 |
| `ValueMemberPath` | `string` | `""` | 값 멤버 경로 |
| `SelectedIndex` | `int` | `-1` | 선택된 인덱스 |
| `SelectedItem` | `object` | `null` | 선택된 아이템 |
| `IsEditable` | `bool` | `false` | 편집 가능 여부 |

#### LabelDateEdit (라벨 + 날짜 선택)

```xml
<controls:LabelDateEdit Caption="입사일" IsRequired="True" SelectedDate="{Binding HireDate}"/>
```

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `SelectedDate` | `DateTime?` | `null` | 선택된 날짜 |
| `DisplayDateStart` | `DateTime?` | `null` | 선택 가능 시작 날짜 |
| `DisplayDateEnd` | `DateTime?` | `null` | 선택 가능 종료 날짜 |
| `DateFormat` | `string` | `yyyy-MM-dd` | 날짜 형식 |
| `IsReadOnly` | `bool` | `false` | 읽기 전용 여부 |

#### 색상

| 영역 | 색상 |
|------|------|
| 라벨 배경 | `#F0F0F0` |
| 라벨 테두리 | `#D0D0D0` |
| 라벨 글자색 | `#323232` |
| 필수 표시 (*) | `Red` |

#### LabelCheckEdit (라벨 + 체크박스)

```xml
<controls:LabelCheckEdit Caption="사용여부" IsChecked="True" CheckText="사용"/>
```

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `IsChecked` | `bool?` | `false` | 체크 상태 |
| `CheckText` | `string` | `""` | 체크박스 옆 텍스트 |
| `IsThreeState` | `bool` | `false` | 3상태 지원 여부 |

#### LabelRadioGroup (라벨 + 라디오버튼 그룹)

```xml
<controls:LabelRadioGroup Caption="성별" FieldName="gender">
    <controls:LabelRadioGroup.ItemsSource>
        <x:Array Type="sys:String">
            <sys:String>남성</sys:String>
            <sys:String>여성</sys:String>
        </x:Array>
    </controls:LabelRadioGroup.ItemsSource>
</controls:LabelRadioGroup>
```

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `ItemsSource` | `IEnumerable` | `null` | 라디오버튼 아이템 |
| `DisplayMemberPath` | `string` | `""` | 표시 멤버 경로 |
| `ValueMemberPath` | `string` | `""` | 값 멤버 경로 |
| `SelectedIndex` | `int` | `-1` | 선택된 인덱스 |
| `SelectedItem` | `object` | `null` | 선택된 아이템 |
| `Orientation` | `Orientation` | `Horizontal` | 배치 방향 |

#### LabelDateEditBetween (라벨 + 시작/종료 날짜)

```xml
<controls:LabelDateEditBetween Caption="조회기간" IsRequired="True"/>
```

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `StartDate` | `DateTime?` | `null` | 시작 날짜 |
| `EndDate` | `DateTime?` | `null` | 종료 날짜 |
| `DisplayDateStart` | `DateTime?` | `null` | 선택 가능 시작 날짜 |
| `DisplayDateEnd` | `DateTime?` | `null` | 선택 가능 종료 날짜 |
| `SeparatorText` | `string` | `~` | 구분자 텍스트 |
| `IsReadOnly` | `bool` | `false` | 읽기 전용 여부 |

---

### FvGridControl (데이터 그리드)

FactoryViewUI의 `FvGridControl` (DevExpress GridControl 기반)을 WPF DataGrid로 포팅한 컨트롤입니다.

#### 사용법

```xml
<controls:FvGridControl ItemsSource="{Binding Items}" AutoGenerateColumns="False">
    <controls:FvGridControl.Columns>
        <DataGridTextColumn Header="ID" Binding="{Binding Id}" Width="60"/>
        <DataGridTextColumn Header="이름" Binding="{Binding Name}" Width="120"/>
    </controls:FvGridControl.Columns>
</controls:FvGridControl>
```

#### 속성

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `FvProperties` | `string` | `""` | 원본 호환용 속성 |
| `ShowRowNumber` | `bool` | `true` | 행 번호 표시 여부 |
| `AlternatingRowColor` | `Brush` | `#F5F5F5` | 교대 행 배경색 |
| `HeaderBackground` | `Brush` | `#F0F0F0` | 헤더 배경색 |
| `SelectedRowBackground` | `Brush` | `#104F89` | 선택된 행 배경색 |

#### 색상

| 영역 | 색상 |
|------|------|
| 헤더 배경 | `#F0F0F0` |
| 선택 행 | `#104F89` |
| 호버 행 | `#E8F4FC` |
| 그리드 선 | `#E0E0E0` |

---

### AccordionControl (아코디언 메뉴)

FactoryViewUI의 `AccordionControl` (DevExpress 기반)을 WPF TreeView로 포팅한 컨트롤입니다.

#### 사용법

```xml
<controls:AccordionControl>
    <controls:AccordionItem Header="시스템 관리" IsGroup="True" IsExpanded="True">
        <controls:AccordionItem Header="사용자 관리"/>
        <controls:AccordionItem Header="권한 관리"/>
    </controls:AccordionItem>
    <controls:AccordionItem Header="생산 관리" IsGroup="True">
        <controls:AccordionItem Header="작업 지시"/>
    </controls:AccordionItem>
</controls:AccordionControl>
```

#### AccordionControl 속성

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `HeaderBackground` | `Brush` | `#1F303A` | 헤더 배경색 |
| `HeaderForeground` | `Brush` | `#B9C2D3` | 헤더 글자색 |
| `ItemBackground` | `Brush` | `#010A13` | 아이템 배경색 |
| `ItemForeground` | `Brush` | `White` | 아이템 글자색 |
| `ItemHoverBackground` | `Brush` | `#13437C` | 호버 배경색 |
| `ItemHoverForeground` | `Brush` | `#00A3FF` | 호버 글자색 |
| `IsMinimized` | `bool` | `false` | 최소화 상태 |

#### AccordionItem 속성

| 속성 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `Icon` | `object` | `null` | 아이콘 |
| `IsGroup` | `bool` | `false` | 그룹(헤더) 여부 |

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
- [x] LabelTextEdit, LabelLookUpEdit, LabelDateEdit 포팅
- [x] LabelCheckEdit, LabelRadioGroup, LabelDateEditBetween 포팅
- [x] FvGridControl 포팅
- [x] AccordionControl 포팅
- [ ] FvMessageBox 포팅
- [ ] DefaultForm 포팅
