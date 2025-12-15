using Microsoft.EntityFrameworkCore;
using FactoryView.Api.Entities;

namespace FactoryView.Api.Data;

/// <summary>
/// Factory View MES 데이터베이스 컨텍스트
/// </summary>
public class FactoryDbContext : DbContext
{
    public FactoryDbContext(DbContextOptions<FactoryDbContext> options) : base(options)
    {
    }

    // Master Tables
    /// <summary>품목 마스터</summary>
    public DbSet<MST110_ITEMS> Items { get; set; }

    /// <summary>업체 마스터</summary>
    public DbSet<MST150_COMPANIES> Companies { get; set; }

    // System Tables
    /// <summary>메뉴 마스터</summary>
    public DbSet<SYS100_MENUS> Menus { get; set; }

    /// <summary>언어 라벨 정보 (SYS100)</summary>
    public DbSet<SYS100_LABELS> Labels { get; set; }

    /// <summary>메뉴 정보 (SYS200)</summary>
    public DbSet<SYS200_MENUS> MenuInfos { get; set; }

    // Sales Tables
    /// <summary>판매 주문 헤더</summary>
    public DbSet<SAL100_SALES_ORDER_HEADERS> SalesOrderHeaders { get; set; }

    /// <summary>판매 주문 상세</summary>
    public DbSet<SAL101_SALES_ORDER_DETAILS> SalesOrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // SAL101: Composite Primary Key
        modelBuilder.Entity<SAL101_SALES_ORDER_DETAILS>()
            .HasKey(e => new { e.OrdNo, e.OrdSeq });

        // SAL100 -> MST150 관계 설정
        modelBuilder.Entity<SAL100_SALES_ORDER_HEADERS>()
            .HasOne(e => e.Customer)
            .WithMany(e => e.SalesOrders)
            .HasForeignKey(e => e.CustId)
            .OnDelete(DeleteBehavior.Restrict);

        // SAL101 -> SAL100 관계 설정
        modelBuilder.Entity<SAL101_SALES_ORDER_DETAILS>()
            .HasOne(e => e.Header)
            .WithMany(e => e.Details)
            .HasForeignKey(e => e.OrdNo)
            .OnDelete(DeleteBehavior.Cascade);

        // SAL101 -> MST110 관계 설정
        modelBuilder.Entity<SAL101_SALES_ORDER_DETAILS>()
            .HasOne(e => e.Item)
            .WithMany()
            .HasForeignKey(e => e.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // SYS100: Self-referencing 관계 설정 (메뉴 트리)
        modelBuilder.Entity<SYS100_MENUS>()
            .HasOne(e => e.Parent)
            .WithMany(e => e.Children)
            .HasForeignKey(e => e.PMenuId)
            .OnDelete(DeleteBehavior.Restrict);

        // SYS200: Self-referencing 관계 설정 (메뉴 트리)
        modelBuilder.Entity<SYS200_MENUS>()
            .HasOne(e => e.Parent)
            .WithMany(e => e.Children)
            .HasForeignKey(e => e.ParentMenuId)
            .OnDelete(DeleteBehavior.Restrict);

        // Decimal precision 설정
        modelBuilder.Entity<SAL100_SALES_ORDER_HEADERS>()
            .Property(e => e.TotalAmount)
            .HasPrecision(18, 4);

        modelBuilder.Entity<SAL101_SALES_ORDER_DETAILS>()
            .Property(e => e.OrdQnt)
            .HasPrecision(18, 4);

        modelBuilder.Entity<SAL101_SALES_ORDER_DETAILS>()
            .Property(e => e.UnitPrice)
            .HasPrecision(18, 4);

        modelBuilder.Entity<SAL101_SALES_ORDER_DETAILS>()
            .Property(e => e.Amount)
            .HasPrecision(18, 4);

        modelBuilder.Entity<SAL101_SALES_ORDER_DETAILS>()
            .Property(e => e.ShipQnt)
            .HasPrecision(18, 4);

        modelBuilder.Entity<SAL101_SALES_ORDER_DETAILS>()
            .Property(e => e.RemainQnt)
            .HasPrecision(18, 4);

        modelBuilder.Entity<MST110_ITEMS>()
            .Property(e => e.SafetyQnt)
            .HasPrecision(18, 4);
    }
}

/// <summary>
/// DbContext 팩토리 (개발/운영 환경 분리)
/// </summary>
public static class DbContextFactory
{
    /// <summary>
    /// SQLite DbContext 생성 (개발용)
    /// </summary>
    public static FactoryDbContext CreateSqliteContext(string connectionString = "Data Source=factory.db")
    {
        var options = new DbContextOptionsBuilder<FactoryDbContext>()
            .UseSqlite(connectionString)
            .Options;

        return new FactoryDbContext(options);
    }

    /// <summary>
    /// PostgreSQL DbContext 생성 (운영용)
    /// </summary>
    public static FactoryDbContext CreatePostgresContext(string connectionString)
    {
        var options = new DbContextOptionsBuilder<FactoryDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new FactoryDbContext(options);
    }

    /// <summary>
    /// DB 초기화 및 시딩 (개발용)
    /// </summary>
    public static void InitializeDatabase(FactoryDbContext context)
    {
        // DB 생성 (없으면)
        context.Database.EnsureCreated();

        // SYS100 라벨 데이터가 없으면 초기 데이터 삽입
        if (!context.Labels.Any())
        {
            SeedLabelData(context);
        }

        // SYS200 메뉴 데이터가 없으면 초기 데이터 삽입
        if (!context.MenuInfos.Any())
        {
            SeedMenuData(context);
        }
    }

    private static void SeedLabelData(FactoryDbContext context)
    {
        var labels = new List<SYS100_LABELS>
        {
            // 대메뉴 라벨
            new SYS100_LABELS { LabelCode = "LBL_MASTER", LabelType = "MENU", LabelKR = "기준정보관리", LabelEN = "Master Data", LabelCH = "基础信息管理", LabelJP = "基準情報管理" },
            new SYS100_LABELS { LabelCode = "LBL_PURCHASE", LabelType = "MENU", LabelKR = "구매관리", LabelEN = "Purchase", LabelCH = "采购管理", LabelJP = "購買管理" },
            new SYS100_LABELS { LabelCode = "LBL_PRODUCTION", LabelType = "MENU", LabelKR = "생산관리", LabelEN = "Production", LabelCH = "生产管理", LabelJP = "生産管理" },
            new SYS100_LABELS { LabelCode = "LBL_SYSTEM", LabelType = "MENU", LabelKR = "시스템관리", LabelEN = "System", LabelCH = "系统管理", LabelJP = "システム管理" },

            // 기준정보관리 하위 메뉴 라벨
            new SYS100_LABELS { LabelCode = "LBL_CODE_MGMT", LabelType = "MENU", LabelKR = "코드관리", LabelEN = "Code Management", LabelCH = "代码管理", LabelJP = "コード管理" },
            new SYS100_LABELS { LabelCode = "LBL_ITEM_INFO", LabelType = "MENU", LabelKR = "품목정보관리", LabelEN = "Item Information", LabelCH = "物品信息管理", LabelJP = "品目情報管理" },

            // 품목정보관리 하위 메뉴 라벨 (Level 3)
            new SYS100_LABELS { LabelCode = "LBL_PRODUCT_INFO", LabelType = "MENU", LabelKR = "제품정보관리", LabelEN = "Product Information", LabelCH = "产品信息管理", LabelJP = "製品情報管理" },
            new SYS100_LABELS { LabelCode = "LBL_MATERIAL_INFO", LabelType = "MENU", LabelKR = "자재정보관리", LabelEN = "Material Information", LabelCH = "物料信息管理", LabelJP = "資材情報管理" },
            new SYS100_LABELS { LabelCode = "LBL_TOOL_INFO", LabelType = "MENU", LabelKR = "공구정보관리", LabelEN = "Tool Information", LabelCH = "工具信息管理", LabelJP = "工具情報管理" },
            new SYS100_LABELS { LabelCode = "LBL_SPARE_PARTS", LabelType = "MENU", LabelKR = "예비품정보관리", LabelEN = "Spare Parts Info", LabelCH = "备件信息管理", LabelJP = "予備品情報管理" },
            new SYS100_LABELS { LabelCode = "LBL_DEV_HISTORY", LabelType = "MENU", LabelKR = "개발이력관리", LabelEN = "Development History", LabelCH = "开发履历管理", LabelJP = "開発履歴管理" },
            new SYS100_LABELS { LabelCode = "LBL_DEFECT_RATE", LabelType = "MENU", LabelKR = "제품별불량률관리", LabelEN = "Defect Rate by Product", LabelCH = "产品不良率管理", LabelJP = "製品別不良率管理" },
            new SYS100_LABELS { LabelCode = "LBL_INGOT_CONV", LabelType = "MENU", LabelKR = "인고트환산정보", LabelEN = "Ingot Conversion Info", LabelCH = "铸锭换算信息", LabelJP = "インゴット換算情報" },
            new SYS100_LABELS { LabelCode = "LBL_PROCESS_MGMT", LabelType = "MENU", LabelKR = "공정관리", LabelEN = "Process Management", LabelCH = "工序管理", LabelJP = "工程管理" },
            new SYS100_LABELS { LabelCode = "LBL_COMPANY_INFO", LabelType = "MENU", LabelKR = "거래처정보관리", LabelEN = "Company Information", LabelCH = "客户信息管理", LabelJP = "取引先情報管理" },
            new SYS100_LABELS { LabelCode = "LBL_EMPLOYEE_INFO", LabelType = "MENU", LabelKR = "인원정보관리", LabelEN = "Employee Information", LabelCH = "人员信息管理", LabelJP = "人員情報管理" },
            new SYS100_LABELS { LabelCode = "LBL_EQUIPMENT_INFO", LabelType = "MENU", LabelKR = "설비정보관리", LabelEN = "Equipment Information", LabelCH = "设备信息管理", LabelJP = "設備情報管理" },
            new SYS100_LABELS { LabelCode = "LBL_MFG_PROCESS", LabelType = "MENU", LabelKR = "제조공정관리", LabelEN = "Manufacturing Process", LabelCH = "制造工序管理", LabelJP = "製造工程管理" },
            new SYS100_LABELS { LabelCode = "LBL_QUALITY_STD", LabelType = "MENU", LabelKR = "품질기준관리", LabelEN = "Quality Standards", LabelCH = "质量标准管理", LabelJP = "品質基準管理" },
            new SYS100_LABELS { LabelCode = "LBL_WORK_SCHEDULE", LabelType = "MENU", LabelKR = "작업일정관리", LabelEN = "Work Schedule", LabelCH = "作业日程管理", LabelJP = "作業日程管理" },
            new SYS100_LABELS { LabelCode = "LBL_MGMT_PLAN", LabelType = "MENU", LabelKR = "관리계획정보", LabelEN = "Management Plan", LabelCH = "管理计划信息", LabelJP = "管理計画情報" },

            // 구매관리 하위 메뉴 라벨
            new SYS100_LABELS { LabelCode = "LBL_MATERIAL_PO", LabelType = "MENU", LabelKR = "자재 구매발주", LabelEN = "Material PO", LabelCH = "物料采购订单", LabelJP = "資材発注" },

            // 시스템관리 하위 메뉴 라벨
            new SYS100_LABELS { LabelCode = "LBL_MENU_MGMT", LabelType = "MENU", LabelKR = "메뉴 관리", LabelEN = "Menu Management", LabelCH = "菜单管理", LabelJP = "メニュー管理" },
            new SYS100_LABELS { LabelCode = "LBL_USER_MGMT", LabelType = "MENU", LabelKR = "사용자 관리", LabelEN = "User Management", LabelCH = "用户管理", LabelJP = "ユーザー管理" },
            new SYS100_LABELS { LabelCode = "LBL_LABEL_MGMT", LabelType = "MENU", LabelKR = "라벨 관리", LabelEN = "Label Management", LabelCH = "标签管理", LabelJP = "ラベル管理" },

            // 공통 버튼 라벨
            new SYS100_LABELS { LabelCode = "BTN_SEARCH", LabelType = "BUTTON", LabelKR = "조회", LabelEN = "Search", LabelCH = "查询", LabelJP = "検索" },
            new SYS100_LABELS { LabelCode = "BTN_SAVE", LabelType = "BUTTON", LabelKR = "저장", LabelEN = "Save", LabelCH = "保存", LabelJP = "保存" },
            new SYS100_LABELS { LabelCode = "BTN_DELETE", LabelType = "BUTTON", LabelKR = "삭제", LabelEN = "Delete", LabelCH = "删除", LabelJP = "削除" },
            new SYS100_LABELS { LabelCode = "BTN_ADD", LabelType = "BUTTON", LabelKR = "추가", LabelEN = "Add", LabelCH = "添加", LabelJP = "追加" },
            new SYS100_LABELS { LabelCode = "BTN_CANCEL", LabelType = "BUTTON", LabelKR = "취소", LabelEN = "Cancel", LabelCH = "取消", LabelJP = "キャンセル" },
            new SYS100_LABELS { LabelCode = "BTN_CONFIRM", LabelType = "BUTTON", LabelKR = "확인", LabelEN = "Confirm", LabelCH = "确认", LabelJP = "確認" },
            new SYS100_LABELS { LabelCode = "BTN_CLOSE", LabelType = "BUTTON", LabelKR = "닫기", LabelEN = "Close", LabelCH = "关闭", LabelJP = "閉じる" },

            // 공통 컬럼 라벨
            new SYS100_LABELS { LabelCode = "COL_SEQ", LabelType = "COLUMN", LabelKR = "순번", LabelEN = "Seq", LabelCH = "序号", LabelJP = "順番" },
            new SYS100_LABELS { LabelCode = "COL_MENU_ID", LabelType = "COLUMN", LabelKR = "메뉴ID", LabelEN = "Menu ID", LabelCH = "菜单ID", LabelJP = "メニューID" },
            new SYS100_LABELS { LabelCode = "COL_LABEL_CODE", LabelType = "COLUMN", LabelKR = "라벨코드", LabelEN = "Label Code", LabelCH = "标签代码", LabelJP = "ラベルコード" },
            new SYS100_LABELS { LabelCode = "COL_DESCRIPTION", LabelType = "COLUMN", LabelKR = "설명", LabelEN = "Description", LabelCH = "描述", LabelJP = "説明" }
        };

        context.Labels.AddRange(labels);
        context.SaveChanges();
    }

    private static void SeedMenuData(FactoryDbContext context)
    {
        var menus = new List<SYS200_MENUS>
        {
            // 대메뉴 (Level 1)
            new SYS200_MENUS
            {
                MenuId = "M001",
                LabelCode = "기준정보",
                ModType = "MES",
                MenuType = "TPS008001",
                DisplayYN = 0,
                ParentMenuId = null,
                MenuSeq = 1,
                Description = "기준정보 관리"
            },
            new SYS200_MENUS
            {
                MenuId = "M002",
                LabelCode = "구매관리",
                ModType = "MES",
                MenuType = "TPS008001",
                DisplayYN = 0,
                ParentMenuId = null,
                MenuSeq = 2,
                Description = "구매 관리"
            },
            new SYS200_MENUS
            {
                MenuId = "M003",
                LabelCode = "생산관리",
                ModType = "MES",
                MenuType = "TPS008001",
                DisplayYN = 0,
                ParentMenuId = null,
                MenuSeq = 3,
                Description = "생산 관리"
            },
            new SYS200_MENUS
            {
                MenuId = "M005",
                LabelCode = "시스템관리",
                ModType = "MES",
                MenuType = "TPS008001",
                DisplayYN = 0,
                ParentMenuId = null,
                MenuSeq = 5,
                Description = "시스템 관리"
            },

            // 중메뉴 (Level 2) - 기준정보
            new SYS200_MENUS
            {
                MenuId = "M001001",
                LabelCode = "코드관리",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M001",
                MenuSeq = 1,
                Description = "CodeInfo"
            },
            new SYS200_MENUS
            {
                MenuId = "M001002",
                LabelCode = "품목정보관리",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M001",
                MenuSeq = 2,
                Description = "ItemInfo"
            },

            // 소메뉴 (Level 3) - 품목정보관리
            new SYS200_MENUS
            {
                MenuId = "M001002001",
                LabelCode = "제품정보관리",
                ModType = "MES",
                MenuType = "TPS008003",
                DisplayYN = 0,
                ParentMenuId = "M001002",
                MenuSeq = 1,
                Description = "MasterProduct"
            },
            new SYS200_MENUS
            {
                MenuId = "M001002002",
                LabelCode = "자재정보관리",
                ModType = "MES",
                MenuType = "TPS008003",
                DisplayYN = 0,
                ParentMenuId = "M001002",
                MenuSeq = 2,
                Description = "MasterRawMaterial"
            },
            new SYS200_MENUS
            {
                MenuId = "M001002003",
                LabelCode = "공구정보관리",
                ModType = "MES",
                MenuType = "TPS008003",
                DisplayYN = 0,
                ParentMenuId = "M001002",
                MenuSeq = 3,
                Description = "MasterTool"
            },
            new SYS200_MENUS
            {
                MenuId = "M001002004",
                LabelCode = "예비품정보관리",
                ModType = "MES",
                MenuType = "TPS008003",
                DisplayYN = 0,
                ParentMenuId = "M001002",
                MenuSeq = 4,
                Description = "MasterProductTotal"
            },
            new SYS200_MENUS
            {
                MenuId = "M001002005",
                LabelCode = "개발이력관리",
                ModType = "MES",
                MenuType = "TPS008003",
                DisplayYN = 0,
                ParentMenuId = "M001002",
                MenuSeq = 5,
                Description = "DevelopmentLog"
            },
            new SYS200_MENUS
            {
                MenuId = "M001002006",
                LabelCode = "제품별불량률관리",
                ModType = "MES",
                MenuType = "TPS008003",
                DisplayYN = 0,
                ParentMenuId = "M001002",
                MenuSeq = 6,
                Description = "ProductDefectRate"
            },
            new SYS200_MENUS
            {
                MenuId = "M001002007",
                LabelCode = "인고트환산정보",
                ModType = "MES",
                MenuType = "TPS008003",
                DisplayYN = 0,
                ParentMenuId = "M001002",
                MenuSeq = 7,
                Description = "IngotConversionInfo"
            },
            new SYS200_MENUS
            {
                MenuId = "M001003",
                LabelCode = "공정관리",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M001",
                MenuSeq = 3,
                Description = "ProcessInfo"
            },
            new SYS200_MENUS
            {
                MenuId = "M001004",
                LabelCode = "거래처정보관리",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M001",
                MenuSeq = 4,
                Description = "CompanyInfo"
            },
            new SYS200_MENUS
            {
                MenuId = "M001005",
                LabelCode = "인원정보관리",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M001",
                MenuSeq = 5,
                Description = "EmployeeInfo"
            },
            new SYS200_MENUS
            {
                MenuId = "M001006",
                LabelCode = "설비정보관리",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M001",
                MenuSeq = 6,
                Description = "EquipmentInfo"
            },
            new SYS200_MENUS
            {
                MenuId = "M001007",
                LabelCode = "제조공정관리",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M001",
                MenuSeq = 7,
                Description = "ManufacturingProcessInfo"
            },
            new SYS200_MENUS
            {
                MenuId = "M001008",
                LabelCode = "품질기준관리",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M001",
                MenuSeq = 8,
                Description = "QualityStandardInfo"
            },
            new SYS200_MENUS
            {
                MenuId = "M001009",
                LabelCode = "작업일정관리",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M001",
                MenuSeq = 9,
                Description = "WorkScheduleInfo"
            },
            new SYS200_MENUS
            {
                MenuId = "M001010",
                LabelCode = "관리계획정보",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M001",
                MenuSeq = 10,
                Description = "ManagementPlanInfo"
            },

            // 중메뉴 (Level 2) - 구매관리
            new SYS200_MENUS
            {
                MenuId = "M002001",
                LabelCode = "자재 구매발주",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M002",
                MenuSeq = 1,
                Description = "MaterialPurchaseOrderView"
            },

            // 중메뉴 (Level 2) - 시스템관리
            new SYS200_MENUS
            {
                MenuId = "M005001",
                LabelCode = "메뉴 관리",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M005",
                MenuSeq = 1,
                Description = "SystemMenu"
            },
            new SYS200_MENUS
            {
                MenuId = "M005002",
                LabelCode = "사용자 관리",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M005",
                MenuSeq = 2,
                Description = "SystemUser"
            },
            new SYS200_MENUS
            {
                MenuId = "M005003",
                LabelCode = "라벨 관리",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M005",
                MenuSeq = 3,
                Description = "LabelInfo"
            }
        };

        context.MenuInfos.AddRange(menus);
        context.SaveChanges();
    }
}
