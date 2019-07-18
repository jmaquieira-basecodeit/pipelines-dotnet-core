using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        // GET api/todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> Get()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET api/todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> Get(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST api/todo
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/todo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/todo/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
