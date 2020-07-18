namespace Mmcc.Stats.Core.Models.Settings
{
    public class DatabaseSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        public override string ToString() =>
            $"Server={Server};Port={Port};Database={DatabaseName};Uid={Username};Pwd={Password};Allow User Variables=True";
    }
}