namespace Telegram.Bot.Library.GLO.Employees
{
    public class Employee
    {
        public Employee(int id, string name, string timestamp)
        {
            Id = id;
            Name = name;
            Timestamp = timestamp;
        }

        public int Id { get; }

        public string Name { get; }

        public string Timestamp { get; private set; }

        public void UpdateTimestamp(string timestamp)
        {
            this.Timestamp = timestamp;
        }
    }
}