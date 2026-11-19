using Backend5.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Forms;

namespace Web.Controllers;

/// <summary>Staff of a ward.</summary>
public class WardStaffsController(WardService wards) : FormController
{
    public async Task<IActionResult> Index(int wardId)
    {
        var ward = await wards.GetAsync(wardId);
        ViewBag.Ward = ward;
        return View(ward.WardStaffs.OrderBy(s => s.Name));
    }

    public async Task<IActionResult> Details(int id) => View(await wards.GetStaffAsync(id));

    public async Task<IActionResult> Create(int wardId)
    {
        ViewBag.Ward = await wards.GetAsync(wardId);
        return View(new WardStaffForm());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int wardId, WardStaffForm model)
    {
        ViewBag.Ward = await wards.GetAsync(wardId);
        return await SubmitAsync(
            () => wards.AddStaffAsync(wardId, model.Name, model.Position),
            () => RedirectToAction(nameof(Index), new { wardId }),
            () => View(model));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var staff = await wards.GetStaffAsync(id);
        return View(new WardStaffForm { Name = staff.Name, Position = staff.Position });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, WardStaffForm model)
    {
        var wardId = (await wards.GetStaffAsync(id)).WardId;
        return await SubmitAsync(
            () => wards.UpdateStaffAsync(id, model.Name, model.Position),
            () => RedirectToAction(nameof(Index), new { wardId }),
            () => View(model));
    }

    public async Task<IActionResult> Delete(int id) => View(await wards.GetStaffAsync(id));

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) =>
        RedirectToAction(nameof(Index), new { wardId = await wards.RemoveStaffAsync(id) });
}
