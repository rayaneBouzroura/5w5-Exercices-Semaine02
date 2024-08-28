﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using WebApplication1.Data;

namespace MVCEtWebAPI.Controllers
{
    [Authorize]
    public class TestDatasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestDatasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TestDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestData.ToListAsync());
        }

        // GET: TestDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testData = await _context.TestData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testData == null)
            {
                return NotFound();
            }

            return View(testData);
        }

        // GET: TestDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestDatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id")] TestData testData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testData);
        }

        // GET: TestDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testData = await _context.TestData.FindAsync(id);
            if (testData == null)
            {
                return NotFound();
            }
            return View(testData);
        }

        // POST: TestDatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id")] TestData testData)
        {
            if (id != testData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestDataExists(testData.Id))
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
            return View(testData);
        }

        // GET: TestDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testData = await _context.TestData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testData == null)
            {
                return NotFound();
            }

            return View(testData);
        }

        // POST: TestDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testData = await _context.TestData.FindAsync(id);
            if (testData != null)
            {
                _context.TestData.Remove(testData);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestDataExists(int id)
        {
            return _context.TestData.Any(e => e.Id == id);
        }
    }
}
