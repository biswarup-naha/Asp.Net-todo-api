using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController(TodoService todoService) : ControllerBase
    {
        private readonly TodoService _todoService = todoService;

        [HttpGet]
        public ActionResult<List<Todo>> GetAll()
        {
            var todos = _todoService.GetAll();
            return Ok(todos);
        }
    }
}
