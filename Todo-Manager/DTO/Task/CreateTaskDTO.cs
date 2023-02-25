namespace Todo_Manager.DTO.Task
{
    public class CreateTaskDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        
        public ICollection<Guid> UserIds { get; set; }
    }
}
