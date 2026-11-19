using Backend5.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Forms;

namespace Web.Controllers;

public class LabsController(LabService labs) : FormController
{
    public async Task<IActionResult> Index() => View(await labs.ListAsync());

    public async Task<IActionResult> Details(int id) => View(await labs.GetAsync(id));

    public IActionResult Create() => View(new LabCreateForm());

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Create(LabCreateForm model) => SubmitAsync(
        () => labs.CreateAsync(model.Name, model.Address, model.Phones),
        () => RedirectToAction(nameof(Index)),
        () => View(model));

    public async Task<IActionResult> Edit(int id)
    {
        var lab = await labs.GetAsync(id);
        return View(new LabEditForm { Name = lab.Name, Address = lab.Address, Phones = lab.PhonesText });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(int id, LabEditForm model) => SubmitAsync(
        () => labs.UpdateAsync(id, model.Name, model.Address, model.Phones),
        () => RedirectToAction(nameof(Index)),
        () => View(model));

    public async Task<IActionResult> Delete(int id) => View(await labs.GetAsync(id));

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await labs.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
