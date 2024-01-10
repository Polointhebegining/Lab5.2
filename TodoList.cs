using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace Lab2
{

    public class TodoList
    {
        private readonly List<TodoTask> tasks = new List<TodoTask>();
        private const string JsonFilePath = "tasks.json";
        private const string XmlFilePath = "tasks.xml";
        private const string DbConnectionString = "Data Source=tasks.db";

        public IEnumerable<TodoTask> Tasks
        {
            get
            {
                return tasks;
            }
        }

        public IEnumerable<TodoTask> CompletedTasks
        {
            get
            {
                foreach (TodoTask task in tasks)
                {
                    if (task.IsCompleted)
                    {
                        yield return task;
                    }
                }
            }
        }

        public IEnumerable<TodoTask> UncompletedTasks
        {
            get
            {
                foreach (TodoTask task in tasks)
                {
                    if (!task.IsCompleted)
                    {
                        yield return task;
                    }
                }
            }
        }

        public void Add(TodoTask task)
        {
            tasks.Add(task);
        }

        public void Remove(TodoTask task)
        {
            tasks.Remove(task);
        }

        public IEnumerable<TodoTask> Search(string tag)
        {
            var result = new List<TodoTask>();

            foreach (var task in tasks)
            {
                if (task.Tags.Contains(tag))
                {
                    result.Add(task);
                    continue;
                }
            }

            return result;
        }

        //SAVING
        public void SaveToJSON()
        {
            var json = JsonSerializer.Serialize(tasks);
            File.WriteAllText(JsonFilePath, json);
        }

        public void SaveToXML()
        {
            var serial = new XmlSerializer(typeof(List<TodoTask>));
            using (var stream = new StreamWriter(XmlFilePath))
            {
                serial.Serialize(stream, tasks);
            }
        }

        public void SaveToSQLite()
        {
            using(var connect = new SqliteConnection(DbConnectionString))
            {
                connect.Open();

                using(var command = connect.CreateCommand())
                { 
                    command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Tasks (
                    Title TEXT,
                    Description TEXT,
                    Deadline TEXT,
                    IsCompleted INTEGER
                )";

                    command.ExecuteNonQuery();

                    foreach(var task in tasks)
                    {
                        command.CommandText = $@"
                    INSERT INTO Tasks (Title, Description, Deadline, 
IsCompleted)
                    VALUES ('{task.Title}', {task.Description}, 
'{task.Deadline:yyyy-MM-dd}', {(task.IsCompleted ? 1 : 0)})";

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        //LOADING
        public void LoadToJSON()
        {
            if (File.Exists(JsonFilePath))
            {
                var json = File.ReadAllText(JsonFilePath);
                tasks.Clear();
                
tasks.AddRange(JsonSerializer.Deserialize<List<TodoTask>>(json));
            }
        }

        public void LoadToXML()
        {
            if (File.Exists(JsonFilePath))
            {
                var serial = new XmlSerializer(typeof(List<TodoTask>));
                using (var stream = new StreamReader(XmlFilePath))
                {
                    tasks.Clear();

                    tasks.AddRange((List<TodoTask>)serial.Deserialize(stream));
                }
            }
        }

        public void LoadToSQLite()
        {
            using(var connect = new SqliteConnection(DbConnectionString))
            {
                connect.Open();

                using(var command = connect.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Tasks";

                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new TodoTask
                            {
                                Title = reader.GetString(0),
                                Description = reader.GetString(1),
                                Deadline = 
DateTime.Parse(reader.GetString(2)),
                                IsCompleted = reader.GetInt32(3) == 1
                            });
                        }
                    }
                }
            }
        }
    }

}
