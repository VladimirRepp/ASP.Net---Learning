using Microsoft.EntityFrameworkCore;
using TestDb.DatabaseLogic.Contexts;
using TestDb.DatabaseLogic.Models;

namespace TestDb.DatabaseLogic.Controllers
{
    public class DbUserController
    {
        private static DbUser AUTHORIZED_USER;

        private readonly DbAppContext _dbContext;

        public DbUser AuthorizedUser => AUTHORIZED_USER;

        public DbUserController(DbAppContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region === Logic ===

        public bool IsAuthorized(DbUser user)
        {
            try
            {
                _dbContext.Database.OpenConnection();

                var found = _dbContext.Users.FirstOrDefault(
                    x => 
                    x.Login == user.Login && x.Password == user.Password
                    );

                if (found == null) 
                    return false;

                AUTHORIZED_USER = found;
            }
            finally
            {
                _dbContext.Database.CloseConnection();
            }

            return true;
        }
        #endregion

        #region === CRUD === 

        public void Add(DbUser user) 
        {
            try
            {
                _dbContext.Database.OpenConnection();

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }
            finally
            {
                _dbContext.Database.CloseConnection();
            }
        }

        public void Update(DbUser user)
        {
            try
            {
                _dbContext.Database.OpenConnection();

                DbUser? found = _dbContext.Users.FirstOrDefault(u => u.Id == user.Id);

                if (found == null)
                    throw new Exception($"DbUserController.Update: User not found with ID: {user.Id}");

                found.Login = user.Login;
                found.Password = user.Password;

                _dbContext.SaveChanges();
            }
            finally
            {
                _dbContext.Database.CloseConnection();
            }
        }

        public void DeleteById(int id)
        {
            try
            {
                _dbContext.Database.OpenConnection();

                var found = _dbContext.Users.FirstOrDefault(u => u.Id == id);

                if (found == null)
                    throw new Exception($"DbUserController.DeleteById: User not found with ID: {id}");

                _dbContext.Users.Remove(found);
                _dbContext.SaveChanges();
            }
            finally
            {
                _dbContext.Database.CloseConnection();
            }
        }
        #endregion

        #region === Testing ===

        public void Test_Add()
        {
            DbUser user = new();
            DbUser user1 = new();

            user.Login = "log1";
            user.Password = "1111";

            user1.Login = "log2";
            user1.Password = "2222";

            Add(user);
            Add(user1);

            Console.WriteLine("Add completed!");
        }

        public void Test_Update()
        {
            Update(new DbUser() { Id = 18, Login = "new_log", Password = "new_pass" });

            Console.WriteLine("Update completed!");
        }

        public void Test_Delete()
        {
            DeleteById(18);

            Console.WriteLine("Delete completed!");
        }
        #endregion

        #region === Views ===

        public void ConsoleView()
        {
            foreach (var user in _dbContext.Users)
            {
                Console.WriteLine($"{user.Id} - {user.Login} {user.Password}");
            }
        }
        #endregion
    }
}
