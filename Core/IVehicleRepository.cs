using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEFA.Core.Models;

namespace VEFA.Core
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetVehicle(int id, bool includeRelated);

        Task<Model> GetModel(int modelId);

        void AddVehicle(Vehicle vehicle);

        void DeleteVehicle(Vehicle vehicle);
    }
}
