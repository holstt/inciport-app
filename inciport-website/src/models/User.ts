export default class User {
	id: number;
	fullName: string;
	email: string;
	municipalityId: number;
	municipalityName: string;
	role: string;

	constructor(id: number, fullName: string, email: string, municipalityId: number, municipalityName: string, role: string) {
		this.id = id;
		this.fullName = fullName;
		this.email = email;
		this.municipalityId = municipalityId;
		this.municipalityName = municipalityName;
		this.role = role;
	}
}