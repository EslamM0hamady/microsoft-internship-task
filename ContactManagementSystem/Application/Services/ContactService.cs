using ContactManagementSystem.Domain.Entities;
using ContactManagementSystem.Domain.Interfaces;

namespace ContactManagementSystem.Application.Services
{
    public class ContactService
    {
        private readonly IContactRepository _repository;

        public ContactService(IContactRepository repository)
        {
            _repository = repository;
        }

        public void AddContact(string name, string phone, string email)
        {
            var contact = new Contact(name, phone, email);
            _repository.Add(contact);
        }

        public bool EditContact(Guid id, string newName, string newPhone, string newEmail)
        {
            return _repository.Update(id, newName, newPhone, newEmail);

        }

        public bool DeleteContact(Guid id)
        {
            return _repository.Delete(id);
        }

        public Contact? ViewContact(Guid id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Contact> ListContacts()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Contact> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _repository.GetAll();

            return _repository.Find(c =>
                c.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                c.Phone.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                c.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Contact> Filter(Func<Contact, bool> predicate)
        {
            return _repository.Find(predicate);
        }

        public void Save()
        {
            _repository.Save();
        }
    }
}
