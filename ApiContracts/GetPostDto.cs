using System.Collections.Generic;

namespace ApiContracts;

public class GetPostDto {
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }

    public int AuthorId { get; set; }
    public string AuthorName { get; set; }
    
    public List<GetCommentDto> Comments { get; set; }

    //TODO: should also return all the comments of a post
    //TODO: should also Author corresponding to the post
}