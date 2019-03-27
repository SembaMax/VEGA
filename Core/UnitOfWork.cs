using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEFA.Persistance;

namespace VEFA.Core
{
    public class UnitOfWork : IUnityOfWork
    {
        private readonly VegaDbContext context;

        public UnitOfWork(VegaDbContext context)
        {
            this.context = context;
        }

        public async Task Complete()
        {
            await context.SaveChangesAsync();
        }
    }
}
