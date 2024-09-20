// See https://aka.ms/new-console-template for more information

using CLI.UI;

using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI...");
IUserRepository userRepository = new UserInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository(userRepository);
ICommentRepository commentRepository = new CommentInMemoryRepository(userRepository, postRepository);

CliApp cliApp = new CliApp(userRepository, commentRepository, postRepository);
await cliApp.StartAsync();