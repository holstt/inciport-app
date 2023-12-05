import Address from './Address';

export default class Location {
	longitude: number;
	latitude: number;
	address: Address;

	constructor(longitude: number, latitude: number, address: Address) {
		this.longitude = longitude;
		this.latitude = latitude;
		this.address = address;
	}
}
