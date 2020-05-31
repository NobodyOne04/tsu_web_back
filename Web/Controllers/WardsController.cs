using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;
using Web.Forms;

namespace Web.Controllers
{
    public class WardsController : Controller
    {
        private readonly ApplicationDbContext context;

        public WardsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.View(await this.context.Wards.ToListAsync());
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Hospital = hospital;
            var wards = await this.context.Wards
                .Include(w => w.Hospital)
                .Where(x => x.HospitalId == hospitalId)
                .ToListAsync();

            return this.View(wards);
        }

        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards
                .Include(w => w.Hospital)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return this.NotFound();
            }

            return this.View(ward);
        }

        public async Task<IActionResult> Create(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Hospital = hospital;
            return this.View(new WardCreateForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? hospitalId, WardCreateForm model)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var ward = new Ward
                {
                    HospitalId = hospital.Id,
                    Name = model.Name
                };

                this.context.Add(ward);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { hospitalId = hospital.Id });
            }

            this.ViewBag.Hospital = hospital;
            return this.View(model);
        }

        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards.SingleOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return this.NotFound();
            }

            var model = new WardEditForm
            {
                Name = ward.Name
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, WardEditForm model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards.SingleOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                ward.Name = model.Name;
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { hospitalId = ward.HospitalId });
            }

            return this.View(model);
        }

        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards
                .Include(w => w.Hospital)
                .Include(w => w.WardStaffs)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return this.NotFound();
            }

            return this.View(ward);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var ward = await this.context.Wards.SingleOrDefaultAsync(m => m.Id == id);
            this.context.Wards.Remove(ward);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { hospitalId = ward.HospitalId });
        }
    }
}
