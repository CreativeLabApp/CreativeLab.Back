using CreativeLab.Domain;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Persistence;

public class DbInitializer
{
    public static void Initialize(CreativeLabDbContext context)
    {
        context.Database.EnsureCreated();
        SeedData(context);
    }

    private static void SeedData(CreativeLabDbContext context)
    {
        // --- Categories ---
        var categories = new List<Category>
        {
            new() { Id = Guid.Parse("11111111-0000-0000-0000-000000000001"), Name = "Живопись",           Order = 1 },
            new() { Id = Guid.Parse("11111111-0000-0000-0000-000000000002"), Name = "Столярное дело",     Order = 2 },
            new() { Id = Guid.Parse("11111111-0000-0000-0000-000000000003"), Name = "Цифровое искусство", Order = 3 },
            new() { Id = Guid.Parse("11111111-0000-0000-0000-000000000004"), Name = "Керамика",           Order = 4 },
            new() { Id = Guid.Parse("11111111-0000-0000-0000-000000000005"), Name = "Вязание",            Order = 5 },
            new() { Id = Guid.Parse("11111111-0000-0000-0000-000000000006"), Name = "Фотография",         Order = 6 },
        };
        foreach (var cat in categories)
            if (!context.Categories.Any(c => c.Id == cat.Id))
                context.Categories.Add(cat);

        // --- Users ---
        var users = new List<User>
        {
            new() { Id = Guid.Parse("22222222-0000-0000-0000-000000000000"), Name = "Admin",    Surname = "CreativeLab", Email = "admin@creativelab.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin12345#") },
            new() { Id = Guid.Parse("22222222-0000-0000-0000-000000000001"), Name = "Мария",    Surname = "Левченко",    Email = "maria@example.com",     PasswordHash = "stub" },
            new() { Id = Guid.Parse("22222222-0000-0000-0000-000000000002"), Name = "Иван",     Surname = "Козлов",      Email = "ivan@example.com",      PasswordHash = "stub" },
            new() { Id = Guid.Parse("22222222-0000-0000-0000-000000000003"), Name = "Анна",     Surname = "Грин",        Email = "anna@example.com",      PasswordHash = "stub" },
            new() { Id = Guid.Parse("22222222-0000-0000-0000-000000000004"), Name = "Ольга",    Surname = "Смирнова",    Email = "olga@example.com",      PasswordHash = "stub" },
            new() { Id = Guid.Parse("22222222-0000-0000-0000-000000000005"), Name = "Дмитрий",  Surname = "Волков",      Email = "dmitry@example.com",    PasswordHash = "stub" },
        };
        foreach (var u in users)
            if (!context.Users.Any(x => x.Id == u.Id))
                context.Users.Add(u);

        context.SaveChanges();

        // --- Masterclass Materials ---
        var mcMaterials = new List<MasterclassMaterial>
        {
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000001"), Name = "Акварель" },
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000002"), Name = "Бумага" },
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000003"), Name = "Кисти" },
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000004"), Name = "Дерево" },
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000005"), Name = "Стамески" },
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000006"), Name = "Наждачная бумага" },
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000007"), Name = "iPad" },
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000008"), Name = "Procreate" },
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000009"), Name = "Глина" },
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000010"), Name = "Гончарный круг" },
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000011"), Name = "Пряжа" },
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000012"), Name = "Крючок" },
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000013"), Name = "Фотоаппарат" },
            new() { Id = Guid.Parse("33333333-0000-0000-0000-000000000014"), Name = "Lightroom" },
        };
        foreach (var m in mcMaterials)
            if (!context.MasterclassMaterials.Any(x => x.Id == m.Id))
                context.MasterclassMaterials.Add(m);

        context.SaveChanges();

        var mat = (Func<string, MasterclassMaterial>)(id =>
            context.MasterclassMaterials.Find(Guid.Parse(id))!);

        // --- Masterclasses ---
        var masterclasses = new List<Masterclass>
        {
            new() {
                Id = Guid.Parse("AAAAAAAA-0000-0000-0000-000000000001"),
                Title = "Акварельный пейзаж",
                Description = "Подробный курс по созданию атмосферных пейзажей акварелью: от выбора бумаги и кистей до финальных деталей.",
                ShortDescription = "Научитесь создавать атмосферные пейзажи акварелью",
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000001"),
                AuthorId = Guid.Parse("22222222-0000-0000-0000-000000000001"),
                ImageUrls = ["https://images.unsplash.com/photo-1579783902614-a3fb3927b6a5?w=800&auto=format&fit=crop", "https://images.unsplash.com/photo-1541961017774-22349e4a1262?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1579783902614-a3fb3927b6a5?w=800&auto=format&fit=crop",
                Views = 1240, Rating = 0, RatingsCount = 87, IsPublished = true,
                PublishedAt = DateTime.UtcNow.AddDays(-30), CreatedAt = DateTime.UtcNow.AddDays(-35),
                Materials = [mat("33333333-0000-0000-0000-000000000001"), mat("33333333-0000-0000-0000-000000000002"), mat("33333333-0000-0000-0000-000000000003")],
            },
            new() {
                Id = Guid.Parse("AAAAAAAA-0000-0000-0000-000000000002"),
                Title = "Деревянная игрушка своими руками",
                Description = "Создание деревянных игрушек с нуля: инструменты, техники резьбы, шлифовка и покраска.",
                ShortDescription = "Создание деревянных игрушек с нуля",
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000002"),
                AuthorId = Guid.Parse("22222222-0000-0000-0000-000000000002"),
                ImageUrls = ["https://images.unsplash.com/photo-1596461404969-9ae70f2830c1?w=800&auto=format&fit=crop", "https://images.unsplash.com/photo-1589939705384-5185137a7f0f?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1596461404969-9ae70f2830c1?w=800&auto=format&fit=crop",
                Views = 980, Rating = 0, RatingsCount = 54, IsPublished = true,
                PublishedAt = DateTime.UtcNow.AddDays(-20), CreatedAt = DateTime.UtcNow.AddDays(-25),
                Materials = [mat("33333333-0000-0000-0000-000000000004"), mat("33333333-0000-0000-0000-000000000005"), mat("33333333-0000-0000-0000-000000000006")],
            },
            new() {
                Id = Guid.Parse("AAAAAAAA-0000-0000-0000-000000000003"),
                Title = "Цифровая иллюстрация в Procreate",
                Description = "Профессиональная иллюстрация на iPad: слои, кисти, цветокоррекция и экспорт готовых работ.",
                ShortDescription = "Профессиональная иллюстрация на iPad",
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000003"),
                AuthorId = Guid.Parse("22222222-0000-0000-0000-000000000003"),
                ImageUrls = ["https://images.unsplash.com/photo-1611224923853-80b023f02d71?w=800&auto=format&fit=crop", "https://images.unsplash.com/photo-1545235617-9465d2a55698?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1611224923853-80b023f02d71?w=800&auto=format&fit=crop",
                Views = 3100, Rating = 0, RatingsCount = 210, IsPublished = true,
                PublishedAt = DateTime.UtcNow.AddDays(-15), CreatedAt = DateTime.UtcNow.AddDays(-18),
                Materials = [mat("33333333-0000-0000-0000-000000000007"), mat("33333333-0000-0000-0000-000000000008")],
            },
            new() {
                Id = Guid.Parse("AAAAAAAA-0000-0000-0000-000000000004"),
                Title = "Керамика для начинающих",
                Description = "Лепка на гончарном круге: центровка глины, вытягивание стенок, обжиг и глазурование.",
                ShortDescription = "Лепка на гончарном круге с нуля",
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000004"),
                AuthorId = Guid.Parse("22222222-0000-0000-0000-000000000004"),
                ImageUrls = ["https://images.unsplash.com/photo-1565193566173-7a0ee3dbe261?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1565193566173-7a0ee3dbe261?w=800&auto=format&fit=crop",
                Views = 760, Rating = 0, RatingsCount = 43, IsPublished = true,
                PublishedAt = DateTime.UtcNow.AddDays(-10), CreatedAt = DateTime.UtcNow.AddDays(-12),
                Materials = [mat("33333333-0000-0000-0000-000000000009"), mat("33333333-0000-0000-0000-000000000010")],
            },
            new() {
                Id = Guid.Parse("AAAAAAAA-0000-0000-0000-000000000005"),
                Title = "Вязание крючком: базовый курс",
                Description = "От выбора пряжи и крючка до первого готового изделия. Основные петли, схемы и разбор типичных ошибок.",
                ShortDescription = "Основы вязания крючком для начинающих",
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000005"),
                AuthorId = Guid.Parse("22222222-0000-0000-0000-000000000005"),
                ImageUrls = ["https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=800&auto=format&fit=crop",
                Views = 520, Rating = 4.5m, RatingsCount = 31, IsPublished = true,
                PublishedAt = DateTime.UtcNow.AddDays(-5), CreatedAt = DateTime.UtcNow.AddDays(-7),
                Materials = [mat("33333333-0000-0000-0000-000000000011"), mat("33333333-0000-0000-0000-000000000012")],
            },
            new() {
                Id = Guid.Parse("AAAAAAAA-0000-0000-0000-000000000006"),
                Title = "Портретная фотография",
                Description = "Работа со светом, постановка кадра, настройки камеры и базовая ретушь в Lightroom.",
                ShortDescription = "Свет, кадр и ретушь в портретной съёмке",
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000006"),
                AuthorId = Guid.Parse("22222222-0000-0000-0000-000000000001"),
                ImageUrls = ["https://images.unsplash.com/photo-1531746020798-e6953c6e8e04?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1531746020798-e6953c6e8e04?w=800&auto=format&fit=crop",
                Views = 1850, Rating = 0, RatingsCount = 120, IsPublished = true,
                PublishedAt = DateTime.UtcNow.AddDays(-8), CreatedAt = DateTime.UtcNow.AddDays(-9),
                Materials = [mat("33333333-0000-0000-0000-000000000013"), mat("33333333-0000-0000-0000-000000000014")],
            },
        };
        foreach (var mc in masterclasses)
            if (!context.Masterclasses.Any(x => x.Id == mc.Id))
                context.Masterclasses.Add(mc);

        context.SaveChanges();

        // --- Product Materials ---
        var productMaterials = new List<ProductMaterial>
        {
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000001"), Name = "Акварель" },
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000002"), Name = "Хлопковая бумага" },
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000003"), Name = "Масляные краски" },
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000004"), Name = "Холст" },
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000005"), Name = "Липа" },
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000006"), Name = "Акриловые краски" },
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000007"), Name = "Глина" },
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000008"), Name = "Глазурь" },
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000009"), Name = "Полимерная глина" },
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000010"), Name = "Фурнитура" },
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000011"), Name = "Мериносовая шерсть" },
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000012"), Name = "Соевый воск" },
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000013"), Name = "Эфирные масла" },
            new() { Id = Guid.Parse("44444444-0000-0000-0000-000000000014"), Name = "Цифровой файл" },
        };
        foreach (var pm in productMaterials)
            if (!context.ProductMaterials.Any(x => x.Id == pm.Id))
                context.ProductMaterials.Add(pm);

        context.SaveChanges();

        var pmat = (Func<string, ProductMaterial>)(id =>
            context.ProductMaterials.Find(Guid.Parse(id))!);

        // --- Products ---
        var products = new List<Product>
        {
            new() {
                Id = Guid.Parse("BBBBBBBB-0000-0000-0000-000000000001"),
                Title = "Акварельная картина «Осенний лес»",
                Description = "Ручная работа акварелью на хлопковой бумаге. Тёплые осенние тона, размер 30×40 см.",
                ShortDescription = "Акварель на хлопковой бумаге, 30×40 см",
                SellerId = Guid.Parse("22222222-0000-0000-0000-000000000001"),
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000001"),
                Price = 120m, IsAvailable = true, StockQuantity = 1,
                ImageUrls = ["https://images.unsplash.com/photo-1578301978693-85fa9c0320b9?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1578301978693-85fa9c0320b9?w=800&auto=format&fit=crop",
                Dimensions = "30×40 см", Weight = 0.3m, CreatedAt = DateTime.UtcNow.AddDays(-20),
                Materials = [pmat("44444444-0000-0000-0000-000000000001"), pmat("44444444-0000-0000-0000-000000000002")],
            },
            new() {
                Id = Guid.Parse("BBBBBBBB-0000-0000-0000-000000000002"),
                Title = "Деревянная игрушка «Ёжик»",
                Description = "Экологичная игрушка из натуральной липы, ручная роспись акриловыми красками.",
                ShortDescription = "Игрушка из липы с ручной росписью",
                SellerId = Guid.Parse("22222222-0000-0000-0000-000000000002"),
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000002"),
                Price = 45m, IsAvailable = true, StockQuantity = 3,
                ImageUrls = ["https://images.unsplash.com/photo-1596461404969-9ae70f2830c1?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1596461404969-9ae70f2830c1?w=800&auto=format&fit=crop",
                Dimensions = "15×10×8 см", Weight = 0.2m, CreatedAt = DateTime.UtcNow.AddDays(-15),
                Materials = [pmat("44444444-0000-0000-0000-000000000005"), pmat("44444444-0000-0000-0000-000000000006")],
            },
            new() {
                Id = Guid.Parse("BBBBBBBB-0000-0000-0000-000000000003"),
                Title = "Цифровая иллюстрация «Космос»",
                Description = "Иллюстрация в стиле sci-fi, создана в Procreate. Разрешение 3000×4000 px, PNG.",
                ShortDescription = "Цифровой файл 3000×4000 px, PNG",
                SellerId = Guid.Parse("22222222-0000-0000-0000-000000000003"),
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000003"),
                Price = 95m, IsAvailable = true, StockQuantity = 999,
                ImageUrls = ["https://images.unsplash.com/photo-1618005198919-d3d4b5a92ead?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1618005198919-d3d4b5a92ead?w=800&auto=format&fit=crop",
                Dimensions = "3000×4000 px", CreatedAt = DateTime.UtcNow.AddDays(-10),
                Materials = [pmat("44444444-0000-0000-0000-000000000014")],
            },
            new() {
                Id = Guid.Parse("BBBBBBBB-0000-0000-0000-000000000004"),
                Title = "Керамическая кружка «Уют»",
                Description = "Ручная лепка, покрыта матовой глазурью. Объём 350 мл.",
                ShortDescription = "Ручная лепка, 350 мл, матовая глазурь",
                SellerId = Guid.Parse("22222222-0000-0000-0000-000000000004"),
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000004"),
                Price = 65m, IsAvailable = true, StockQuantity = 5,
                ImageUrls = ["https://images.unsplash.com/photo-1514228742587-6b1558fcf93a?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1514228742587-6b1558fcf93a?w=800&auto=format&fit=crop",
                Dimensions = "Ø10×12 см", Weight = 0.4m, CreatedAt = DateTime.UtcNow.AddDays(-8),
                Materials = [pmat("44444444-0000-0000-0000-000000000007"), pmat("44444444-0000-0000-0000-000000000008")],
            },
            new() {
                Id = Guid.Parse("BBBBBBBB-0000-0000-0000-000000000005"),
                Title = "Украшение «Весенние цветы»",
                Description = "Комплект серьги + колье из полимерной глины. Гипоаллергенная фурнитура.",
                ShortDescription = "Серьги и колье из полимерной глины",
                SellerId = Guid.Parse("22222222-0000-0000-0000-000000000004"),
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000001"),
                Price = 80m, IsAvailable = true, StockQuantity = 2,
                ImageUrls = ["https://images.unsplash.com/photo-1599643478518-a784e5dc4c8f?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1599643478518-a784e5dc4c8f?w=800&auto=format&fit=crop",
                Dimensions = "Разные", Weight = 0.05m, CreatedAt = DateTime.UtcNow.AddDays(-5),
                Materials = [pmat("44444444-0000-0000-0000-000000000009"), pmat("44444444-0000-0000-0000-000000000010")],
            },
            new() {
                Id = Guid.Parse("BBBBBBBB-0000-0000-0000-000000000006"),
                Title = "Шерстяной шарф «Зимняя сказка»",
                Description = "Ручное вязание из 100% мериносовой шерсти. Размер 180×30 см.",
                ShortDescription = "Вязаный шарф из мериносовой шерсти",
                SellerId = Guid.Parse("22222222-0000-0000-0000-000000000005"),
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000005"),
                Price = 110m, IsAvailable = true, StockQuantity = 4,
                ImageUrls = ["https://images.unsplash.com/photo-1594736797933-d100db3638df?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1594736797933-d100db3638df?w=800&auto=format&fit=crop",
                Dimensions = "180×30 см", Weight = 0.3m, CreatedAt = DateTime.UtcNow.AddDays(-3),
                Materials = [pmat("44444444-0000-0000-0000-000000000011")],
            },
            new() {
                Id = Guid.Parse("BBBBBBBB-0000-0000-0000-000000000007"),
                Title = "Картина маслом «Закат в горах»",
                Description = "Масляная живопись на льняном холсте, 50×70 см. Покрыта лаком.",
                ShortDescription = "Масло на холсте, 50×70 см",
                SellerId = Guid.Parse("22222222-0000-0000-0000-000000000001"),
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000001"),
                Price = 320m, IsAvailable = true, StockQuantity = 1,
                ImageUrls = ["https://images.unsplash.com/photo-1578301978018-3005759f48f7?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1578301978018-3005759f48f7?w=800&auto=format&fit=crop",
                Dimensions = "50×70 см", Weight = 2.0m, CreatedAt = DateTime.UtcNow.AddDays(-1),
                Materials = [pmat("44444444-0000-0000-0000-000000000003"), pmat("44444444-0000-0000-0000-000000000004")],
            },
            new() {
                Id = Guid.Parse("BBBBBBBB-0000-0000-0000-000000000008"),
                Title = "Ароматические свечи «Лаванда»",
                Description = "Набор из 3 свечей на соевом воске с натуральными эфирными маслами. Горение — 30 ч каждая.",
                ShortDescription = "3 свечи на соевом воске с лавандой",
                SellerId = Guid.Parse("22222222-0000-0000-0000-000000000005"),
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000001"),
                Price = 35m, IsAvailable = true, StockQuantity = 10,
                ImageUrls = ["https://images.unsplash.com/photo-1585320806297-9794b3e4eeae?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1585320806297-9794b3e4eeae?w=800&auto=format&fit=crop",
                Dimensions = "Ø6×8 см каждая", Weight = 0.5m, CreatedAt = DateTime.UtcNow,
                Materials = [pmat("44444444-0000-0000-0000-000000000012"), pmat("44444444-0000-0000-0000-000000000013")],
            },
            new() {
                Id = Guid.Parse("BBBBBBBB-0000-0000-0000-000000000009"),
                Title = "Гончарный набор «Начинающий гончар»",
                Description = "Набор для работы с глиной: глина, инструменты, подробная инструкция.",
                ShortDescription = "Глина + инструменты для начинающих",
                SellerId = Guid.Parse("22222222-0000-0000-0000-000000000004"),
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000004"),
                Price = 145m, IsAvailable = false, StockQuantity = 0,
                ImageUrls = ["https://images.unsplash.com/photo-1574732011388-155ac2c0f2c9?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1574732011388-155ac2c0f2c9?w=800&auto=format&fit=crop",
                Dimensions = "30×40×10 см", Weight = 3.0m, CreatedAt = DateTime.UtcNow.AddDays(-40),
                Materials = [pmat("44444444-0000-0000-0000-000000000007")],
            },
            new() {
                Id = Guid.Parse("BBBBBBBB-0000-0000-0000-000000000010"),
                Title = "Фотография «Городские огни»",
                Description = "Профессиональная фотография ночного города, печать на холсте 40×60 см.",
                ShortDescription = "Печать на холсте, 40×60 см",
                SellerId = Guid.Parse("22222222-0000-0000-0000-000000000001"),
                CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000006"),
                Price = 175m, IsAvailable = false, StockQuantity = 0,
                ImageUrls = ["https://images.unsplash.com/photo-1519681393784-d120267933ba?w=800&auto=format&fit=crop"],
                ThumbnailUrl = "https://images.unsplash.com/photo-1519681393784-d120267933ba?w=800&auto=format&fit=crop",
                Dimensions = "40×60 см", Weight = 1.2m, CreatedAt = DateTime.UtcNow.AddDays(-60),
                Materials = [pmat("44444444-0000-0000-0000-000000000004")],
            },
        };
        foreach (var p in products)
            if (!context.Products.Any(x => x.Id == p.Id))
                context.Products.Add(p);

        context.SaveChanges();
    }
}
