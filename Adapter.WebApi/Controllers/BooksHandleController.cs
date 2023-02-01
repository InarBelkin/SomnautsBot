using Core.Interfaces.Driving;
using Core.Models.Book;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksHandleController : ControllerBase
{
    private readonly IBooksService _booksService;

    public BooksHandleController(IBooksService booksService)
    {
        _booksService = booksService;
    }

    [HttpPost("scan")]
    public async Task<ActionResult<ScanBooksResultModel>> Scan(ScanBooksParamsModel dto)
    {
        var result = await _booksService.ScanAvailableBooks(dto);
        return result;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetLists()
    {
        var result = await _booksService.GetListOfBooks();
        return Ok(result);
    }
}