import Token from './Token';
import User from './User';

export default class AuthorizedUser extends User {
	token: Token;

	constructor(
		id: number,
		fullName: string,
		email: string,
		municipalityId: number,
		municipalityName: string,
		role: string,
		token: Token
	) {
		super(id, fullName, email, municipalityId, municipalityName, role);
		this.token = token;
	}
}
