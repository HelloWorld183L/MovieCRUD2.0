using MovieCRUD.SharedKernel;

namespace MovieCRUD.Infrastructure.Persistence
{
    public class UserEntity : IEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}