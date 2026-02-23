using ContactManagementSystem.Domain.Entities;
using ContactManagementSystem.Domain.Interfaces;
using System.Text.Json;

namespace ContactManagementSystem.Infrastructure
{
	public class JsonContactRepository : IContactRepository
	{
		private Dictionary<Guid, Contact> _contacts = new Dictionary<Guid, Contact>();
		private readonly string _filePath;

		public JsonContactRepository(string filePath)
		{
			_filePath = filePath;
		}

		public void Add(Contact contact)
		{
			_contacts.Add(contact.Id, contact);
		}

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

		public bool Delete(Guid id)
		{
			return _contacts.Remove(id);
		}

		public Contact? GetById(Guid id)
		{
			if (_contacts.TryGetValue(id, out Contact? contact))
			{
				return contact;
			}

			return null;
		}

		public IEnumerable<Contact> GetAll()
		{
			return _contacts.Values;
		}

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

		public void Load()
		{
			if (!File.Exists(_filePath) || new FileInfo(_filePath).Length == 0)
			{
				return;
			}

			var json = File.ReadAllText(_filePath);
			var list = JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();

			_contacts = list.ToDictionary(c => c.Id);
		}

		public IEnumerable<Contact> Find(Func<Contact, bool> predicate)
		{
			return _contacts.Values.Where(predicate);
		}
	}
}

