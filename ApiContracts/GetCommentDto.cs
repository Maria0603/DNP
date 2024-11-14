namespace ApiContracts;

public class GetCommentDto {
    public int Id { get; set; }
    public string Body { get; set; }
    public int PostId { get; set; }
    public int AuthorId { get; set; }
    public string AuthorUsername { get; set; }
}