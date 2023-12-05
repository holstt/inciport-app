import axios from 'axios';
import AuthorizedUser from '../models/AuthorizedUser';
import configData from '../Config.json';

class AuthService {
	async login(email: string, password: string): Promise<AuthorizedUser> {
		const result = await axios.post(
			configData.API_URL + 'auth/login',
			{
				email,
				password,
			}
		);

		if (result.status === 200) {
			sessionStorage.setItem('AuthorizedUser', JSON.stringify(result.data));
		}
		return result.data;
	}

	logout() {
		sessionStorage.removeItem('AuthorizedUser');
	}
	
	getCurrentUser(): AuthorizedUser | null {
		const user: string | null = sessionStorage.getItem('AuthorizedUser');
		if (user) {
			const authorizedUser: AuthorizedUser = JSON.parse(user);
			return authorizedUser;
		}
		return null;
	}

	setMunicipalityOnUser(rowId: number | null, rowName: string) {
		let user: AuthorizedUser | null = this.getCurrentUser();
		if(user) {
			if (!rowId) {
				rowId = user.municipalityId;
			}
			user.municipalityId = rowId;
            user.municipalityName = rowName;
            sessionStorage.setItem('AuthorizedUser', JSON.stringify(user));
			return true;
		} else {
			return false;
		}
	}
}

export default new AuthService();
