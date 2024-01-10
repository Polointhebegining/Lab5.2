
using Microsoft.AspNetCore.Mvc;
using Lab2;
using System.Collections.Generic;

[ApiController]
[Route("[controller]")]
public class TodoTasksController : ControllerBase
{
    private readonly TodoList _todoList;

    public TodoTasksController()
    {
        // This should ideally be replaced with a proper data access mechanism
        _todoList = new TodoList();
    }

    [HttpGet]
    public IEnumerable<TodoTask> Get()
    {
        return _todoList.Tasks;
    }

    [HttpGet("{id}")]
    public ActionResult<TodoTask> Get(int id)
    {
        var task = _todoList.GetTaskById(id);
        if (task == null)
        {
            return NotFound();
        }
        return task;
    }
    
    [HttpPost]
    public ActionResult<TodoTask> Post([FromBody] TodoTask task)
    {
        _todoList.Add(task);
        return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] TodoTask task)
    {
        if (!_todoList.UpdateTask(id, task))
        {
            return NotFound();
        }
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (!_todoList.Delete(id))
        {
            return NotFound();
        }
        return NoContent();
    }
}
