using Backend5.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Forms;

namespace Web.Controllers;

/// <summary>The labs a hospital works with.</summary>
public class HospitalLabsController(HospitalService hospitals, LabService labs) : FormController
{
    public async Task<IActionResult> Index(int hospitalId)
    {
        var hospital = await hospitals.GetAsync(hospitalId);
        ViewBag.Hospital = hospital;
        return View(hospital.Labs.OrderBy(l => l.Lab.Name));
    }

    public async Task<IActionResult> Create(int hospitalId) => await ShowForm(hospitalId, new HospitalLabCreateForm());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int hospitalId, HospitalLabCreateForm model)
    {
        IActionResult showForm = await ShowForm(hospitalId, model);
        return await SubmitAsync(
            () => hospitals.AttachLabAsync(hospitalId, model.LabId),
            () => RedirectToAction(nameof(Index), new { hospitalId }),
            () => showForm);
    }

    public async Task<IActionResult> Delete(int hospitalId, int labId)
    {
        var hospital = await hospitals.GetAsync(hospitalId);
        var link = hospital.Labs.SingleOrDefault(l => l.LabId == labId);
        return link is null ? NotFound() : View(link);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int hospitalId, int labId)
    {
        await hospitals.DetachLabAsync(hospitalId, labId);
        return RedirectToAction(nameof(Index), new { hospitalId });
    }

    private async Task<IActionResult> ShowForm(int hospitalId, HospitalLabCreateForm model)
    {
        ViewBag.Hospital = await hospitals.GetAsync(hospitalId);
        ViewBag.LabId = new SelectList(await labs.ListAsync(), "Id", "Name", model.LabId);
        return View(nameof(Create), model);
    }
}
