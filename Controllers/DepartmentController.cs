using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Models;

namespace Blog.Controllers;


public class DepartmentController : Controller
{
    private readonly DBContext context;

    public DepartmentController(DBContext context)
    {
        this.context = context;
    }

    public IActionResult GetAll()
        {
            var departments = context.Departments
             .Include(d => d.Authors)
             .ToList();
             return View("GetAll", departments);
        }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Department department)
    {
        if (ModelState.IsValid)
        {
            context.Departments.Add(department);
            context.SaveChanges();
            return RedirectToAction("GetAll"); 
        }

        return View(department); 
    }

    public IActionResult Edit(int id)
    {
        var department = context.Departments.FirstOrDefault(d => d.ID == id);
        if (department == null)
        {
            return NotFound();
        }
        
        ViewBag.Authors = context.Authors.ToList(); 
        return View(department);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Department department)
    {
        if (id != department.ID)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(department);
                context.SaveChanges();
                return RedirectToAction("GetAll");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Departments.Any(d => d.ID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        ViewBag.Authors = context.Authors.ToList(); 
        return View(department);
    }
}
