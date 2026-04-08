using Microsoft.AspNetCore.Identity;
using Library.Web.Models;

namespace Library.Web.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Reader>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.EnsureCreatedAsync();

            // Создаём роли
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Создаём администратора
            var adminEmail = "admin@library.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new Reader
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "Adminov",
                    RegistrationDate = DateTime.UtcNow
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Создаём тестового пользователя
            var userEmail = "user@library.com";
            Reader? user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                user = new Reader
                {
                    UserName = "user",
                    Email = userEmail,
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    RegistrationDate = DateTime.UtcNow
                };
                await userManager.CreateAsync(user, "User123!");
                await userManager.AddToRoleAsync(user, "User");
            }

            // Добавляем авторов
            if (!context.Authors.Any())
            {
                var authors = new[]
                {
                    new Author { FirstName = "Лев", LastName = "Толстой", BirthDate = new DateTime(1828, 9, 9), Country = "Россия" },
                    new Author { FirstName = "Фёдор", LastName = "Достоевский", BirthDate = new DateTime(1821, 11, 11), Country = "Россия" }
                };
                await context.Authors.AddRangeAsync(authors);
                await context.SaveChangesAsync();

                var genres = new[]
                {
                    new Genre { Name = "Роман", Description = "Художественное произведение" },
                    new Genre { Name = "Фантастика", Description = "Вымышленные миры" }
                };
                await context.Genres.AddRangeAsync(genres);
                await context.SaveChangesAsync();

                var books = new[]
                {
                    new Book { Title = "Война и мир", AuthorId = authors[0].Id, GenreId = genres[0].Id, PublicationYear = 1869 },
                    new Book { Title = "Преступление и наказание", AuthorId = authors[1].Id, GenreId = genres[0].Id, PublicationYear = 1866 }
                };
                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();

                // Создаём выдачу книги
                var loan = new Loan
                {
                    BookId = books[0].Id,
                    ReaderId = user.Id,
                    LoanDate = DateTime.UtcNow.AddDays(-5),
                    DueDate = DateTime.UtcNow.AddDays(9)
                };
                await context.Loans.AddAsync(loan);
                await context.SaveChangesAsync();
            }
        }
    }
}