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
            try
            {
                var todos = await _todoService.GetAll();
                return Ok(new ApiResponse<List<Todo>>
                {
                    Success = todos != null,
                    Message = todos != null ? "Todos fetched" : "Todos not found",
                    Data = todos
                });
            }
            catch (System.Exception e)
            {
                
                return BadRequest(new ApiResponse<List<Todo>>
                {
                    Success = false,
                    Message = e.Message
                });
            }
            
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
                    Message= todo != null ? "Todo fetched" : "Todo not found",
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
        public async Task<ActionResult<Todo>> Add(Todo todo)
        {
            try
            {
                await _todoService.Add(todo);
                return Ok(new ApiResponse<Todo>
                {
                    Success = true,
                    Message = "Todo added",
                    Data = todo
                }); 
            }
            catch (System.Exception e)
            {
                return BadRequest(new ApiResponse<Todo>
                {
                    Success = false,
                    Message = e.Message
                });
            }
               
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Todo todo)
        {
            try
            {
                await _todoService.Update(id, todo);
                return Ok(new ApiResponse<Todo>
                { 
                    Success = true,
                    Message = "Todo updated",
                    Data = todo
                });
            }
            catch (System.Exception e)
            {
                return BadRequest(new ApiResponse<Todo>
                {
                    Success = false,
                    Message = e.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _todoService.Delete(id);
                return Ok(new ApiResponse<Todo>
                {
                    Success = true,
                    Message = "Todo deleted"
                });
            }
            catch (System.Exception e)
            {
                return BadRequest(new ApiResponse<Todo>
                {
                    Success = false,
                    Message = e.Message
                });
            }
        }
    }
}
