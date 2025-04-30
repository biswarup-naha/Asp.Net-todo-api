using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Utils;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController(TodoService todoService) : ControllerBase
    {
        private readonly TodoService _todoService = todoService;

        [HttpGet]
        public async Task<ActionResult<List<Todo>>> GetAll()
        {
            var todos = await _todoService.GetAll();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetById(string id)
        {
            try
            {
                var todo = await _todoService.GetById(id);
                return Ok(new ApiResponse<Todo>
                {
                    Success = todo != null,
                    Message= todo != null ? "Todo found" : "Todo not found",
                    Data = todo
                }); 
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<Todo>
                {
                    Success = false,
                    Message = e.Message
                });
            }
           
        }

        [HttpPost]
        public ActionResult Add(Todo todo)
        {
            _todoService.Add(todo);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Update(string id, Todo todo)
        {
            _todoService.Update(id, todo);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            _todoService.Delete(id);
            return Ok();
        }
    }
}
