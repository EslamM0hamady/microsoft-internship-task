using ContactManagementSystem.Domain.Entities;
using ContactManagementSystem.Domain.Interfaces;
using System.Text.Json;

namespace ContactManagementSystem.Infrastructure
{
    // File-based repository that stores contacts as JSON on disk.
    public class JsonContactRepository : IContactRepository
    {
        // In-memory index keyed by contact Id.
        private Dictionary<Guid, Contact> _contacts = new Dictionary<Guid, Contact>();
        private readonly string _filePath;

        public JsonContactRepository(string filePath)
        {
            _filePath = filePath;
        }

        // Add a new contact to the in-memory index.
        public void Add(Contact contact)
        {
            _contacts.Add(contact.Id, contact);
        }

        // Update contact fields by Id; returns false if the Id does not exist.
        public bool Update(Guid id, string newName, string newPhone, string newEmail)
        {
            var contact = GetById(id);
            if (contact == null)
            {
                return false;
            }

            contact.Name = newName;
            contact.Phone = newPhone;
            contact.Email = newEmail;

            return true;
        }

        // Remove a contact from the index by Id.
        public bool Delete(Guid id)
        {
            return _contacts.Remove(id);
        }

        // Try to get a contact from the index by Id.
        public Contact? GetById(Guid id)
        {
            if (_contacts.TryGetValue(id, out Contact? contact))
            {
                return contact;
            }

            return null;
        }

        // Return all contacts currently loaded in memory.
        public IEnumerable<Contact> GetAll()
        {
            return _contacts.Values;
        }

        // Serialize the in-memory contacts to the JSON file.
        public void Save()
        {
            var directory = Path.GetDirectoryName(_filePath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var list = _contacts.Values.ToList();

            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_filePath, json);
        }

        // Load contacts from the JSON file into the in-memory index.
        public void Load()
        {
            if (!File.Exists(_filePath) || new FileInfo(_filePath).Length == 0)
            {
                return;
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                var list = JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();

                _contacts = list.ToDictionary(c => c.Id);
            }
            catch (Exception)
            {
                // If the file is corrupted or unreadable, fall back to an empty set of contacts.
                _contacts = new Dictionary<Guid, Contact>();
            }
        }

        // Apply an in-memory predicate over all contacts.
        public IEnumerable<Contact> Find(Func<Contact, bool> predicate)
        {
            return _contacts.Values.Where(predicate);
        }
    }
}

