using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;
using TestImtahan.DAL;
using TestImtahan.Models;

namespace TestImtahan.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private AppDbContext _context { get; set; }
        private IWebHostEnvironment _env { get; set; }
        public SliderController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<ClientsComments> comments = await _context.Comments.ToListAsync();
            return View(comments);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientsComments comments)
        {
            string fileName = comments.FullName + comments.Image.FileName;
            string path = Path.Combine();
            using (FileStream fs = new FileStream(Path.Combine(_env.WebRootPath, "assets/image", fileName), FileMode.Create))
            {
                comments.Image.CopyTo(fs);
            };
            comments.ImageUrl = fileName;
            await _context.Comments.AddAsync(comments);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
