import axios from 'axios';
import AuthorizedUser from '../models/AuthorizedUser';
import IncidentReport from '../models/IncidentReport';
import MainCategoryOptions from '../models/MainCategoryOptions';
import User from '../models/User';
import WorkerTeam from '../models/WorkerTeam';
import UserDTO from '../models/UserDTO';
import Category from '../models/Category';
import Municipality from '../models/Municipality';
import configData from '../Config.json';

class ApiClient {
	private timeOutTime = 10000;
	private user: string | null = sessionStorage.getItem('AuthorizedUser');
	currentUser: AuthorizedUser | null = this.user == null ? null : JSON.parse(this.user);
	private baseMunicipalityUrl: string = configData.API_URL + 'municipalities/' + this.currentUser?.municipalityId;
	private incidentUrl = this.baseMunicipalityUrl + '/inciports/';
	private userUrl = this.baseMunicipalityUrl + '/users/';
	private workerUrl = this.baseMunicipalityUrl + '/workerteams/';
	private mainCategoryUrl = this.baseMunicipalityUrl + '/categories/';
	private municipalityUrl = configData.API_URL + 'municipalities/';

	constructor() {
		axios.defaults.headers.common['Authorization'] = 'Bearer ' + this.currentUser?.token.value;
		axios.defaults.headers.post['Content-Type'] = 'application/json';
		axios.defaults.headers.put['Content-Type'] = 'application/json';
	}

	/* Incidents */
	getIncidentReports = async () => await axios.get<IncidentReport[]>(this.incidentUrl);

	createIncidentReport = async (incidentReport: IncidentReport) => await axios.post(this.incidentUrl, incidentReport);

	deleteIncidentReport = async (id: number) => await axios.delete(this.incidentUrl + id);

	updateIncidentReport = async (incidentReport: IncidentReport) =>
		await axios.put(this.incidentUrl + incidentReport.id, incidentReport);

	/* Users */
	getUsers = async () => await axios.get<User[]>(this.userUrl);

	createUser = async (user: UserDTO) => await axios.post(this.userUrl, user);

	deleteUser = async (id: number) => await axios.delete(this.userUrl + id);

	updateUser = async (user: User) => await axios.put(this.userUrl + user.id, user);

	/* Worker teams */
	getWorkerTeams = async () => await axios.get<WorkerTeam[]>(this.workerUrl);

	createWorkerTeam = async (team: WorkerTeam) => await axios.post(this.workerUrl, team);

	deleteWorkerTeam = async (id: number) => await axios.delete(this.workerUrl + id);

	updateTeam = async (team: WorkerTeam) => await axios.put(this.workerUrl + team.id, team);

	/* Categories */
	getMainCategories = async () => await axios.get<MainCategoryOptions[]>(this.mainCategoryUrl);

	createNewMainCategory = async (mainCategory: MainCategoryOptions) => await axios.post(this.mainCategoryUrl, mainCategory);

	deleteMainCategory = async (id: number) => await axios.delete(this.mainCategoryUrl + id);

	updateMainCategory = async (mainCategory: MainCategoryOptions) =>
		await axios.put(this.mainCategoryUrl + mainCategory.id, mainCategory);

	/* Sub categories */
	getSubCategories = async (id: number) => await axios.get<Category[]>(this.mainCategoryUrl + id + '/subcategories');

	createSubCategory = async (mainCategoryId: number, subCategory: Category) =>
		await axios.post(this.mainCategoryUrl + mainCategoryId + '/subcategories', subCategory);

	deleteSubCategory = async (mainCategoryId: number, subCategoryId: number) =>
		await axios.delete(this.mainCategoryUrl + mainCategoryId + '/subcategories/' + subCategoryId);

	updateSubCategory = async (mainCategoryId: number, subCategory: Category) =>
		await axios.put(this.mainCategoryUrl + mainCategoryId + '/subcategories/' + subCategory.id, subCategory);

	/* Municipalities */
	getMunicipalities = async () => await axios.get<Municipality[]>(this.municipalityUrl);

	createMunicipality = async (municipalityName: string) =>
		await axios.post(this.municipalityUrl, { name: municipalityName });

	deleteMunicipality = async (id: number) => await axios.delete(this.municipalityUrl + id);

	updateMunicipality = async (municipalityId: number, municipalityName: string) =>
		await axios.put(this.municipalityUrl + municipalityId, { id: municipalityId, name: municipalityName });

	getIncidentStatusses = async () => await axios.get<string[]>(configData.API_URL + 'statuses');

	getImage = async (url: string) => {
		const response = await axios.get(url, {
			headers: {
				'Content-Type': 'image/jpeg',
				Authorization: 'Bearer ' + this.currentUser?.token.value,
			},
			timeout: this.timeOutTime,
			responseType: 'arraybuffer',
		});
		const base64String = await Buffer.from(response.data, 'binary').toString('base64');
		return base64String;
	};
}

export const APIClient = new ApiClient();
