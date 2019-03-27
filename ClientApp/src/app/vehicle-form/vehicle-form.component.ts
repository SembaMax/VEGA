import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs/observable/forkJoin'
import { SaveVehicle, Vehicle } from '../models/Vehicle';
import { ToastyService } from 'ng2-toasty';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  makes: any[];
  models: any[]
  vehicle: SaveVehicle = {
    id: 0,
    isRegistered: false,
    makeId: 0,
    modelId: 0,
    features: [],
    contact: { contactName: "", contactPhone: "", contactEmail: "" }
  };
  features: any[]

  constructor(
    private route: ActivatedRoute, // use this to read route parameters
    private router: Router, // use this to navigate user to different page
    private vehicleService: VehicleService,
    private toastyService: ToastyService
  ) {

    route.params.subscribe(p => {
      this.vehicle.id = +p['id'];
    });
  }

  ngOnInit() {
    let sources = [this.vehicleService.getMakes(), this.vehicleService.getFeatures()]
    if (this.vehicle.id)
      sources.push(this.vehicleService.getVehicle(this.vehicle.id));

    forkJoin(sources).subscribe(
      data => {
      this.makes = data[0];
      this.features = data[1];
        if (this.vehicle.id) {
          this.setVehicle(data[2]);
          this.populateModels();
        }
    },
      error => {
        if (error.status == 404)
          this.router.navigate(['/home']);
      }
    );
  }

  private setVehicle(v: Vehicle) {
    this.vehicle.id = v.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.contact = v.contact;
    this.vehicle.features = v.features.map(f => f.id);
  }

  private populateModels() {
    var selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
    this.models = selectedMake ? selectedMake.models : [];
  }

  onMakeChange() {
    console.log("Changes : ", this.vehicle)
    this.populateModels();
    delete this.vehicle.modelId;
  }

  onFeatureToggle(featureId, $event) {
    if ($event.target.checked)
      this.vehicle.features.push(featureId);
    else {
      var index = this.vehicle.features.indexOf(featureId);
      this.vehicle.features.splice(index,1);
    }
  }

  submit() {
    if (this.vehicle.id) {
      this.vehicleService.updateVehicle(this.vehicle).subscribe(res => {
        this.toastyService.success({
          title: 'Success',
          msg: 'The vehicle is updated successfully',
          theme: 'bootstrap',
          showClose: true,
          timeout: 5000
        });
      });
    }
    else {
      this.vehicleService.createVehicle(this.vehicle).subscribe(vehicleRes => {
        this.toastyService.success({
          title: 'Success',
          msg: 'The vehicle is created successfully',
          theme: 'bootstrap',
          showClose: true,
          timeout: 5000
        });
      });
    }
  }

  delete() {
    if (confirm("Are You Sure ?")) {
      this.vehicleService.deleteVehicle(this.vehicle.id).subscribe(res => {
        this.router.navigate(['/home']);
      });
    }
  }
}
