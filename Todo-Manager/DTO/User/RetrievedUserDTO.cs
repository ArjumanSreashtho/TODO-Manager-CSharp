namespace Todo_Manager.DTO.User;

public class RetrievedUserDTO
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    
    public string Name { get; set; }
    
    public string Role { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}