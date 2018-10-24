using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace TodoFunction
{
    public static class TodoApi
    {
        static List<Todo> todos = new List<Todo>();

        [FunctionName("CreateTodo")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")]HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating a new todo item");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);
            if (string.IsNullOrEmpty(input.Description))
                return new BadRequestResult();

            var todo = new Todo { Description = input.Description };
            todos.Add(todo);
            return new OkObjectResult(todo);
        }

        [FunctionName("GetAllTodos")]
        public static IActionResult GetAllTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="todo")]HttpRequest req, ILogger log)
        {
            log.LogInformation("Getting All Todos.");

            return new OkObjectResult(todos);
        }

        [FunctionName("GetTodo")]
        public static IActionResult GetTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id}")]HttpRequest req, ILogger log, string id)
        {
            log.LogInformation("Getting todo");
            var todo = todos.FirstOrDefault(t => t.Id == id);

            return new OkObjectResult(todo);
        }

        [FunctionName("UpdateTodo")]
        public static async Task<IActionResult> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo/{id}")]HttpRequest req, ILogger log, string id)
        {
            log.LogInformation("updating todo");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var input = JsonConvert.DeserializeObject<TodoUpdateModel>(requestBody);
            var todo = todos.FirstOrDefault(t => t.Id == id);
            todo.IsCompleted = input.IsCompleted;
            todo.Description = input.Description;

            return new OkObjectResult(todo);
        }

        [FunctionName("DeleteTodo")]
        public static IActionResult DeleteTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route ="todo/{id}")]HttpRequest req, ILogger log, string id)
        {
            todos.Remove(todos.FirstOrDefault(t => t.Id == id));

            return new OkResult();
        }
    }
}
