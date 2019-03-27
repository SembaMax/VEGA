using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VEFA.Core
{
    public interface IUnityOfWork
    {
        Task Complete();
    }
}
