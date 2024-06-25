using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControlAdmin_os.Common.Entities;
using ControlAdmin_os.Web.Data;
using System.Diagnostics.Metrics;

namespace ControlAdmin_os.Web.Controllers
{
    public class CountriesController : Controller
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            _context = context;
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////
        //Métodos de para Country //////////////////////////////////////////////////////////////////////////////
        
        
        // GET: Countries
        public async Task<IActionResult> Index()
        {
              return _context.Countries != null ? 
                          View(await _context.Countries
                          .Include(c=>c.Departments)
                          .ToListAsync()) :
                          Problem("Entity set 'DataContext.Countries'  is null.");
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .Include(c => c.Departments)!
                .ThenInclude(d => d.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }


        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Hay un registro con el mismo nombre");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Hay un registro con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Hay un registro con el mismo nombre.");
                        //ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .Include(c => c.Departments)!
                .ThenInclude(d=>d.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
           
            
            
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        //Métodos de para Department //////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> AddDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            Department model = new Department { IdCountry = country.Id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                var country = await _context.Countries
                    .Include(c => c.Departments)
                    .FirstOrDefaultAsync(c => c.Id == department.IdCountry);
                if (country == null)
                {
                    return NotFound();
                }

                try
                {
                    department.Id = 0;
                    country.Departments!.Add(department);
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                                        
                    ////return RedirectToAction($"{nameof(Details)}/{country.Id}");                
                    return RedirectToAction("Details", new { id = country.Id });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Hay un registro con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Hay un registro con el mismo nombre.");
                        //ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(department);
        }


        public async Task<IActionResult> EditDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Departments!.FirstOrDefault(d => d.Id == department.Id) != null);
            department.IdCountry = country!.Id;
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                   //return RedirectToAction($"{nameof(Details)}/{department.IdCountry}");
                    return RedirectToAction("Details", new {id=department.IdCountry});

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Hay un registro con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Hay un registro con el mismo nombre.");
                       // ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(department);
        }

        public async Task<IActionResult> DeleteDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Departments!.FirstOrDefault(d => d.Id == department.Id) != null);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            //return RedirectToAction($"{nameof(Details)}/{country!.Id}");
            return RedirectToAction("Details", new { id=country!.Id });
        }

        public async Task<IActionResult> DetailsDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Departments!.FirstOrDefault(d => d.Id == department.Id) != null);
            department.IdCountry = country!.Id;
            return View(department);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        //Métodos de para City //////////////////////////////////////////////////////////////////////////////

        public async Task<IActionResult> AddCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            City model = new City { IdDepartment = department.Id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCity(City city)
        {
            if (ModelState.IsValid)
            {
                var department = await _context.Departments
                    .Include(d => d.Cities)
                    .FirstOrDefaultAsync(c => c.Id == city.IdDepartment);
                if (department == null)
                {
                    return NotFound();
                }

                try
                {
                    city.Id = 0;
                    department.Cities!.Add(city);
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                    
                    //return RedirectToAction($"{nameof(DetailsDepartment)}/{department.Id}");
                    return RedirectToAction("DetailsDepartment", new { id = department.Id });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                        //ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(city);
        }


        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Cities!.FirstOrDefault(c => c.Id == city.Id) != null);
            city.IdDepartment = department!.Id;
            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCity(City city)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                    //return RedirectToAction($"{nameof(DetailsDepartment)}/{city.IdDepartment}");
                    return RedirectToAction("DetailsDepartment", new { id = city.IdDepartment });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                        //ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(city);
        }

        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Cities!.FirstOrDefault(c => c.Id == city.Id) != null);
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
           // return RedirectToAction($"{nameof(DetailsDepartment)}/{department!.Id}");
            return RedirectToAction("DetailsDepartment", new { id = department!.Id });
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////
        //


    }
}
