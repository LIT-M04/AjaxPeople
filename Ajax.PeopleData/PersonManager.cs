using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajax.PeopleData
{
    public class PersonManager
    {
        private string _connectionString;

        public PersonManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddPerson(Person p)
        {
            InitiateDBAction(cmd =>
            {
                cmd.CommandText = "INSERT INTO People VALUES (@FirstName, @LastName, @Age); SELECT @@identity";
                cmd.Parameters.AddWithValue("@Firstname", p.FirstName);
                cmd.Parameters.AddWithValue("@LastName", p.LastName);
                cmd.Parameters.AddWithValue("@Age", p.Age);
                p.Id = (int)(decimal)cmd.ExecuteScalar();
            });
        }

        public void UpdatePerson(Person p)
        {
            InitiateDBAction(cmd =>
            {
                cmd.CommandText = "UPDATE People SET FirstName = @FirstName, LastName = @LastName, Age = @Age " +
                "WHERE Id = @Id;";
                cmd.Parameters.AddWithValue("@Id", p.Id);
                cmd.Parameters.AddWithValue("@Firstname", p.FirstName);
                cmd.Parameters.AddWithValue("@LastName", p.LastName);
                cmd.Parameters.AddWithValue("@Age", p.Age);
                cmd.ExecuteNonQuery();
            });
        }

        public Person GetPerson(int id)
        {
            Person person = null;
            InitiateDBAction(cmd =>
            {
                cmd.CommandText = "SELECT * FROM People WHERE Id = @id;";
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                reader.Read();
                person = new Person();
                person.Id = id;
                person.FirstName = (string)reader["FirstName"];
                person.LastName = (string)reader["LastName"];
                person.Age = (int)reader["Age"];
            });

            return person;
        }

        public IEnumerable<Person> GetPeople()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM People";
                    var people = new List<Person>();
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        people.Add(new Person
                        {
                            Id = (int)reader["Id"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            Age = (int)reader["Age"]
                        });
                    }
                    return people;
                }
            }
        }

        public void DeletePerson(int id)
        {
            InitiateDBAction(cmd =>
            {
                cmd.CommandText = "DELETE FROM People WHERE Id=@Id";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            });

        }

        private void InitiateDBAction(Action<SqlCommand> action)
        {
            using (var connection = new SqlConnection(_connectionString))//how come no curlys?
            using (var cmd = connection.CreateCommand())
            {
                connection.Open();
                action(cmd);
            }
        }
    }
}
