using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEFA.Core.Models;
using VEFA.Persistance;

namespace VEFA.Core
{
    public class VehicleRepository: IVehicleRepository
    {
        private readonly VegaDbContext context;

        public VehicleRepository(VegaDbContext context)
        {
            this.context = context;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            
        }

        public Task<Model> GetModel(int modelId)
        {
            return context.Models.FindAsync(modelId);
        }

        public async Task<Vehicle> GetVehicle(int id, bool includeRelated)
        {
            if (!includeRelated)
                return await context.Vehicles.FindAsync(id);

            var vehicle = await context.Vehicles
                    .Include(v => v.Features).ThenInclude(vf => vf.Feature)
                    .Include(v => v.Model).ThenInclude(m => m.Make)
                    .SingleOrDefaultAsync(v => v.Id == id);

            return vehicle;
        }
    }
}
