using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Text;
using TP_Final_MatÍasOlmos.Data;
using TP_Final_MatÍasOlmos.Models;
using TP_Final_MatÍasOlmos.ViewModels;

namespace TP_Final_MatÍasOlmos.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public ProductosController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Productos
        public async Task<IActionResult> Index(string busquedaNombre, int? busquedaCategoriaId, int? busquedaMarcaId)
        {
            var applicationDbContext = _context.productos.Include(p => p.categoria).Select(p => p).Include(p => p.marca).Include(p => p.proveedor).Select(p => p);

            //comienzan las validaciones
            //filtro por nombre
            if (!string.IsNullOrEmpty(busquedaNombre))
            {
                applicationDbContext = applicationDbContext.Where(a => a.nombre.Contains(busquedaNombre));
            }
            //filtro por categoria
            if (busquedaCategoriaId.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(a => a.categoriaId == busquedaCategoriaId);
            }
            //filtro por categoria
            if (busquedaMarcaId.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(a => a.marcaId == busquedaMarcaId);
            }

            ProductosViewModel modelo = new ProductosViewModel()
            {
                productos = applicationDbContext.ToList(),
                categorias = new SelectList(_context.categorias, "Id", "descripcion", busquedaCategoriaId),
                marca = new SelectList(_context.marcas, "Id", "descripcion", busquedaMarcaId),

                //proveedores = ICollection<Proveedor>
            };
            return View(modelo);
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.productos
                .Include(p => p.categoria)
                .Include(p => p.marca)
                .Include(p => p.proveedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["categoriaId"] = new SelectList(_context.categorias, "Id", "descripcion");
            ViewData["marcaId"] = new SelectList(_context.marcas, "Id", "descripcion");
            ViewData["proveedorId"] = new SelectList(_context.proveedores, "Id", "nombre");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre,precio,descripcion,urlImagen,favorito,categoriaId,marcaId,proveedorId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];
                    var pathDestino = Path.Combine(env.WebRootPath, "images\\productos");
                    if (archivoFoto.Length > 0)
                    {
                        //generar nombre aleatorio para el archivo
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFoto.FileName);

                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            producto.urlImagen = archivoDestino;
                        }
                    }
                }

                //Añade a la DB
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["categoriaId"] = new SelectList(_context.categorias, "Id", "Id", producto.categoriaId);
            ViewData["marcaId"] = new SelectList(_context.marcas, "Id", "Id", producto.marcaId);
            ViewData["proveedorId"] = new SelectList(_context.proveedores, "Id", "Id", producto.proveedorId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["categoriaId"] = new SelectList(_context.categorias, "Id", "descripcion", producto.categoriaId);
            ViewData["marcaId"] = new SelectList(_context.marcas, "Id", "descripcion", producto.marcaId);
            ViewData["proveedorId"] = new SelectList(_context.proveedores, "Id", "nombre", producto.proveedorId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre,precio,descripcion,urlImagen,favorito,categoriaId,marcaId,proveedorId")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];
                    var pathDestino = Path.Combine(env.WebRootPath, "images\\productos");
                    if (archivoFoto.Length > 0)
                    {
                        //generar nombre aleatorio para el archivo
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFoto.FileName);

                        if (!string.IsNullOrEmpty(producto.urlImagen))
                        {
                            string fotoAnterior = Path.Combine(pathDestino, producto.urlImagen);
                            if (System.IO.File.Exists(fotoAnterior))
                            {
                                System.IO.File.Delete(fotoAnterior);
                            }
                        }

                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            producto.urlImagen = archivoDestino;
                        }
                    }
                }

                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
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
            ViewData["categoriaId"] = new SelectList(_context.categorias, "Id", "Id", producto.categoriaId);
            ViewData["marcaId"] = new SelectList(_context.marcas, "Id", "Id", producto.marcaId);
            ViewData["proveedorId"] = new SelectList(_context.proveedores, "Id", "Id", producto.proveedorId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.productos
                .Include(p => p.categoria)
                .Include(p => p.marca)
                .Include(p => p.proveedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.productos.FindAsync(id);
            _context.productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.productos.Any(e => e.Id == id);
        }
    }
}
