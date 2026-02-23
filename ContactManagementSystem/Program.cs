using ContactManagementSystem.Application.Services;
using ContactManagementSystem.Infrastructure;
using ContactManagementSystem.Presentation;

namespace ContactManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            JsonContactRepository repository = new JsonContactRepository("E:\\C#Applications\\ContactManagementSystem\\ContactManagementSystem\\Data\\contacts.json");
            repository.Load();

            ContactService service = new ContactService(repository);

            ConsoleUI console = new ConsoleUI(service);
            console.ListContacts();

            console.Run();
        }
    }
}
