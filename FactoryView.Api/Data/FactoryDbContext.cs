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
            // 메뉴 라벨
            new SYS100_LABELS { LabelCode = "LBL_MASTER", LabelType = "MENU", LabelKR = "기준정보", LabelEN = "Master", LabelCH = "基础信息", LabelJP = "基準情報" },
            new SYS100_LABELS { LabelCode = "LBL_PURCHASE", LabelType = "MENU", LabelKR = "구매관리", LabelEN = "Purchase", LabelCH = "采购管理", LabelJP = "購買管理" },
            new SYS100_LABELS { LabelCode = "LBL_PRODUCTION", LabelType = "MENU", LabelKR = "생산관리", LabelEN = "Production", LabelCH = "生产管理", LabelJP = "生産管理" },
            new SYS100_LABELS { LabelCode = "LBL_SYSTEM", LabelType = "MENU", LabelKR = "시스템관리", LabelEN = "System", LabelCH = "系统管理", LabelJP = "システム管理" },
            new SYS100_LABELS { LabelCode = "LBL_PRODUCT_MASTER", LabelType = "MENU", LabelKR = "제품 마스터", LabelEN = "Product Master", LabelCH = "产品主数据", LabelJP = "製品マスター" },
            new SYS100_LABELS { LabelCode = "LBL_MATERIAL_MASTER", LabelType = "MENU", LabelKR = "자재 마스터", LabelEN = "Material Master", LabelCH = "物料主数据", LabelJP = "資材マスター" },
            new SYS100_LABELS { LabelCode = "LBL_COMPANY_MASTER", LabelType = "MENU", LabelKR = "거래처 마스터", LabelEN = "Company Master", LabelCH = "客户主数据", LabelJP = "取引先マスター" },
            new SYS100_LABELS { LabelCode = "LBL_MATERIAL_PO", LabelType = "MENU", LabelKR = "자재 구매발주", LabelEN = "Material PO", LabelCH = "物料采购订单", LabelJP = "資材発注" },
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
                LabelCode = "제품 마스터",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M001",
                MenuSeq = 1,
                Description = "MasterProduct"
            },
            new SYS200_MENUS
            {
                MenuId = "M001002",
                LabelCode = "자재 마스터",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M001",
                MenuSeq = 2,
                Description = "MasterMaterial"
            },
            new SYS200_MENUS
            {
                MenuId = "M001003",
                LabelCode = "거래처 마스터",
                ModType = "MES",
                MenuType = "TPS008002",
                DisplayYN = 0,
                ParentMenuId = "M001",
                MenuSeq = 3,
                Description = "MasterCompany"
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
