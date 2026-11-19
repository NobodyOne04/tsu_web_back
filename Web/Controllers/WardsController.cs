using Backend5.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Forms;

namespace Web.Controllers;

public class WardsController(WardService wards, HospitalService hospitals) : FormController
{
    public async Task<IActionResult> Index(int hospitalId)
    {
        ViewBag.Hospital = await hospitals.GetAsync(hospitalId);
        return View(await wards.ListAsync(hospitalId));
    }

    public async Task<IActionResult> Details(int id) => View(await wards.GetAsync(id));

    public async Task<IActionResult> Create(int hospitalId)
    {
        ViewBag.Hospital = await hospitals.GetAsync(hospitalId);
        return View(new WardCreateForm());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int hospitalId, WardCreateForm model)
    {
        ViewBag.Hospital = await hospitals.GetAsync(hospitalId);
        return await SubmitAsync(
            () => wards.CreateAsync(hospitalId, model.Name),
            () => RedirectToAction(nameof(Index), new { hospitalId }),
            () => View(model));
    }

    public async Task<IActionResult> Edit(int id) => View(new WardEditForm { Name = (await wards.GetAsync(id)).Name });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, WardEditForm model)
    {
        var hospitalId = (await wards.GetAsync(id)).HospitalId;
        return await SubmitAsync(
            () => wards.RenameAsync(id, model.Name),
            () => RedirectToAction(nameof(Index), new { hospitalId }),
            () => View(model));
    }

    public async Task<IActionResult> Delete(int id) => View(await wards.GetAsync(id));

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var hospitalId = (await wards.GetAsync(id)).HospitalId;
        await wards.DeleteAsync(id);
        return RedirectToAction(nameof(Index), new { hospitalId });
    }
}
