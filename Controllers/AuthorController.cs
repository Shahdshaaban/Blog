using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Models;
using System.Linq;

public class AuthorController : Controller
{
    private readonly DBContext _context;

    public AuthorController(DBContext context)
    {
        _context = context;
    }

    public IActionResult GetAll()
    {
        var authors = _context.Authors.Include(a => a.Department).ToList();
        return View(authors);
    }


public IActionResult Create()
{
    ViewBag.Authors = _context.Authors.ToList(); 
    ViewBag.Departments = _context.Departments.ToList();
    return View();
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(Author author)
{
    if (ModelState.IsValid)
    {
        _context.Authors.Add(author);
        _context.SaveChanges();
        return RedirectToAction("GetAll");
    }

    ViewBag.Authors = _context.Authors.ToList();
    ViewBag.Departments = _context.Departments.ToList();
    return View(author);
}



public IActionResult Edit(int id)
{
    var author = _context.Authors.FirstOrDefault(a => a.ID == id);
    if (author == null)
    {
        return NotFound();
    }
    ViewBag.Departments = _context.Departments.ToList();
    return View(author);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Edit(int id, Author author)
{
    if (id != author.ID)
    {
        return BadRequest();
    }

    if (ModelState.IsValid)
    {
        try
        {
            _context.Update(author);
            _context.SaveChanges();
            return RedirectToAction("GetAll");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Authors.Any(a => a.ID == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    ViewBag.Departments = _context.Departments.ToList();
    return View(author);
}


    public IActionResult Delete(int id)
    {
        var author = _context.Authors.FirstOrDefault(a => a.ID == id);
        if (author == null)
        {
            return NotFound();
        }
        return View(author);
    }

    [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult DeleteConfirmed(int id)
{
    var author = _context.Authors.Find(id);
    if (author != null)
    {
        _context.Authors.Remove(author);
        _context.SaveChanges();
    }
    return RedirectToAction("GetAll");
}

}
