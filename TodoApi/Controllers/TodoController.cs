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
            return await _context.TodoItems.AsNoTracking().ToListAsync();
        }

        // GET api/todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> Get(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItems.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST api/todo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<TodoItem>> Post([Bind("Name")] TodoItem todoItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(todoItem);
                    await _context.SaveChangesAsync();
                    return todoItem;
                }
            }
            catch //(DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return todoItem;
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
