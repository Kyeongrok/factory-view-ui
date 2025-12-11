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
}
