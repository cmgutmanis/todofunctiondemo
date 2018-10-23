using System;
using System.Collections.Generic;
using System.Text;

namespace TodoFunction
{
    public class Todo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Description { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public bool IsCompleted { get; set; }
    }

    public class TodoCreateModel
    {
        public string Description { get; set; }
    }

    public class TodoUpdateModel
    {
        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
