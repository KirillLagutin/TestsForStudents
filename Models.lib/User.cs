namespace Models.lib;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public int GroupId { get; set; }
    public int AuthorizationId { get; set; }
    public int RoleId { get; set; }
}