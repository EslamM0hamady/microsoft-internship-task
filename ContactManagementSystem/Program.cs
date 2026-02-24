using ContactManagementSystem.Application.Services;
using ContactManagementSystem.Infrastructure;
using ContactManagementSystem.Presentation;

namespace ContactManagementSystem
{
	public class Program
	{
		public static void Main(string[] args)
		{
			string filePath = Path.GetFullPath(
				Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\contacts.json"));

			JsonContactRepository repository = new JsonContactRepository(filePath);
			repository.Load();

			ContactService service = new ContactService(repository);

			ConsoleUI console = new ConsoleUI(service);
			console.ListContacts();

			console.Run();
		}
	}
}
