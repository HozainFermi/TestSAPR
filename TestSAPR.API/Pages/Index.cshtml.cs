using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestSAPR.Application.DTOs.Part.Add;
using TestSAPR.Application.DTOs.Part.Delete;
using TestSAPR.Application.DTOs.Part.Rename;
using TestSAPR.Application.Interfaces;
using TestSAPR.Domain.Exceptions;
using TestSAPR.Domain.Model;

namespace TestSAPR.API.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IPartService _service;
        public List<Part> Parts { get; set; } = new();

        public IndexModel(IPartService service)
        {
            _service = service;
        }

        public async Task OnGetAsync()
        {
            Parts = await _service.GetTreeAsync(default);
        }

        public async Task<IActionResult> OnPostCreateAsync(string name)
        {
            try
            {
                await _service.AddNewPartAsync(new AddPartDto(name), default);
            }            
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await OnGetAsync();
                return Page();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRenameAsync(Guid part_id, string new_name)
        {
            try
            {           
                await _service.RenamePartAsync(new RenamePartDto(part_id, new_name), HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToPage();
        }


        public async Task<IActionResult> OnGetDeleteAsync(Guid id)
        {
            try 
            { 
                await _service.DeletePartAsync(new DeletePartDto(id), default); 
            }
            catch (Exception ex) 
            { 
                TempData["Error"] = ex.Message; 
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetExportAsync(Guid id)
        {
            var tree = await _service.GetTreeAsync(default);
            var target = FindInTree(tree, id);

            var content = ExportToExcel(target);
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{target.Name}.xlsx");
        }

        public async Task<IActionResult> OnPostAddChildAsync(Guid parent_id, string child_name, int quantity)
        {

            try
            {
                var dto = new AddNestedPartDto(parent_id, child_name, quantity);
                await _service.AddNestedPartAsync(dto, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToPage();
        }
        private byte[] ExportToExcel(Part part)
        {
            using var package = new OfficeOpenXml.ExcelPackage();
            var ws = package.Workbook.Worksheets.Add($"{part.Name}");        
            ws.Cells[1, 1].Value = "Наименование";
            ws.Cells[1, 2].Value = "Кол-во";

            int row = 2;
            FillExcelRow(part, ws, ref row);
            
            return package.GetAsByteArray();
        }

        private void FillExcelRow(Part p, OfficeOpenXml.ExcelWorksheet ws, ref int row)
        {            
            ws.Cells[row, 1].Value = p.Name;
            ws.Cells[row, 2].Value = p.Quantity;
            row++;
            foreach (var c in p.Children)
            {
                FillExcelRow(c, ws, ref row);
            }
        }

        private Part FindInTree(List<Part> nodes, Guid id)
        {
            foreach (var n in nodes)
            {
                if (n.Id == id) return n;
                var found = FindInTree(n.Children, id);
                if (found != null) return found;
            }
            return null;
        }
    }
}
