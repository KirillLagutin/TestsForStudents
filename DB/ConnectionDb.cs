using MySql.Data.MySqlClient;

namespace DB;
public class ConnectionDb
{
    private MySqlConnection _connection;
    private MySqlCommand _cmd;

    public ConnectionDb(string comand)
    {
        _connection = new MySqlConnection(GetConnectionString());
        _cmd = new MySqlCommand(comand, _connection);
    }

    private static string GetConnectionString()
    {
        using var file = new StreamReader("connection_to_db.ini");
        return file.ReadToEnd();
    }

    public MySqlCommand GetCmd() => _cmd;

    public async Task ConnectionOpenAsynk() => await _connection.OpenAsync();
    public async Task ConnectionCloseAsync() => await _connection.CloseAsync();
}