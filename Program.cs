using Microsoft.EntityFrameworkCore;
using Library.Web.Data;
using Library.Web.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
SeedData(app.Services);

static void SeedData(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

        context.Database.Migrate();

        if (context.Authors.Any()) return;

        // Авторы
        var authors = new[]
        {
            new Author { FirstName = "Лев", LastName = "Толстой", BirthDate = new DateTime(1828, 9, 9), Country = "Россия" },
            new Author { FirstName = "Фёдор", LastName = "Достоевский", BirthDate = new DateTime(1821, 11, 11), Country = "Россия" },
            new Author { FirstName = "Джордж", LastName = "Оруэлл", BirthDate = new DateTime(1903, 6, 25), Country = "Великобритания" }
        };
        context.Authors.AddRange(authors);
        context.SaveChanges();

        // Жанры
        var genres = new[]
        {
            new Genre { Name = "Роман", Description = "Художественное произведение большого объёма" },
            new Genre { Name = "Фантастика", Description = "Литература о вымышленных мирах" },
            new Genre { Name = "Антиутопия", Description = "Изображение тоталитарного общества" }
        };
        context.Genres.AddRange(genres);
        context.SaveChanges();

        // Книги
        var books = new[]
        {
            new Book { Title = "Война и мир", AuthorId = authors[0].Id, GenreId = genres[0].Id, PublicationYear = 1869, PageCount = 1225, Publisher = "Русский вестник" },
            new Book { Title = "Анна Каренина", AuthorId = authors[0].Id, GenreId = genres[0].Id, PublicationYear = 1877, PageCount = 864, Publisher = "Русский вестник" },
            new Book { Title = "Преступление и наказание", AuthorId = authors[1].Id, GenreId = genres[0].Id, PublicationYear = 1866, PageCount = 671, Publisher = "Русский вестник" },
            new Book { Title = "1984", AuthorId = authors[2].Id, GenreId = genres[2].Id, PublicationYear = 1949, PageCount = 328, Publisher = "Secker & Warburg", Isbn = "978-0-452-28423-4" }
        };
        context.Books.AddRange(books);
        context.SaveChanges();

        // Читатели
        var readers = new[]
        {
            new Reader { FirstName = "Иван", LastName = "Петров", Email = "ivan@mail.ru", Phone = "+7-123-456-78-90" },
            new Reader { FirstName = "Мария", LastName = "Иванова", Email = "maria@mail.ru", Phone = "+7-987-654-32-10" }
        };
        context.Readers.AddRange(readers);
        context.SaveChanges();

        // Выдачи
        var loans = new[]
        {
            new Loan { BookId = books[0].Id, ReaderId = readers[0].Id, LoanDate = DateTime.UtcNow.AddDays(-5), DueDate = DateTime.UtcNow.AddDays(9) },
            new Loan { BookId = books[3].Id, ReaderId = readers[1].Id, LoanDate = DateTime.UtcNow.AddDays(-10), DueDate = DateTime.UtcNow.AddDays(4) }
        };
        context.Loans.AddRange(loans);
        context.SaveChanges();

        // Отзывы
        var reviews = new[]
        {
            new Review { BookId = books[0].Id, ReaderId = readers[0].Id, Rating = 5, Comment = "Гениально!" },
            new Review { BookId = books[3].Id, ReaderId = readers[1].Id, Rating = 5, Comment = "Актуально и сейчас" }
        };
        context.Reviews.AddRange(reviews);
        context.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();