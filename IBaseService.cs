using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umel.DTO;

namespace Umel.Services
{
    public interface IBaseService<T> where T : IEntity
    {
        List<T> List();
    }
}
