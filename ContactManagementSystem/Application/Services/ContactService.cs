using ContactManagementSystem.Domain.Entities;
using ContactManagementSystem.Domain.Interfaces;

namespace ContactManagementSystem.Application.Services
{
    // Application layer for contact operations; delegates storage to IContactRepository.
    public class ContactService
    {
        private readonly IContactRepository _repository;

        public ContactService(IContactRepository repository)
        {
            _repository = repository;
        }

        // Create and store a new contact.
        public void AddContact(string name, string phone, string email)
        {
            var contact = new Contact(name, phone, email);
            _repository.Add(contact);
        }

        // Update basic fields; returns false if the contact is missing.
        public bool EditContact(Guid id, string newName, string newPhone, string newEmail)
        {
            return _repository.Update(id, newName, newPhone, newEmail);

        }

        // Delete a contact by Id.
        public bool DeleteContact(Guid id)
        {
            return _repository.Delete(id);
        }

        // Get a single contact, or null if not found.
        public Contact? ViewContact(Guid id)
        {
            return _repository.GetById(id);
        }

        // Return all contacts.
        public IEnumerable<Contact> ListContacts()
        {
            return _repository.GetAll();
        }

        // Case-insensitive search on name, phone and email; empty keyword returns all.
        public IEnumerable<Contact> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _repository.GetAll();

            return _repository.Find(c =>
                c.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                c.Phone.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                c.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        // Apply a custom in-memory filter.
        public IEnumerable<Contact> Filter(Func<Contact, bool> predicate)
        {
            return _repository.Find(predicate);
        }

        // Persist current state to the underlying store.
        public void Save()
        {
            _repository.Save();
        }
    }
}
