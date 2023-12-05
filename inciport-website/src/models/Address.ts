export default class Address {
	street: string; // With house number
	city: string;
	zipCode: string;
	country: string;
	municipality: string;

	constructor(street: string, city: string, zipCode: string, country: string, municipality: string) {
		this.street = street;
		this.city = city;
		this.zipCode = zipCode;
		this.country = country;
		this.municipality = municipality;
	}

	public toString = (): string => {
		return [this.street, this.zipCode, this.city, this.country].filter(x => x).join(', ');
	};
}
