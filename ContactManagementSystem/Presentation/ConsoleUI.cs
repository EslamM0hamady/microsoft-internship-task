using ContactManagementSystem.Application.Services;
using ContactManagementSystem.Domain.Entities;

namespace ContactManagementSystem.Presentation
{
    // Console-based user interface for interacting with contacts.
    public class ConsoleUI
    {
        private readonly ContactService _service;

        public ConsoleUI(ContactService service)
        {
            _service = service;
        }

        // Main interaction loop showing the menu and dispatching actions.
        public void Run()
        {
            while (true)
            {
                ShowMenu();
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        AddContact();
                        break;
                    case "2":
                        EditContact();
                        break;
                    case "3":
                        DeleteContact();
                        break;
                    case "4":
                        ViewContact();
                        break;
                    case "5":
                        ListContacts();
                        break;
                    case "6":
                        SearchContact();
                        break;
                    case "7":
                        FilterContacts();
                        break;
                    case "8":
                        SaveContacts();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }

        // Write the main menu options to the console.
        private void ShowMenu()
        {
            Console.WriteLine("\n===== Contact Management =====");
            Console.WriteLine("1. Add Contact");
            Console.WriteLine("2. Edit Contact");
            Console.WriteLine("3. Delete Contact");
            Console.WriteLine("4. View Contact");
            Console.WriteLine("5. List Contacts");
            Console.WriteLine("6. Search");
            Console.WriteLine("7. Filter");
            Console.WriteLine("8. Save");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");
        }

        // Display a single contact in a readable format.
        private void PrintContact(Contact contact)
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine($"Id: {contact.Id}");
            Console.WriteLine($"Name: {contact.Name}");
            Console.WriteLine($"Phone: {contact.Phone}");
            Console.WriteLine($"Email: {contact.Email}");
            Console.WriteLine($"CreatedAt: {contact.CreatedAt}");
        }

        // Capture details from the user and add a new contact.
        private void AddContact()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine()?.Trim();

            Console.Write("Phone: ");
            var phone = Console.ReadLine()?.Trim();

            Console.Write("Email: ");
            var email = Console.ReadLine()?.Trim();

            var success = _service.AddContact(name, phone, email);

            Console.WriteLine(success
                ? "Contact added successfully!"
                : "Name, phone, and email are required.");
        }

        // Edit an existing contact selected by Id.
        private void EditContact()
        {
            Console.Write("Enter Contact Id: ");
            if (!Guid.TryParse(Console.ReadLine()?.Trim(), out Guid id))
            {
                Console.WriteLine("Invalid Id format.");
                return;
            }

            Console.Write("New Name: ");
            var name = Console.ReadLine()?.Trim();

            Console.Write("New Phone: ");
            var phone = Console.ReadLine()?.Trim();

            Console.Write("New Email: ");
            var email = Console.ReadLine()?.Trim();

            var success = _service.EditContact(id, name, phone, email);

            Console.WriteLine(success
                ? "Contact updated successfully."
                : "Contact not found or provided data is invalid.");
        }

        // Delete a contact selected by Id.
        private void DeleteContact()
        {
            Console.Write("Enter Contact Id: ");
            if (!Guid.TryParse(Console.ReadLine()?.Trim(), out Guid id))
            {
                Console.WriteLine("Invalid Id format.");
                return;
            }

            var success = _service.DeleteContact(id);

            Console.WriteLine(success ? "Contact deleted successfully." : "Contact not found.");
        }

        // Show a single contact selected by Id.
        private void ViewContact()
        {
            Console.Write("Enter Contact Id: ");
            if (!Guid.TryParse(Console.ReadLine()?.Trim(), out Guid id))
            {
                Console.WriteLine("Invalid Id format.");
                return;
            }

            var contact = _service.ViewContact(id);

            if (contact == null)
            {
                Console.WriteLine("Contact not found.");
                return;
            }

            PrintContact(contact);
        }

        // List all stored contacts.
        public void ListContacts()
        {
            var contacts = _service.ListContacts();

            if (!contacts.Any())
            {
                Console.WriteLine("No contacts available.");
                return;
            }

            foreach (var contact in contacts)
            {
                PrintContact(contact);
            }
        }

        // Search contacts by keyword across multiple fields.
        private void SearchContact()
        {
            Console.Write("Enter a keyword to search (Name/Phone/Email): ");
            var keyword = Console.ReadLine()?.Trim();

            var results = _service.Search(keyword);

            var resultsList = results.ToList();
            if (!resultsList.Any())
            {
                Console.WriteLine("No contacts found.");
                return;
            }

            foreach (var contact in resultsList)
            {
                PrintContact(contact);
            }
        }

        // Filter contacts using simple "starts with" criteria.
        private void FilterContacts()
        {
            Console.WriteLine("Filter Options:");
            Console.WriteLine("1. Name starts with");
            Console.WriteLine("2. Phone starts with");
            Console.Write("Choose: ");

            var choice = Console.ReadLine()?.Trim();

            Console.Write("Enter value: ");
            var value = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine("Filter value cannot be empty.");
                return;
            }

            var results = choice switch
            {
                "1" => _service.Filter(c =>
                        c.Name.StartsWith(value, StringComparison.OrdinalIgnoreCase)),
                "2" => _service.Filter(c =>
                        c.Phone.StartsWith(value, StringComparison.OrdinalIgnoreCase)),
                _ => new List<Contact>()
            };

            if (!results.Any())
            {
                Console.WriteLine("No contacts found.");
                return;
            }

            foreach (var contact in results)
            {
                PrintContact(contact);
            }
        }

        // Persist current contacts to disk.
        private void SaveContacts()
        {
            _service.Save();
            Console.WriteLine("Contacts saved successfully.");
        }
    }
}
