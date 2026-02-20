using PlasticModelApp.Infrastructure.Data;
using PlasticModelApp.Infrastructure.Data.Entities;

namespace PlasticModelApp.Infrastructure.UnitTests.Fixtures;

public static class TestDataSeeder
{
    public static class Ids
    {
        public const string BrandA = "brand-a";
        public const string BrandB = "brand-b";
        public const string TypeAcrylic = "type-acrylic";
        public const string TypeEnamel = "type-enamel";
        public const string GlossMatte = "gloss-matte";
        public const string GlossGloss = "gloss-gloss";
        public const string CategoryFinish = "cat-finish";
        public const string CategoryTone = "cat-tone";
        public const string TagMatte = "tag-matte";
        public const string TagGlossy = "tag-glossy";
        public const string TagWarm = "tag-warm";
        public const string TagCool = "tag-cool";
        public const string PaintP1 = "paint-p1";
        public const string PaintP2 = "paint-p2";
        public const string PaintP3 = "paint-p3";
        public const string PaintDeleted = "paint-deleted";
    }

    public static void SeedCommon(ApplicationDbContext db)
    {
        var now = new DateTimeOffset(2026, 2, 20, 0, 0, 0, TimeSpan.Zero);

        db.Brands.AddRange(
            new BrandEntity { Id = Ids.BrandA, Name = "Brand A", Description = "A" },
            new BrandEntity { Id = Ids.BrandB, Name = "Brand B", Description = "B" });

        db.PaintTypes.AddRange(
            new PaintTypeEntity { Id = Ids.TypeAcrylic, Name = "Acrylic", Description = "ac" },
            new PaintTypeEntity { Id = Ids.TypeEnamel, Name = "Enamel", Description = "en" });

        db.Glosses.AddRange(
            new GlossEntity { Id = Ids.GlossMatte, Name = "Matte", Description = "m" },
            new GlossEntity { Id = Ids.GlossGloss, Name = "Gloss", Description = "g" });

        db.TagCategories.AddRange(
            new TagCategoryEntity { Id = Ids.CategoryFinish, Name = "Finish", Description = "finish tags" },
            new TagCategoryEntity { Id = Ids.CategoryTone, Name = "Tone", Description = "tone tags" });

        db.Tags.AddRange(
            new TagEntity
            {
                Id = Ids.TagMatte,
                Name = "Matte",
                TagCategoryId = Ids.CategoryFinish,
                Hex = "#111111",
                Effect = "flat",
                Description = "matte",
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            },
            new TagEntity
            {
                Id = Ids.TagGlossy,
                Name = "Glossy",
                TagCategoryId = Ids.CategoryFinish,
                Hex = "#222222",
                Effect = "shine",
                Description = "glossy",
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            },
            new TagEntity
            {
                Id = Ids.TagWarm,
                Name = "Warm",
                TagCategoryId = Ids.CategoryTone,
                Hex = "#aa5500",
                Effect = "warm",
                Description = "warm",
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            },
            new TagEntity
            {
                Id = Ids.TagCool,
                Name = "Cool",
                TagCategoryId = Ids.CategoryTone,
                Hex = "#0055aa",
                Effect = "cool",
                Description = "cool",
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = true
            });

        db.Paints.AddRange(
            new PaintEntity
            {
                Id = Ids.PaintP1,
                Name = "Alpha Red",
                ModelNumber = "A-001",
                ModelNumberPrefix = "A",
                ModelNumberNumber = 1,
                BrandId = Ids.BrandA,
                PaintTypeId = Ids.TypeAcrylic,
                GlossId = Ids.GlossMatte,
                Price = 100m,
                Description = "p1",
                Hex = "#ff0000",
                RgbR = 255,
                RgbG = 0,
                RgbB = 0,
                HslH = 0f,
                HslS = 1f,
                HslL = 0.5f,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            },
            new PaintEntity
            {
                Id = Ids.PaintP2,
                Name = "Bravo Red",
                ModelNumber = "A-002",
                ModelNumberPrefix = "A",
                ModelNumberNumber = 2,
                BrandId = Ids.BrandA,
                PaintTypeId = Ids.TypeEnamel,
                GlossId = Ids.GlossGloss,
                Price = 120m,
                Description = "p2",
                Hex = "#ff1100",
                RgbR = 255,
                RgbG = 17,
                RgbB = 0,
                HslH = 4f,
                HslS = 1f,
                HslL = 0.5f,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            },
            new PaintEntity
            {
                Id = Ids.PaintP3,
                Name = "Charlie Blue",
                ModelNumber = "B-001",
                ModelNumberPrefix = "B",
                ModelNumberNumber = 1,
                BrandId = Ids.BrandB,
                PaintTypeId = Ids.TypeAcrylic,
                GlossId = Ids.GlossMatte,
                Price = 130m,
                Description = "p3",
                Hex = "#0000ff",
                RgbR = 0,
                RgbG = 0,
                RgbB = 255,
                HslH = 240f,
                HslS = 1f,
                HslL = 0.5f,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            },
            new PaintEntity
            {
                Id = Ids.PaintDeleted,
                Name = "Deleted Paint",
                ModelNumber = "A-999",
                ModelNumberPrefix = "A",
                ModelNumberNumber = 999,
                BrandId = Ids.BrandA,
                PaintTypeId = Ids.TypeAcrylic,
                GlossId = Ids.GlossMatte,
                Price = 90m,
                Description = "deleted",
                Hex = "#eeeeee",
                RgbR = 238,
                RgbG = 238,
                RgbB = 238,
                HslH = 0f,
                HslS = 0f,
                HslL = 0.9f,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = true
            });

        db.PaintTags.AddRange(
            new PaintTagEntity { PaintId = Ids.PaintP1, TagId = Ids.TagMatte },
            new PaintTagEntity { PaintId = Ids.PaintP1, TagId = Ids.TagWarm },
            new PaintTagEntity { PaintId = Ids.PaintP2, TagId = Ids.TagGlossy },
            new PaintTagEntity { PaintId = Ids.PaintP2, TagId = Ids.TagWarm },
            new PaintTagEntity { PaintId = Ids.PaintP3, TagId = Ids.TagMatte },
            new PaintTagEntity { PaintId = Ids.PaintDeleted, TagId = Ids.TagMatte },
            new PaintTagEntity { PaintId = Ids.PaintDeleted, TagId = Ids.TagWarm });

        db.SaveChanges();
    }
}
