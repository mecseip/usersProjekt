using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static usersProjekt.Dtos;

namespace usersProjekt.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private List<UserDto> users = new();
        Connection conn = new Connection();

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            try
            {
                conn.connection.Open();
                string sql = "SELECT * FROM users";
                MySqlCommand cmd = new MySqlCommand(sql, conn.connection);

                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                do
                {
                    var result = new UserDto(
                        reader.GetGuid(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetString(3)
                    );

                    users.Add(result);
                } while (reader.Read());

                conn.connection.Close();

                return Ok(users);

            }
            catch (Exception e)
            {

                return BadRequest(e);
            }

        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetById(Guid id)
        {
            conn.connection.Open();
            string sql = $"SELECT * FROM users WHERE Id='{id}'";

            MySqlCommand cmd = new MySqlCommand(sql, conn.connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                var result = new UserDto(
                       reader.GetGuid(0),
                       reader.GetString(1),
                       reader.GetInt32(2),
                       reader.GetString(3)
                   );

                conn.connection.Close();
                return Ok(result);

            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        public ActionResult<UserDto> PostProduct(CreateUserDto createUser)
        {
            DateTime localTime = DateTime.Now;
            string sqlTime = localTime.ToString("yyyy-MM-dd HH:mm:ss");

            var user = new UserDto(
                Guid.NewGuid(),
                createUser.Name,
                createUser.Age,
                sqlTime
                );

            conn.connection.Open();

            string sql = $"INSERT INTO `users`(`Id`, `Name`, `Age`, `CreatedTime`) VALUES ('{user.Id}','{user.Name}',{user.Age},'{user.Time}')";

            MySqlCommand cmd = new MySqlCommand(sql, conn.connection);
            cmd.ExecuteNonQuery();

            conn.connection.Close();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public ActionResult<UserDto> PutProduct(Guid id, UpdateUserDto updateUser)
        {
            try
            {
                conn.connection.Open();
                string sql = $"UPDATE `users` SET `Name`='{updateUser.Name}',`Age`={updateUser.Age} WHERE Id = '{id}'";

                MySqlCommand cmd = new MySqlCommand(sql, conn.connection);

                cmd.ExecuteNonQuery();

                conn.connection.Close();

                return StatusCode(200, "A felhasználó módosítva");
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }

        }

        [HttpDelete("{id}")]
        public ActionResult<object> Delete(Guid id)
        {
            try
            {
                conn.connection.Open();

                string sql = $"DELETE FROM users WHERE Id='{id}'";

                MySqlCommand cmd = new MySqlCommand(sql, conn.connection);

                if (cmd.ExecuteNonQuery() == 0)
                {
                    return StatusCode(404, "A felhasználó nem létezik.");
                }
                else
                {
                    return StatusCode(200, "A felhasználó törölve.");
                }

            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
