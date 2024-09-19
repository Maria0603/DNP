namespace Entities;

public class Comment : IEntity {
    public int Id { get; set; }
    public string Body { get; set; }
}