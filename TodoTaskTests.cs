using System;
using Xunit;
using System.IO;
using Lab2;
using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using System.Linq;

namespace HelloWorldTests
{
    public class TodoTaskTests
    {

        [Fact]
        public void AddTaskTest()
        {
            TodoList todoList = new TodoList();
            var task1 = new TodoTask { Title = "Task 1", Description = 
"Description 1", Deadline = DateTime.Today.AddDays(4), IsCompleted = 
false};
            var task2 = new TodoTask { Title = "Task 2", Description = 
"Description 2", Deadline = DateTime.Today.AddDays(3), IsCompleted = 
false};

            todoList.Add(task1);
            todoList.Add(task2);

            var ListToCompare = todoList.Tasks.ToList();

            Assert.Equal(2, ListToCompare.Count);
            Assert.Equal("Task 1", ListToCompare[0].Title);
            Assert.Equal("Description 2", ListToCompare[2].Description);
        }

        [Fact]
        public void DelTaskTest()
        {
            TodoList todoList = new TodoList();
            var task1 = new TodoTask { Title = "Task 1", Description = 
"Description 1", Deadline = DateTime.Today.AddDays(4), IsCompleted = false 
};
            var task2 = new TodoTask { Title = "Task 2", Description = 
"Description 2", Deadline = DateTime.Today.AddDays(3), IsCompleted = false 
};
            var task3 = new TodoTask { Title = "Task 3", Description = 
"Description 3", Deadline = DateTime.Today.AddDays(3), IsCompleted = false 
};

            todoList.Add(task1);
            todoList.Add(task2);
            todoList.Add(task3);

            var Before = todoList.Tasks.ToList();
            Assert.Equal(3, Before.Count);

            todoList.Remove(task1);

            var After = todoList.Tasks.ToList();
            Assert.Equal(2, After.Count);
            Assert.Equal("Task 2", After[0].Title);
        }

        [Fact]  
        public void CompletedTasksTest()
        {
            TodoList todoList = new TodoList();
            var task1 = new TodoTask { Title = "Task 1", Description = 
"Description 1", Deadline = DateTime.Today.AddDays(4), IsCompleted = false 
};
            var task2 = new TodoTask { Title = "Task 2", Description = 
"Description 2", Deadline = DateTime.Today.AddDays(4), IsCompleted = false 
};

            todoList.Add(task1);
            todoList.Add(task2);

            var ListTasks = todoList.CompletedTasks.ToList();
            Assert.Equal(0, ListTasks.Count);
        }

        public void UncompletedTasksTest()
        {
            TodoList todoList = new TodoList();
            var task1 = new TodoTask { Title = "Task 1", Description = 
"Description 1", Deadline = DateTime.Today.AddDays(4), IsCompleted = false 
};
            var task2 = new TodoTask { Title = "Task 2", Description = 
"Description 2", Deadline = DateTime.Today.AddDays(4), IsCompleted = false 
};

            todoList.Add(task1);
            todoList.Add(task2);

            var ListTasks = todoList.UncompletedTasks.ToList();
            Assert.Equal(2, ListTasks.Count);
        }

        public void SaveLoadJsonTest()
        {
            TodoList todoList = new TodoList();
            var task1 = new TodoTask { Title = "Task 1", Description = 
"Description 1", Deadline = DateTime.Today.AddDays(4), IsCompleted = false 
};
            var task2 = new TodoTask { Title = "Task 2", Description = 
"Description 2", Deadline = DateTime.Today.AddDays(4), IsCompleted = false 
};

            todoList.Add(task1);
            todoList.Add(task2);

            todoList.SaveToJSON();

            var TestList = new TodoList();
            TestList.LoadToJSON();

            var Tasks = todoList.Tasks.ToList();

            Assert.Equal(2, Tasks.Count);
            Assert.Equal("Task 1", Tasks[0].Title);
            Assert.Equal("Description 2", Tasks[2].Description);
        }

        public void SaveLoadXMLTest()
        {
            TodoList todoList = new TodoList();
            var task1 = new TodoTask { Title = "Task 1", Description = 
"Description 1", Deadline = DateTime.Today.AddDays(4), IsCompleted = false 
};
            var task2 = new TodoTask { Title = "Task 2", Description = 
"Description 2", Deadline = DateTime.Today.AddDays(4), IsCompleted = false 
};

            todoList.Add(task1);
            todoList.Add(task2);

            todoList.SaveToXML();

            var TestList = new TodoList();
            TestList.LoadToXML();

            var Tasks = todoList.Tasks.ToList();

            Assert.Equal(2, Tasks.Count);
            Assert.Equal("Task 1", Tasks[0].Title);
            Assert.Equal("Description 2", Tasks[2].Description);
        }

        public void SaveLoadSQLiteTest()
        {
            TodoList todoList = new TodoList();
            var task1 = new TodoTask { Title = "Task 1", Description = 
"Description 1", Deadline = DateTime.Today.AddDays(4), IsCompleted = false 
};
            var task2 = new TodoTask { Title = "Task 2", Description = 
"Description 2", Deadline = DateTime.Today.AddDays(4), IsCompleted = false 
};

            todoList.Add(task1);
            todoList.Add(task2);

            todoList.SaveToSQLite();

            var TestList = new TodoList();
            TestList.LoadToSQLite();

            var Tasks = todoList.Tasks.ToList();

            Assert.Equal(2, Tasks.Count);
            Assert.Equal("Task 1", Tasks[0].Title);
            Assert.Equal("Description 2", Tasks[2].Description);
        }
    }
}
