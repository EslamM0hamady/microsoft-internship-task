using ContactManagementSystem.Domain.Entities;

namespace ContactManagementSystem.Domain.Interfaces
{
    public interface IContactRepository
    {
        void Add(Contact contact);
        bool Update(Guid id, string newName, string newPhone, string newEmail);
        bool Delete(Guid id);
        Contact? GetById(Guid id);
        IEnumerable<Contact> GetAll();
        IEnumerable<Contact> Find(Func<Contact, bool> predicate);
        void Save();
        void Load();
    }
}
