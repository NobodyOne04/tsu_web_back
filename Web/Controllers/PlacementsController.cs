using Backend5.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Forms;

namespace Web.Controllers;

/// <summary>Beds of a ward.</summary>
public class PlacementsController(WardService wards) : FormController
{
    public async Task<IActionResult> Index(int wardId)
    {
        var ward = await wards.GetAsync(wardId);
        ViewBag.Ward = ward;
        return View(ward.Placements.OrderBy(p => p.Bed));
    }

    public async Task<IActionResult> Details(int id) => View(await wards.GetPlacementAsync(id));

    public async Task<IActionResult> Create(int wardId)
    {
        ViewBag.Ward = await wards.GetAsync(wardId);
        return View(new PlacementForm());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int wardId, PlacementForm model)
    {
        ViewBag.Ward = await wards.GetAsync(wardId);
        return await SubmitAsync(
            () => wards.AddPlacementAsync(wardId, model.Bed),
            () => RedirectToAction(nameof(Index), new { wardId }),
            () => View(model));
    }

    public async Task<IActionResult> Edit(int id) => View(new PlacementForm { Bed = (await wards.GetPlacementAsync(id)).Bed });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PlacementForm model)
    {
        var wardId = (await wards.GetPlacementAsync(id)).WardId;
        return await SubmitAsync(
            () => wards.MovePlacementAsync(id, model.Bed),
            () => RedirectToAction(nameof(Index), new { wardId }),
            () => View(model));
    }

    public async Task<IActionResult> Delete(int id) => View(await wards.GetPlacementAsync(id));

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) =>
        RedirectToAction(nameof(Index), new { wardId = await wards.RemovePlacementAsync(id) });
}
