using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InveDB.Datos;
using InveDB.Modelos;

namespace InveDB.Controles
{
    public class MovimientoesController : Controller
    {
        private readonly ContextoAlmacen _context;

        public MovimientoesController(ContextoAlmacen context)
        {
            _context = context;
        }

        // GET: Movimientoes
        public async Task<IActionResult> Index()
        {
            var contextoAlmacen = _context.Movimientos.Include(m => m.Producto).Include(m => m.Proveedor).Include(m => m.Sucursal);
            return View(await contextoAlmacen.ToListAsync());
        }

        // GET: Movimientoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.Movimientos
                .Include(m => m.Producto)
                .Include(m => m.Proveedor)
                .Include(m => m.Sucursal)
                .FirstOrDefaultAsync(m => m.IdMovimiento == id);
            if (movimiento == null)
            {
                return NotFound();
            }

            return View(movimiento);
        }

        // GET: Movimientoes/Create
        public IActionResult Create()
        {
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre");
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Nombre");
            ViewData["IdSucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "Nombre");
            return View();
        }

        // POST: Movimientoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMovimiento,IdProducto,Tipo,Cantidad,Fecha,IdProveedor,IdSucursal")] Movimiento movimiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movimiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", movimiento.IdProducto);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Nombre", movimiento.IdProveedor);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "Nombre", movimiento.IdSucursal);
            return View(movimiento);
        }

        // GET: Movimientoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.Movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", movimiento.IdProducto);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Nombre", movimiento.IdProveedor);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "Nombre", movimiento.IdSucursal);
            return View(movimiento);
        }

        // POST: Movimientoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMovimiento,IdProducto,Tipo,Cantidad,Fecha,IdProveedor,IdSucursal")] Movimiento movimiento)
        {
            if (id != movimiento.IdMovimiento)
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
                    if (!MovimientoExists(movimiento.IdMovimiento))
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
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", movimiento.IdProducto);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Nombre", movimiento.IdProveedor);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "Nombre", movimiento.IdSucursal);
            return View(movimiento);
        }

        // GET: Movimientoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.Movimientos
                .Include(m => m.Producto)
                .Include(m => m.Proveedor)
                .Include(m => m.Sucursal)
                .FirstOrDefaultAsync(m => m.IdMovimiento == id);
            if (movimiento == null)
            {
                return NotFound();
            }

            return View(movimiento);
        }

        // POST: Movimientoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movimiento = await _context.Movimientos.FindAsync(id);
            if (movimiento != null)
            {
                _context.Movimientos.Remove(movimiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovimientoExists(int id)
        {
            return _context.Movimientos.Any(e => e.IdMovimiento == id);
        }
    }
}
