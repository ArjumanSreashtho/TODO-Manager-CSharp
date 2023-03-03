using Todo_Manager.DTO.User;
using Todo_Manager.Models;

namespace Todo_Manager.DTO.Task
{
    public class CreateTaskDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        
        public ICollection<Guid> Users { get; set; }
    }
}
