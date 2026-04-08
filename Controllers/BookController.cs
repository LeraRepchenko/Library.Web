using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Library.Web.Data;
using Library.Web.Models;
using Library.Web.Services;

namespace Library.Web.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly LibraryDbContext _context;

        public BookController(IBookService bookService, LibraryDbContext context)
        {
            _bookService = bookService;
            _context = context;
        }

        // GET: /Book
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBooksAsync();
            return View(books);
        }

        // GET: /Book/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        // GET: /Book/Create
        public async Task<IActionResult> Create()
        {
            await LoadViewBags();
            return View();
        }

        // POST: /Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                await _bookService.CreateBookAsync(book);
                return RedirectToAction(nameof(Index));
            }

            await LoadViewBags();
            return View(book);
        }

        // GET: /Book/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();
            await LoadViewBags(book.AuthorId, book.GenreId);
            return View(book);
        }

        // POST: /Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var updated = await _bookService.UpdateBookAsync(book);
                if (updated == null) return NotFound();
                return RedirectToAction(nameof(Index));
            }

            await LoadViewBags(book.AuthorId, book.GenreId);
            return View(book);
        }

        // POST: /Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _bookService.DeleteBookAsync(id);
            if (!success) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadViewBags(int? selectedAuthorId = null, int? selectedGenreId = null)
        {
            ViewBag.Authors = new SelectList(
                await _context.Authors.ToListAsync(),
                "Id", "FullName", selectedAuthorId);

            ViewBag.Genres = new SelectList(
                await _context.Genres.ToListAsync(),
                "Id", "Name", selectedGenreId);
        }
    }
}