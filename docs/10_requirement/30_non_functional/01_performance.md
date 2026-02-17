# パフォーマンス要件

PlasticModelAppのパフォーマンス要件・目標値。

---

## レスポンスタイム目標

| エンドポイント | P95目標 | P99目標 |
| -------------- | --------- | --------- |
| API（単純クエリ） | < 200ms | < 500ms |
| API（検索） | < 500ms | < 1000ms |
| ページロード（初回） | < 2s | < 3s |
| ページロード（2回目以降） | < 500ms | < 1s |

---

## スループット目標

| 環境 | リクエスト/秒 | 同時接続数 |
| ------ | ------------- | ----------- |
| Development | 10 req/s | 50 |
| Staging | 50 req/s | 100 |
| Production | 100 req/s | 500 |

---

## データベースクエリ

### クエリ実行時間目標

| クエリタイプ | 目標 |
| ------------ | ------ |
| Primary Key検索 | < 10ms |
| Index検索 | < 50ms |
| 全文検索 | < 100ms |
| JOIN（3テーブル以内） | < 100ms |

### N+1クエリ対策

Eager Loadingを使用:

```csharp
// Bad: N+1 query
var stocks = await _context.PaintStocks.ToListAsync();
foreach (var stock in stocks)
{
    var paint = await _context.Paints.FindAsync(stock.PaintId); // N回クエリ
}

// Good: Eager loading
var stocks = await _context.PaintStocks
    .Include(s => s.Paint)
    .ToListAsync();
```

---

## キャッシング戦略

### クライアントサイドキャッシング

- **Service Worker**: Blazor WASM静的アセットをキャッシュ
- **LocalStorage**: ユーザー設定、最近の検索履歴

### サーバーサイドキャッシング（将来）

| データ | キャッシュ期間 | 理由 |
| -------- | ------------ | ------ |
| 塗料マスタ | 1時間 | 変更頻度が低い |
| メーカー・ブランド | 24時間 | ほぼ変更なし |
| ユーザーデータ | キャッシュなし | 常に最新が必要 |

---

## データベースインデックス

### 必須インデックス

```sql
-- ユーザーIDでのフィルタ（頻繁に使用）
CREATE INDEX IX_PaintStocks_UserId ON PaintStocks (UserId);
CREATE INDEX IX_CompareLists_UserId ON CompareLists (UserId);
CREATE INDEX IX_PaintFavorites_UserId ON PaintFavorites (UserId);

-- 塗料検索
CREATE INDEX IX_Paints_Name ON Paints (Name);
CREATE INDEX IX_Paints_ProductCode ON Paints (ProductCode);
CREATE INDEX IX_Paints_ManufacturerId ON Paints (ManufacturerId);
CREATE INDEX IX_Paints_BrandId ON Paints (BrandId);

-- 外部キー
CREATE INDEX IX_PaintStocks_PaintId ON PaintStocks (PaintId);
CREATE INDEX IX_CompareListItems_PaintId ON CompareListItems (PaintId);
```

---

## ページネーション

### デフォルト設定

- **デフォルトページサイズ**: 30
- **最大ページサイズ**: 100
- **Offset vs Cursor**: Offset pagination（現状）

### 実装例

```csharp
public async Task<PaginatedResult<PaintDto>> SearchPaintsAsync(
    string? keyword,
    int page = 1,
    int pageSize = 30)
{
    pageSize = Math.Min(pageSize, 100); // 最大100

    var query = _context.Paints.AsQueryable();

    if (!string.IsNullOrEmpty(keyword))
    {
        query = query.Where(p => p.Name.Contains(keyword));
    }

    var totalCount = await query.CountAsync();
    var items = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new PaginatedResult<PaintDto>
    {
        Items = items,
        TotalCount = totalCount,
        Page = page,
        PageSize = pageSize
    };
}
```

---

## 画像最適化（将来）

### 塗料画像

- **形式**: WebP（フォールバック: JPEG）
- **サイズ**:
  - サムネイル: 200x200px
  - 詳細: 800x800px
- **圧縮**: 80%品質

---

## CDN活用（将来）

- **静的アセット**: Cloudflare CDN
- **画像**: Cloudflare Images
- **効果**: レイテンシ削減、サーバー負荷削減

---

## SLO（Service Level Objectives）

### 可用性

- **目標**: 99.9%（月間43.2分ダウンタイム許容）

### レスポンスタイム

- **API P95**: 95%のリクエストが200ms以内
- **API P99**: 99%のリクエストが500ms以内

---

## パフォーマンステスト

### ローカル開発

```bash
# Apache Bench
ab -n 1000 -c 10 https://localhost:5001/api/paints/search

# dotnet-counters（メモリ・CPU監視）
dotnet-counters monitor --process-id <pid>
```

### Staging環境

- **ツール**: JMeter / Gatling
- **シナリオ**:
  - 同時ユーザー数: 100
  - テスト時間: 10分
  - レスポンスタイム目標: P95 < 500ms

---
