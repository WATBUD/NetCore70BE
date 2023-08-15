using Microsoft.AspNetCore.Mvc;
using NetCore60.Models;
using System.Collections.Generic;
using System.Linq;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private static List<TodoItem> _items = new List<TodoItem>();
        private static long _nextId = 1;

        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> Get()
        {
            return _items;
        }

        [HttpGet("{id}")]
        public ActionResult<TodoItem> GetById(long id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public ActionResult<TodoItem> Create(TodoItem item)
        {
            item.Id = _nextId++;
            _items.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, TodoItem updatedItem)
        {
            var existingItem = _items.FirstOrDefault(i => i.Id == id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = updatedItem.Name;
            existingItem.IsComplete = updatedItem.IsComplete;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var itemToRemove = _items.FirstOrDefault(i => i.Id == id);
            if (itemToRemove == null)
            {
                return NotFound();
            }
            _items.Remove(itemToRemove);
            return NoContent();
        }
    }
}
