using Microsoft.AspNetCore.Mvc;

namespace ImageResizer.Controllers;


[ApiController]
[Route("[controller]")]
public class TodoListController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodoListController(ITodoService todoService)
    {
       _todoService = todoService;
    }

    [HttpGet]
    public async Task<IEnumerable<TodoElement>> Get()
    {
        return await _todoService.GetTodosAsync();
    }

}