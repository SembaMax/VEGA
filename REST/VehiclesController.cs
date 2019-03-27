using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VEFA.Core.Models;
using VEFA.REST.Resources;
using VEFA.Persistance;
using VEFA.Core;

namespace VEFA.REST
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IVehicleRepository vehicleRepositiory;
        private readonly IUnityOfWork unitOfWork;

        public VehiclesController(IMapper mapper, IUnityOfWork unitOfWork, IVehicleRepository vehicleRepositiory)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.vehicleRepositiory = vehicleRepositiory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody]SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = await vehicleRepositiory.GetModel(vehicleResource.ModelId);
            if (model == null)
            {
                ModelState.AddModelError("ModelId", "The 'modelId' is missing");
                return BadRequest(ModelState);
            }

            try
            {
                var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
                vehicle.LastUpdateTime = System.DateTime.Now;
                vehicleRepositiory.AddVehicle(vehicle);
                await unitOfWork.Complete();

                vehicle = await vehicleRepositiory.GetVehicle(vehicle.Id, includeRelated: true);

                var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVehicle([FromBody]SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var vehicleFromDB = await vehicleRepositiory.GetVehicle(vehicleResource.Id, includeRelated: true);
                if (vehicleFromDB == null)
                    return NotFound();
                mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicleFromDB);
                vehicleFromDB.LastUpdateTime = System.DateTime.Now;
                await unitOfWork.Complete();
                vehicleFromDB = await vehicleRepositiory.GetVehicle(vehicleFromDB.Id, includeRelated: true);
                var result = mapper.Map<Vehicle, VehicleResource>(vehicleFromDB);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }

        [HttpDelete("{vehicleId}")]
        public async Task<IActionResult> DeleteVehicle(int vehicleId)
        {
            try
            {
                var vehicle = await vehicleRepositiory.GetVehicle(vehicleId, includeRelated: false);
                if (vehicle == null)
                    return NotFound();
                vehicleRepositiory.DeleteVehicle(vehicle);
                await unitOfWork.Complete();
                return Ok(vehicleId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }

        [HttpGet("{vehicleId}")]
        public async Task<IActionResult> GetVehicle(int vehicleId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var vehicle = await vehicleRepositiory.GetVehicle(vehicleId, includeRelated: true);
                if (vehicle == null)
                    return NotFound();
                var vehicleResource = mapper.Map<Vehicle, VehicleResource>(vehicle);
                return Ok(vehicleResource);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }
    }
}