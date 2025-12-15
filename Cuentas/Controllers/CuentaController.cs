using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cuentas.Data;
using Cuentas.Models;

namespace Cuentas.Controllers
{
    public class CuentaController : Controller
    {
        private readonly CuentasContext _context;

        public CuentaController(CuentasContext context)
        {
            _context = context;
        }

        // GET: Cuenta
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cuenta.ToListAsync());
        }

        // GET: Cuenta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.Cuenta
                .FirstOrDefaultAsync(m => m.id == id);
            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }

        // GET: Cuenta/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cuenta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,numero,descripcion,creditos,debitos,balance")] Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                cuenta.balance = cuenta.creditos - cuenta.debitos;

                _context.Add(cuenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cuenta);
        }

        // GET: Cuenta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            return View(cuenta);
        }

        // POST: Cuenta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id, descripcion")] Cuenta cuenta)
        {
            if (id != cuenta.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var cuentaOriginal = await _context.Cuenta.FindAsync(id);

                if (cuentaOriginal == null)
                {
                    return NotFound();
                }

                try
                {
                    
                    cuentaOriginal.descripcion = cuenta.descripcion;

                    
                    _context.Update(cuentaOriginal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuentaExists(cuenta.id))
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

            var cuentaParaVista = await _context.Cuenta.AsNoTracking().FirstOrDefaultAsync(c => c.id == id);
            return View(cuentaParaVista);
        }

        // GET: Cuenta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.Cuenta
                .FirstOrDefaultAsync(m => m.id == id);
            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }

        // POST: Cuenta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta != null)
            {
                _context.Cuenta.Remove(cuenta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CuentaExists(int id)
        {
            return _context.Cuenta.Any(e => e.id == id);
        }
    }
}
