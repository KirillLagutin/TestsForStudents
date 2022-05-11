using MySql.Data.MySqlClient;


namespace DB;

public class ConnectionDb
{
    private MySqlConnection _db;
    private MySqlCommand _command;

    public ConnectionDb()
    {
        _db = new MySqlConnection(GetConnectionString());
        _command = new MySqlCommand {Connection = _db};
    }

    private static string GetConnectionString()
    {
        using var file = new StreamReader("connection_to_db.ini");
        return file.ReadToEnd();
    }
    
    public MySqlCommand GetCmd() => _command;
    
    public async Task ConnectionOpenAsynk() => await _db.OpenAsync();
    public async Task ConnectionCloseAsync() => await _db.CloseAsync();
}