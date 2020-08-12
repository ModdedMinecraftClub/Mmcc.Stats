namespace Mmcc.Stats.Core.Data.Models
{
    public class Server
    {
        public int ServerId { get; set; }
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
        public string ServerName { get; set; }
        public bool Enabled { get; set; }
    }
}
