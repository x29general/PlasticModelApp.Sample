# テーブル設計

## `Paints`

| Column Name          | Type                        | Constraints | Index       | Notes                      |
|:---------------------|:----------------------------|:------------|:------------|:---------------------------|
| Id                   | text                        | PK          | PRIMARY KEY | PaintId value object       |
| Name                 | varchar(100)                | NOT NULL    | INDEX       |                            |
| ModelNumber          | varchar(50)                 |             | UNIQUE (BrandId, ModelNumber) | ModelNumber value object   |
| ModelNumberPrefix    | varchar(50)                 |             | INDEX       | Sort prefix                |
| ModelNumberNumber    | integer                     |             | INDEX       | Sort number                |
| BrandId              | text                        | FK NOT NULL | INDEX       | Brand FK                   |
| PaintTypeId          | text                        | FK NOT NULL | INDEX       | PaintType FK               |
| GlossId              | text                        | FK NOT NULL | INDEX       | Gloss FK                   |
| Price                | numeric(10,2)               | NOT NULL    |             | Price value object         |
| Description          | varchar(500)                |             |             |                            |
| ImageUrl             | varchar(200)                |             |             |                            |
| Hex                  | text                        | NOT NULL    |             | HexColor value object      |
| Rgb_R                | integer                     | NOT NULL    |             | RGB Red component          |
| Rgb_G                | integer                     | NOT NULL    |             | RGB Green component        |
| Rgb_B                | integer                     | NOT NULL    |             | RGB Blue component         |
| Hsl_H                | real                        | NOT NULL    |             | HSL Hue component          |
| Hsl_S                | real                        | NOT NULL    |             | HSL Saturation component   |
| Hsl_L                | real                        | NOT NULL    |             | HSL Lightness component    |
| CreatedAt            | timestamp with time zone    | NOT NULL    |             | DEFAULT CURRENT_TIMESTAMP  |
| UpdatedAt            | timestamp with time zone    | NOT NULL    |             | DEFAULT CURRENT_TIMESTAMP  |
| IsDeleted            | boolean                     | NOT NULL    |             | DEFAULT false, soft delete |

**Composite Indexes:**
- `(ModelNumberPrefix, ModelNumberNumber)` for model number sorting

**Query Filters:**
- Soft delete: `WHERE IsDeleted = false`

---

## `Brands`

| Column Name | Type         | Constraints | Index       | Notes |
|:------------|:-------------|:------------|:------------|:------|
| Id          | text         | PK          | PRIMARY KEY | BrandId value object |
| Name        | varchar(50)  | NOT NULL    | INDEX       | |
| Description | text         |             |             | |

---

## `PaintTypes`

| Column Name | Type        | Constraints | Index       | Notes |
|:------------|:------------|:------------|:------------|:------|
| Id          | text        | PK          | PRIMARY KEY | PaintTypeId value object |
| Name        | varchar(50) | NOT NULL    |             | |
| Description | text        |             |             | |

---

## `Glosses`

| Column Name | Type        | Constraints | Index       | Notes |
|:------------|:------------|:------------|:------------|:------|
| Id          | text        | PK          | PRIMARY KEY | GlossId value object |
| Name        | varchar(50) | NOT NULL    |             | |
| Description | text        |             |             | |

---

## `Tags`

| Column Name   | Type                     | Constraints | Index       | Notes |
|:--------------|:-------------------------|:------------|:------------|:------|
| Id            | text                     | PK          | PRIMARY KEY | TagId value object |
| Name          | varchar(50)              | NOT NULL    |             | |
| TagCategoryId | text                     | FK NOT NULL | INDEX       | TagCategory FK |
| Hex           | varchar(7)               |             |             | Hex color code |
| Effect        | text                     |             |             | |
| Description   | text                     |             |             | |
| CreatedAt     | timestamp with time zone | NOT NULL    |             | DEFAULT CURRENT_TIMESTAMP |
| UpdatedAt     | timestamp with time zone | NOT NULL    |             | DEFAULT CURRENT_TIMESTAMP |
| IsDeleted     | boolean                  | NOT NULL    |             | DEFAULT false, soft delete |

**Query Filters:**
- Soft delete: `WHERE IsDeleted = false`

---

## `TagCategories`

| Column Name | Type        | Constraints | Index       | Notes |
|:------------|:------------|:------------|:------------|:------|
| Id          | text        | PK          | PRIMARY KEY | TagCategoryId value object |
| Name        | varchar(50) | NOT NULL    |             | |
| Description | text        |             |             | |

---

## `PaintTags`

| Column Name | Type | Constraints              | Index       | Notes |
|:------------|:-----|:-------------------------|:------------|:------|
| PaintId     | text | PK, FK                   | INDEX       | Composite PK with TagId |
| TagId       | text | PK, FK                   | INDEX       | Composite PK with PaintId |

**Composite Primary Key:**
- `(PaintId, TagId)`

---

## インデックス戦略

- **PK**: すべての主キーは自動的にインデックスが作成される。
- **FK**: すべての外部キー列には、結合の最適化のためにインデックスが作成される。
- **ユニーク制約**: (BrandId, ModelNumber)
- **複合インデックス**: (ModelNumberPrefix, ModelNumberNumber)は、モデル番号でのソートに使用される。
- **論理削除**: PaintsとTagsは、IsDeleted=trueのレコードを除外するクエリフィルタを使用する。
