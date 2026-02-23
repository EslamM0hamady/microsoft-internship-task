
namespace ContactManagementSystem.Domain.Entities
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }

        public Contact(string name, string phone, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Phone = phone;
            Email = email;
            CreatedAt = DateTime.Now;
        }
    }
}
