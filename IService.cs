using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umel.DTO;

namespace Umel.Services
{
    public interface IService<T, Y> : IBaseService<T>
     where T : IEntity
     where Y : class
    {
        int Create(T obj, int? userID);
        int Edit(T obj, int? userID);
        T Detail(int id);
        T MapSingle(Y source);
        List<T> MapList(List<Y> source);
    }
}
