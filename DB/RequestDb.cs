using Models.lib;
using System.Data.Common;

namespace DB;

public class RequestDb
{
    public List<User> _users;
    public Authorization _authorizations;
    public List<Group> _groups;
    public List<Question> _questions;
    public List<Ansver> _ansvers;
    public List<Subject> _subjects;
    public List<Test> _tests;
    public List<Result> _result;
    public Role _roles;

    public RequestDb()
    {
        _authorizations = new Authorization();
        _groups = new List<Group>();
        _questions = new List<Question>();
        _ansvers = new List<Ansver>();
        _subjects = new List<Subject>();
        _tests = new List<Test>();
        _result = new List<Result>();
        _roles = new Role();
        _users = new List<User>();
    }

    //ДОБАВЛЕНИЕ, УДАЛЕНИЕ, ИЗМЕНЕНИЕ
    public async Task RequestExecuteNonQueryAsync(string comand)
    {
        ConnectionDb _connectDb = new ConnectionDb(comand);

        await _connectDb.ConnectionOpenAsynk();
        await _connectDb.GetCmd().ExecuteNonQueryAsync();
        await _connectDb.ConnectionCloseAsync();
    }

    //заполняем таблицу вопросов с картинкой
    /*public async Task SetQuestionAsync(string path, string name, int number)
    {

        string comand = "INSERT INTO tab_Questions VALUES (@Name, @Image, @Number)";


        ConnectionDb _connectDb = new ConnectionDb(comand);

        _connectDb.GetCmd().Parameters.Add("@Name", MySqlDbType.VarChar, 500);
        _connectDb.GetCmd().Parameters.Add("@Number", MySqlDbType.Int32);
        await _connectDb.ConnectionOpenAsynk();
        
        // массив для хранения бинарных данных файла
        byte[] image;
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            image = new byte[fs.Length];
            fs.Read(image, 0, image.Length);
            _connectDb.GetCmd().Parameters.Add("@Image", MySqlDbType.LongBlob, Convert.ToInt32(fs.Length));
        }
        
        // передаем данные в команду через параметры
        _connectDb.GetCmd().Parameters["@Name"].Value = name;
        _connectDb.GetCmd().Parameters["@Number"].Value = number;
        _connectDb.GetCmd().Parameters["@Image"].Value = image;

        await _connectDb.GetCmd().ExecuteNonQueryAsync();
        await _connectDb.ConnectionCloseAsync();
    }*/

    //ПОЛУЧЕНИЕ ДАННЫХ

    //gлучение данных пользователь
    public async Task GetUserAsync(string comand)
    {
        ConnectionDb _connectDb = new ConnectionDb(comand);

        await _connectDb.ConnectionOpenAsynk();
        using (DbDataReader reader = await _connectDb.GetCmd().ExecuteReaderAsync())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        MiddleName = reader.GetString(3),
                        GroupId = reader.GetInt32(4),
                        AuthorizationId = reader.GetInt32(5)
                    });
                }
            }
        }

        await _connectDb.ConnectionCloseAsync();
    }

    //проверка логина и пароля
    public async Task<string?> QueryAuthorizationAsync(string comand, string login, string password)
    {
        ConnectionDb _connectDb = new ConnectionDb(comand);
        var _message = "Неверный логин или пароль !!!";

        await _connectDb.ConnectionOpenAsynk();
        using (DbDataReader reader = await _connectDb.GetCmd().ExecuteReaderAsync())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _authorizations.Id = reader.GetInt32(0);
                    _authorizations.Login = reader.GetString(1);
                    _authorizations.Password = reader.GetString(2);
                    _authorizations.RoleId = reader.GetInt32(3);
                    if (Equals(login, _authorizations.Login) && Equals(password, _authorizations.Password))
                    {
                        await _connectDb.ConnectionCloseAsync();
                        return await GetRoleAsync(_authorizations.RoleId);
                    }
                }
            }
        }

        await _connectDb.ConnectionCloseAsync();
        return _message;
    }

    //Возврат роли по id
    private async Task<string?> GetRoleAsync(int id)
    {
        string _comand = $"Select name from tab_Roles Where id = {id}";
        ConnectionDb _connectDb = new ConnectionDb(_comand);

        await _connectDb.ConnectionOpenAsynk();
        using (DbDataReader reader = await _connectDb.GetCmd().ExecuteReaderAsync())
        {
            while (reader.Read())
            {
                _roles.Name = reader.GetString(reader.GetOrdinal("name"));
            }
        }

        await _connectDb.ConnectionCloseAsync();
        return _roles.Name;
    }

    //получение данных таблицы Groups
    public async Task GetGroupAsync()
    {
        var comand = "SELECT * FROM tab_Groups";
        ConnectionDb _connectDb = new ConnectionDb(comand);

        await _connectDb.ConnectionOpenAsynk();
        using (DbDataReader reader = await _connectDb.GetCmd().ExecuteReaderAsync())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _groups.Add(new Group
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }
        }

        await _connectDb.ConnectionCloseAsync();
    }

    //получение данных таблицы Question
    public async Task GetQuestionAsync()
    {
        var comand = $"SELECT * FROM tab_Questions";
        ConnectionDb _connectDb = new ConnectionDb(comand);

        await _connectDb.ConnectionOpenAsynk();
        using (DbDataReader reader = await _connectDb.GetCmd().ExecuteReaderAsync())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if ((reader.GetValue(2)).Equals(typeof(byte[])))
                    {
                        _questions.Add(new Question
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            //Picture = (byte[])reader.GetValue(2),
                            Number = reader.GetInt32(2)
                        });
                    }
                    else
                    {
                        _questions.Add(new Question
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Number = reader.GetInt32(3)
                        });
                    }
                }
            }
        }

        await _connectDb.ConnectionCloseAsync();
    }

    //получение данных таблицы Ansver
    public async Task GetAnsverAsync()
    {
        var comand = "SELECT * FROM tab_Ansvers";
        ConnectionDb _connectDb = new ConnectionDb(comand);

        await _connectDb.ConnectionOpenAsynk();
        using (DbDataReader reader = await _connectDb.GetCmd().ExecuteReaderAsync())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _ansvers.Add(new Ansver
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Scores = reader.GetInt32(2)
                    });
                }
            }
        }

        await _connectDb.ConnectionCloseAsync();
    }

    //получение данных таблицы Subject
    public async Task GetSubjectAsync()
    {
        var comand = "SELECT * FROM tab_Subjects";
        ConnectionDb _connectDb = new ConnectionDb(comand);

        await _connectDb.ConnectionOpenAsynk();
        using (DbDataReader reader = await _connectDb.GetCmd().ExecuteReaderAsync())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _subjects.Add(new Subject
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Allotted_time = reader.GetInt32(2)
                    });
                }
            }
        }

        await _connectDb.ConnectionCloseAsync();
    }

    //получение данных таблицы Test
    public async Task GetTestAsync()
    {
        var comand = "SELECT * FROM tab_Tests";
        ConnectionDb _connectDb = new ConnectionDb(comand);

        await _connectDb.ConnectionOpenAsynk();
        using (DbDataReader reader = await _connectDb.GetCmd().ExecuteReaderAsync())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _tests.Add(new Test
                    {
                        Id = reader.GetInt32(0),
                        QuestionId = reader.GetInt32(1),
                        AnsverId = reader.GetInt32(2),
                        SubjectId = reader.GetInt32(3),
                        Attempt = reader.GetInt32(4)
                    });
                }
            }
        }

        await _connectDb.ConnectionCloseAsync();
    }

    //получение данных таблицы Result
    public async Task GetResultAsync()
    {
        var comand = "SELECT * FROM tab_Results";
        ConnectionDb _connectDb = new ConnectionDb(comand);

        await _connectDb.ConnectionOpenAsynk();
        using (DbDataReader reader = await _connectDb.GetCmd().ExecuteReaderAsync())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _result.Add(new Result
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        Date = reader.GetDateTime(2),
                        Time = reader.GetDateTime(3),
                        Evalution = reader.GetInt32(4),
                        TestId = reader.GetInt32(5)
                    });
                }
            }
        }

        await _connectDb.ConnectionCloseAsync();
    }
}