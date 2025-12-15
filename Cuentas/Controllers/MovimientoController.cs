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
    public class MovimientoController : Controller
    {
        private readonly CuentasContext _context;

        public MovimientoController(CuentasContext context)
        {
            _context = context;
        }

        // GET: Movimiento
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movimiento.ToListAsync());
        }

        // GET: Movimiento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.Movimiento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movimiento == null)
            {
                return NotFound();
            }

            return View(movimiento);
        }

        // GET: Movimiento/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movimiento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tipo,Concepto,Monto,Fecha,IdCuenta")] Movimiento movimiento)
        {
            if (!ModelState.IsValid)
            {
                return View(movimiento);
            }

            var cuenta = await _context.Cuenta.FirstOrDefaultAsync(m => m.id == movimiento.IdCuenta);

            if (cuenta == null || cuenta.balance <= 0)
            {
                ModelState.AddModelError("", "No es posible registrar el movimiento, cuenta inactiva.");
                return View(movimiento);
            }

                _context.Add(movimiento);

            if (cuenta != null)
                {
                    if (movimiento.Tipo == 1)
                    {
                        cuenta.setCredito(movimiento.Monto);
                    }
                    else
                    {
                        cuenta.setDebito(movimiento.Monto);
                    }

                    cuenta.setBalance();
                    _context.Update(cuenta);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
       
       
        

        // GET: Movimiento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.Movimiento.FindAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }
            return View(movimiento);
        }

        // POST: Movimiento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tipo,Concepto,Monto,Fecha,IdCuenta")] Movimiento movimiento)
        {
            if (id != movimiento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movimiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovimientoExists(movimiento.Id))
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
            return View(movimiento);
        }

        // GET: Movimiento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.Movimiento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movimiento == null)
            {
                return NotFound();
            }

            return View(movimiento);
        }

        // POST: Movimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movimiento = await _context.Movimiento.FindAsync(id);
            if (movimiento != null)
            {
                _context.Movimiento.Remove(movimiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovimientoExists(int id)
        {
            return _context.Movimiento.Any(e => e.Id == id);
        }
    }
}
