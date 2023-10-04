namespace usersProjekt
{
    public class Dtos
    {
        public record UserDto(Guid Id, string Name, int Age, string Time);
        public record CreateUserDto(string Name, int Age);
        public record UpdateUserDto(string Name, int Age);
    }
}
