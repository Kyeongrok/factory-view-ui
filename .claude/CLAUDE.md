# Factory View UI - Claude Code 설정

## 프로젝트 개요
WPF 기반 MES(Manufacturing Execution System) Factory View UI 프로젝트

## 기술 스택
- .NET 8.0 (Windows)
- WPF (Windows Presentation Foundation)
- C#
- Database:
  - 개발: SQLite
  - 운영: PostgreSQL

## 프로젝트 구조
```
factory-view-ui/
├── FactoryView.Api/          # API 클라이언트 및 Entity
│   ├── Entities/             # 데이터베이스 Entity 클래스
│   ├── Master/               # 마스터 데이터 API
│   └── System/               # 시스템 API
├── MES/                      # MES 관련 문서
└── 레거시 분석/               # 레거시 시스템 분석 문서
```

## Entity 명명 규칙
- 형식: `{테이블코드}_{영문명}` (예: MST110_ITEMS, SAL100_SALES_ORDER_HEADERS)
- 테이블 접두어:
  - MST: 마스터 데이터
  - SAL: 판매
  - PUR: 구매
  - PRD: 생산
  - LOG: 로트/로그
  - SCH: 일정/계획
  - QCM: 품질관리
  - MCH: 설비관리
  - SYS: 시스템
  - COD: 코드

## 참고 문서
- MES/1 생산 프로세스.md - 생산 프로세스 전체 흐름
- 레거시 분석/Backend/백엔드 API 테이블 및 Controller 매핑.md - 테이블 구조 참조
