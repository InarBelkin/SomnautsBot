using Core.Interfaces.Driving;
using Core.Models.Book;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksHandleController : ControllerBase
{
    private readonly IBooksHandleService _booksHandleService;

    public BooksHandleController(IBooksHandleService booksHandleService)
    {
        _booksHandleService = booksHandleService;
    }

    [HttpPost("scan")]
    public async Task<ActionResult<ScanBooksResultModel>> Scan(ScanBooksParamsModel dto)
    {
        var result = await _booksHandleService.ScanAvailableBooks(dto);
        return result;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetLists()
    {
        var result = await _booksHandleService.GetListOfHandleBooks();
        return Ok(result);
    }
}