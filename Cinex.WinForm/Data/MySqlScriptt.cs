using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace Cinex.WinForm.Data
{
    public class MySqlScriptt
    {
        private readonly string _connectionText;

        public MySqlScriptt(string connectionText)
        {
            _connectionText = connectionText;
        }

        public async Task ExecuteAsync(string commandText)
        {
            using (var conn = new MySqlConnection(_connectionText))
            {
                if (conn.State != ConnectionState.Open) await conn.OpenAsync();

                var script = new MySqlScript(connection: conn, query: commandText);

                await script.ExecuteAsync();
            }
        }
    }
}
