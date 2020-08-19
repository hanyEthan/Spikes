namespace domain.DTOs
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; } = new Address();
    }
}
