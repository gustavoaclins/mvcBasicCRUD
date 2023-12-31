﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvcBasicCRUD.Data;
using mvcBasicCRUD.Models;
using NuGet.Common;

namespace mvcBasicCRUD.Controllers
{
    public class ChoresController : Controller
    {
        private readonly ProjectDBContext _context;

        public ChoresController(ProjectDBContext context)
        {
            _context = context;
        }

        // GET: Chores
        public async Task<IActionResult> Index()
        {
            List<Chore> chores = _context.Chores!.ToList();

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


            return View(choresView);
        }

        // GET: Chores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Chores == null)
            {
                return NotFound();
            }

            var chore = await _context.Chores
                .Include(c => c.ChoreType)
                .FirstOrDefaultAsync(m => m.ChoreID == id);
            if (chore == null)
            {
                return NotFound();
            }

            return View(chore);
        }

        // GET: Chores/Create
        public IActionResult Create()
        {
            ViewData["ChoreTypeID"] = new SelectList(_context.ChoreTypes, "ChoreTypeID", "Name");
            return View();
        }

        // POST: Chores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChoreID,Title,DueDate,IsCompleted,ChoreTypeID")] Chore chore)
        {
            var choreOverlap = _context.Chores!.Where(c => c.Title == chore.Title && c.DueDate.Date == chore.DueDate.Date);

            if (choreOverlap.Any())
            {
                ModelState.AddModelError("", $"There is already a Chore with the name due on this date.");
                ViewData["ChoreTypeID"] = new SelectList(_context.ChoreTypes, "ChoreTypeID", "Name", chore.ChoreTypeID);
                return View(chore);
            }

            if (ModelState.IsValid)
            {
                _context.Add(chore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChoreTypeID"] = new SelectList(_context.ChoreTypes, "ChoreTypeID", "Name", chore.ChoreTypeID);
            return View(chore);
        }

        // GET: Chores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Chores == null)
            {
                return NotFound();
            }

            var chore = await _context.Chores.FindAsync(id);
            if (chore == null)
            {
                return NotFound();
            }
            ViewData["ChoreTypeID"] = new SelectList(_context.ChoreTypes, "ChoreTypeID", "Name", chore.ChoreTypeID);
            return View(chore);
        }

        // POST: Chores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChoreID,Title,DueDate,IsCompleted,ChoreTypeID")] Chore chore)
        {
            if (id != chore.ChoreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var choreOverlap = _context.Chores!.Where(c => c.Title == chore.Title && c.DueDate.Date == chore.DueDate.Date && c.ChoreID != chore.ChoreID);

                if (choreOverlap.Any())
                {
                    ModelState.AddModelError("", $"There is already a Chore with the name due on this date.");
                    ViewData["ChoreTypeID"] = new SelectList(_context.ChoreTypes, "ChoreTypeID", "Name", chore.ChoreTypeID);
                    return View(chore);
                }

                try
                {
                    _context.Update(chore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChoreExists(chore.ChoreID))
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
            ViewData["ChoreTypeID"] = new SelectList(_context.ChoreTypes, "ChoreTypeID", "Name", chore.ChoreTypeID);
            return View(chore);
        }

        // GET: Chores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Chores == null)
            {
                return NotFound();
            }

            var chore = await _context.Chores
                .Include(c => c.ChoreType)
                .FirstOrDefaultAsync(m => m.ChoreID == id);
            if (chore == null)
            {
                return NotFound();
            }

            return View(chore);
        }

        // POST: Chores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chores == null)
            {
                return Problem("Entity set 'ProjectDBContext.Chores'  is null.");
            }
            var chore = await _context.Chores.FindAsync(id);
            if (chore != null)
            {
                _context.Chores.Remove(chore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChoreExists(int id)
        {
          return (_context.Chores?.Any(e => e.ChoreID == id)).GetValueOrDefault();
        }

        private string ChoreStatus(bool completed, DateTime due)
        {
            if (completed)
            {
                return "Completed";
            }
            else if (!completed && due.Date <=  DateTime.Now.Date)
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
