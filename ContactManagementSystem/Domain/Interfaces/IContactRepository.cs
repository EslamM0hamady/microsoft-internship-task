using ContactManagementSystem.Domain.Entities;

namespace ContactManagementSystem.Domain.Interfaces
{
    // Abstraction over how contacts are stored and retrieved.
    public interface IContactRepository
    {
        // Store a new contact instance.
        void Add(Contact contact);

        // Update basic fields for an existing contact; returns false when not found.
        bool Update(Guid id, string newName, string newPhone, string newEmail);

        // Remove a contact by Id; returns false when not found.
        bool Delete(Guid id);

        // Retrieve a contact by Id, or null if missing.
        Contact? GetById(Guid id);

        // Return all contacts from the backing store.
        IEnumerable<Contact> GetAll();

        // In-memory filtering based on a predicate.
        IEnumerable<Contact> Find(Func<Contact, bool> predicate);

        // Persist in-memory changes to the backing store.
        void Save();

        // Load contacts from the backing store into memory.
        void Load();
    }
}
