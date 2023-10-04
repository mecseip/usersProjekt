using MySql.Data.MySqlClient;

namespace usersProjekt
{
    public class Connection
    {
        public MySqlConnection connection;
        private string Host;
        private string Db;
        private string User;
        private string Password;
        private string ConnectionString;

        public Connection()
        {
            this.Host = "192.168.0.117";
            this.Db = "db";
            this.User = "root";
            this.Password = "password";

            this.ConnectionString = "SERVER=" + Host + ";DATABASE=" + Db + ";UID=" + User + ";PASSWORD=" + Password + ";SslMode=None";

            connection = new MySqlConnection(ConnectionString);
        }
    }
}
