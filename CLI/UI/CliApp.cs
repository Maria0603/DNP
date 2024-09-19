using RepositoryContracts;

namespace CLI.UI;

public class CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository) {
    public async Task StartAsync() {
        throw new NotImplementedException();
    }
}