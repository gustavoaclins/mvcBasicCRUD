using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvcBasicCRUD.Data;
using mvcBasicCRUD.Models;

namespace mvcBasicCRUD.Controllers
{
    public class ChoreTypesController : Controller
    {
        private readonly ProjectDBContext _context;

        public ChoreTypesController(ProjectDBContext context)
        {
            _context = context;
        }

        // GET: ChoreTypes
        public async Task<IActionResult> Index()
        {
              return _context.ChoreTypes != null ? 
                          View(await _context.ChoreTypes.ToListAsync()) :
                          Problem("Entity set 'ProjectDBContext.ChoreTypes'  is null.");
        }

        // GET: ChoreTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ChoreTypes == null)
            {
                return NotFound();
            }

            var choreType = await _context.ChoreTypes
                .FirstOrDefaultAsync(m => m.ChoreTypeID == id);
            if (choreType == null)
            {
                return NotFound();
            }

            List<Chore> chores = _context.Chores!.Where(c => c.ChoreTypeID == id).ToList();

            List<ChoresViewModels> choresView = new List<ChoresViewModels>();

            foreach (Chore c in chores)
            {
                ChoresViewModels choreDetails = new ChoresViewModels
                {
                    Id = c.ChoreID,
                    Title = c.Title,
                    DueDate = c.DueDate,
                    ChoreType = c.ChoreType!.Name,
                    Status = ChoreStatus(c.IsCompleted, c.DueDate)
                };

                choresView.Add(choreDetails);
            }

            ViewData["Choures"] = choresView;

            return View(choreType);
        }

        // GET: ChoreTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChoreTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChoreTypeID,Name")] ChoreType choreType)
        {

            if (ModelState.IsValid)
            {

                var choreTypeWithName = _context.ChoreTypes!.Where(ctp => ctp.Name == choreType.Name);

                if (choreTypeWithName.Any())
                {
                    ModelState.AddModelError("", $"The is a Chore Type with name: {choreType.Name}");
                    return View(choreType);
                }

                _context.Add(choreType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(choreType);
        }

        // GET: ChoreTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ChoreTypes == null)
            {
                return NotFound();
            }

            var choreType = await _context.ChoreTypes.FindAsync(id);
            if (choreType == null)
            {
                return NotFound();
            }
            return View(choreType);
        }

        // POST: ChoreTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChoreTypeID,Name")] ChoreType choreType)
        {
            if (id != choreType.ChoreTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var choreTypeWithName = _context.ChoreTypes!.Where(ctp => ctp.Name == choreType.Name);

                if (choreTypeWithName.Any())
                {
                    ModelState.AddModelError("", $"The is a Chore Type with name: {choreType.Name}");
                    return View(choreType);
                }

                try
                {
                    _context.Update(choreType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChoreTypeExists(choreType.ChoreTypeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(choreType);
        }

        // GET: ChoreTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ChoreTypes == null)
            {
                return NotFound();
            }

            var choreType = await _context.ChoreTypes
                .FirstOrDefaultAsync(m => m.ChoreTypeID == id);
            if (choreType == null)
            {
                return NotFound();
            }

            return View(choreType);
        }

        // POST: ChoreTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ChoreTypes == null)
            {
                return Problem("Entity set 'ProjectDBContext.ChoreTypes'  is null.");
            }
            var choreType = await _context.ChoreTypes.FindAsync(id);
            if (choreType != null)
            {
                _context.ChoreTypes.Remove(choreType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChoreTypeExists(int id)
        {
          return (_context.ChoreTypes?.Any(e => e.ChoreTypeID == id)).GetValueOrDefault();
        }

        private string ChoreStatus(bool completed, DateTime due)
        {
            if (completed)
            {
                return "Completed";
            }
            else if (!completed && due.Date <= DateTime.Now.Date)
            {
                return "Overdue";
            }
            else
            {
                return "To Do";
            }
        }
    }
}
