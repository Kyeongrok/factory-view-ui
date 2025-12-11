# 개발 가이드

## Entity 구조

### 테이블 명명 규칙

- 형식: `{테이블코드}_{영문명}`
- 예: `MST110_ITEMS`, `SAL100_SALES_ORDER_HEADERS`

### 테이블 접두어

| 접두어 | 설명 |
|--------|------|
| MST | 마스터 데이터 |
| SAL | 판매 |
| PUR | 구매 |
| PRD | 생산 |
| LOG | 로트/로그 |
| SCH | 일정/계획 |
| QCM | 품질관리 |
| SYS | 시스템 |

## 데이터베이스

### 개발 환경 (SQLite)

```csharp
var db = DbContextFactory.CreateSqliteContext();
```

### 운영 환경 (PostgreSQL)

```csharp
var db = DbContextFactory.CreatePostgresContext("Host=localhost;Database=factory;Username=user;Password=pass");
```
