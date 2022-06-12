using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;
using System.Linq;
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
            using (FileStream fs = new FileStream(Path.Combine(_env.WebRootPath, "assets/image", fileName), FileMode.Create))
            {
                comments.Image.CopyTo(fs);
            }
            comments.ImageUrl = fileName;
            await _context.Comments.AddAsync(comments);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            ClientsComments comment = _context.Comments.Find(id);
            if(comment == null) return NotFound();
            return View(comment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientsComments newComment)
        {
            if (newComment == null) return NotFound();
            ClientsComments oldComment = _context.Comments.Find(newComment.Id);
            string deletePath = Path.Combine(_env.WebRootPath, "assets", "image" + oldComment.ImageUrl);
            if (System.IO.File.Exists(deletePath))
            {
                System.IO.File.Delete(deletePath);
            }
            string fileName = newComment.FullName + newComment.Image.FileName;
            using (FileStream fs = new FileStream(Path.Combine(_env.WebRootPath, "assets/image", fileName), FileMode.Create))
            {
                newComment.Image.CopyTo(fs);
            }
            oldComment.FullName = newComment.FullName;
            oldComment.Comment = newComment.Comment;
            oldComment.ImageUrl = fileName;
            oldComment.Position = newComment.Position;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            ClientsComments deletedComment = _context.Comments.Find(id);
            if (deletedComment == null) return NotFound();
            _context.Comments.Remove(deletedComment);
            _context.SaveChanges();
            string deletePath = Path.Combine(_env.WebRootPath, "assets", "image" + deletedComment.ImageUrl);
            if (System.IO.File.Exists(deletePath))
            {
                System.IO.File.Delete(deletePath);
            }
            return RedirectToAction("Index");
        }
    }
}
