using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Models;
using System.Linq;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBContext _context;

        public HomeController(DBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
             var posts = _context.Posts.Include(p => p.Author)
                              .OrderByDescending(p => p.CreateDate)
                              .ToList();
            return View(posts);
        }
        

        public IActionResult CreatePost()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(Post post, IFormFile ImageFile)
        {
            if (ImageFile == null)
            {
                ModelState.AddModelError("", "Image file is not received.");
            }

            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads); 
                    }

                    var filePath = Path.Combine(uploads, ImageFile.FileName);

                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        post.ImagePath =  ImageFile.FileName; 
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Error saving image: {ex.Message}");
                    }
                }

                post.CreateDate = DateTime.Now; 
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            return View(post);
        }

      
        public IActionResult EditPost(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            return View(post);
        }


[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EditPost(int id, Post post, IFormFile ImageFile)
{
    if (id != post.ID)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        if (ImageFile != null && ImageFile.Length > 0)
        {
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads); 
            }

            var filePath = Path.Combine(uploads, ImageFile.FileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                post.ImagePath = ImageFile.FileName; 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error saving image: {ex.Message}");
                ViewBag.Authors = _context.Authors.ToList();
                ViewBag.Departments = _context.Departments.ToList();
                return View(post);
            }
        }
        else
        {
            var existingPost = await _context.Posts.FindAsync(id);
            post.ImagePath = existingPost.ImagePath; 
        }
        post.CreateDate = DateTime.Now; 
        _context.Update(post);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    ViewBag.Authors = _context.Authors.ToList();
    ViewBag.Departments = _context.Departments.ToList();
    return View(post);
}

        public IActionResult DeletePost(int id)
        {
            var post = _context.Posts.Include(p => p.Author).FirstOrDefault(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var post = _context.Posts.Find(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ViewAllAuthors()
        {
            return RedirectToAction("GetAll", "Author");
        }

        public IActionResult ViewAllDepartments()
        {
            return RedirectToAction("GetAll", "Department");
        }


        public IActionResult About()
        {
            return View();
        }

    }

    
}
