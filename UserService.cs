using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Umel.Data;
using Umel.DTO;
using Umel.Repository.UnitOfWork;

namespace Umel.Services
{
    public class UserService
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

        public int InsertUser(UserDTO user)
        {
            try
            {
                //hash
                string SaltDegeri = Crypto.GenerateSalt();
                /*
                 GenerateSalt metodunu kullanarak rastgele byte değerlerinden oluşan
                 SaltDegeri isminde bir değişken tanımladık.
                 */
                string HashDegeri = Crypto.HashPassword(user.Password);

                Kullanici newUser = new Kullanici
                {
                    Email = user.Email,
                    SecurityStamp = SaltDegeri,
                    PasswordHash=HashDegeri,
                    AdSoyad = user.NameSurname,
                    UyelikTipi = 1 //normal kullanıcı
                };

                uow.Users.Insert(newUser);
                return uow.Commit();
            }
            catch (DbEntityValidationException ex)
            {
                uow.RollBack();
                return 0;
            }
            catch (Exception ex)
            {
                uow.RollBack();
                return 0;
            }
        }

        public int UpdateUser(string email,string sifre)
        {
            try
            {
                //hash
                string SaltDegeri = Crypto.GenerateSalt();
                /*
                 GenerateSalt metodunu kullanarak rastgele byte değerlerinden oluşan
                 SaltDegeri isminde bir değişken tanımladık.
                 */
                string HashDegeri = Crypto.HashPassword(sifre);

                Kullanici updatingUser = GetUser(email);
                updatingUser.PasswordHash = HashDegeri;
                updatingUser.SecurityStamp = SaltDegeri;

                uow.Users.Update(updatingUser);
                return uow.Commit();
            }
            catch (DbEntityValidationException ex)
            {
                uow.RollBack();
                return 0;
            }
            catch (Exception ex)
            {
                uow.RollBack();
                return 0;
            }
        }
        //public int UpdateUser(UserDTO user)
        //{
        //    try
        //    {
        //        User updatingUser = new User
        //        {
        //            UserId = user.UserId,
        //            UserName = user.UserName,
        //            Password = user.Password
        //        };

        //        uow.Users.Update(updatingUser);
        //        return uow.Commit();
        //    }
        //    catch (DbEntityValidationException ex)
        //    {

        //        uow.RollBack();
        //        return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        uow.RollBack();
        //        return 0;
        //    }
        //}

        //public UserDTO GetUser(int id)
        //{
        //    var result = uow.Users.Single(id);
        //    UserDTO user = new UserDTO
        //    {
        //        UserName = result.UserName,
        //        UserId = result.UserId,
        //        Password = result.Password
        //    };
        //    return user;
        //}

        public UserDTO Login(string email, string password)
        {
            //string SaltDegeri = Crypto.GenerateSalt();
            ///*
            // GenerateSalt metodunu kullanarak rastgele byte değerlerinden oluşan
            // SaltDegeri isminde bir değişken tanımladık.
            // */
            //string HashDegeri = Crypto.HashPassword(password);
            /*
             Girilecek olan şifre değerini Hash işlemine tabii tutarak HashDegeri
             isminde bir RFC 2898 Hash değeri elde etmiş olduk.
             */
            //bool EsitMi = Crypto.VerifyHashedPassword(HashDegeri, password);
            /*
             Burada ise, girilen şifre ile HashDegeri isimli değişkenimiz içindeki
             değeri karşılaştırarak aynı mı değil mi kontrol ettik.Eğer aynı ise
             true, değil ise false değerini dönecektir.
             */


            //hash mantığını burda çalıştır
            Kullanici user = uow.Users.Single(x => x.Email==email);
            if (user != null)
            {
                //şifre uyuyor mu ?
                bool EsitMi = Crypto.VerifyHashedPassword(user.PasswordHash, password);
                if (EsitMi)
                {

                    UserDTO loginUser = new UserDTO();
                    loginUser.UserId = user.KullaniciID;
                    loginUser.NameSurname = user.AdSoyad;
                    loginUser.RecordStatusID = user.UyelikTipi;
                    return loginUser;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

        private Kullanici GetUser(string email)
        {
            Kullanici user = uow.Users.Single(x => x.Email == email);
            return user;
        }
        public bool ControlEmail(string email)
        {
            Kullanici user = uow.Users.Single(x => x.Email == email);
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int DeleteUser(int id)
        {
            var obj = uow.Users.Single(id);
            var result = uow.Users.Delete(obj);
            return uow.Commit();
        }

    }
}
