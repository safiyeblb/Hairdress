using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umel.Repository.UnitOfWork;

namespace Umel.Services
{
    public class OrderService
    {
        private UnitOfWork uow = new UnitOfWork();

        //public List<UserDTO> GetUsers()
        //{
        //    //Repository<User> Users = new Repository<User>(new IvhoManagementEntities());
        //    //Users.GetAll();

        //    var result = uow.Users.GetAll();
        //    List<UserDTO> users = new List<UserDTO>();

        //    if (result.Count > 0)
        //    {
        //        foreach (var item in result)
        //        {
        //            UserDTO User = new UserDTO
        //            {
        //                UserName = item.UserName,
        //                UserId = item.UserId,
        //                Password = item.Password
        //            };

        //            users.Add(User);
        //        }
        //        return users;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}


    }
}
