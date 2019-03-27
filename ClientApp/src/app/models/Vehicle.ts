export interface KeyValuePair {
  id: number;
  name: string;
}

export interface Contact {
  contactName: string;
  contactPhone: string;
  contactEmail: string;
}

export interface Vehicle {
  id: number;
  model: KeyValuePair;
  make: KeyValuePair;
  contact: Contact;
  isRegistered: boolean;
  features: KeyValuePair[];
  lastUpdate: string;
}

export interface SaveVehicle {
  id: number;
  modelId: number;
  makeId: number;
  contact: Contact;
  isRegistered: boolean;
  features: number[];
}
