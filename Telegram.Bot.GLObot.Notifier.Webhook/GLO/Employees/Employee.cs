namespace Telegram.Bot.GLObot.Notifier.Webhook.GLO.Employees
{
    class Employee
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