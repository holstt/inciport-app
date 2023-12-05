export default class Municipality {
	id: number;
	name: string;
	incidentReportCount: number;
	workerCount: number;
	categoriesCount: number;
	usersCount: number;

	constructor(
		id: number,
		name: string,
		incidentReportCount: number,
		workerCount: number,
		categoriesCount: number,
		usersCount: number
	) {
		this.id = id;
		this.name = name;
		this.incidentReportCount = incidentReportCount;
		this.workerCount = workerCount;
		this.categoriesCount = categoriesCount;
		this.usersCount = usersCount;
	}
}
