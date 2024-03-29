import { Injectable } from '@angular/core';
import { Http } from '@angular/http'
import 'rxjs/add/operator/map'
import { SaveVehicle } from '../models/Vehicle';

@Injectable()
export class VehicleService {

  constructor(private http: Http) { }

  getMakes() {
    return this.http.get('/api/makes').map(res => res.json());
  }

  getFeatures() {
    return this.http.get('/api/features').map(res => res.json())
  }

  createVehicle(vehicle) {
    return this.http.post("/api/vehicles", vehicle).map(res => res.json())
  }

  updateVehicle(vehicle: SaveVehicle) {
    return this.http.put("/api/vehicles/", vehicle).map(res => res.json())
  }

  getVehicle(id) {
    return this.http.get("/api/vehicles/" + id).map(res => res.json())
  }

  deleteVehicle(id) {
    return this.http.delete("/api/vehicles/" + id).map(res => res.json())
  }
}
