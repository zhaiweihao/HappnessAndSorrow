using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreTest_1010.Data;
using Microsoft.EntityFrameworkCore;
using CoreTest_1010.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreTest_1010.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;
        public StudentsController(SchoolContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            { return NotFound(); }
            var student = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ID == id);
            if (student == null) return NotFound();
            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName", "FirstMidName", "EnrollmentDate")]Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "unable to save changes" + "try again ,if the problem persits" + "see your administrator");
            }
            return View(student);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)//, [Bind("ID,EnrollmentDate,FirstMidName,LastName")] Student student)
        {
            #region oldCode
            //if (id == null)
            //{
            //    return NotFound();
            //}
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(student);
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction("Index");
            //    }
            //    catch (DbUpdateException /* ex */)
            //    {
            //        //Log the error (uncomment ex variable name and write a log.)
            //        ModelState.AddModelError("", "Unable to save changes. " +
            //            "Try again, and if the problem persists, " +
            //            "see your system administrator.");
            //    }
            //}
            //return View(student);
            #endregion

            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Students.SingleOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Student>(
                studentToUpdate,
                "",
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(studentToUpdate);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            { return NotFound(); }
            var student = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ID == id);
            if (student == null) return NotFound();
            return View(student);
        }
    }
}
