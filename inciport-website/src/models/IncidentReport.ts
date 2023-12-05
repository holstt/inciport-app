import ContactInformation from './ContactInformation';
import Location from './Location';
import ChosenMainCategory from './ChosenMainCategory';
import WorkerTeam from './WorkerTeam';

export default class IncidentReport {
	id: number;
	status: string;
	chosenMainCategory: ChosenMainCategory;
	location: Location;
	description: string;
	contactInformation: ContactInformation;
	imageUrls: string[];
	timestampCreatedUtc: Date;
	timestampModifiedUtc: Date;
	assignedTeam: WorkerTeam;

	constructor(
		id: number,
		status: string,
		chosenMainCategory: ChosenMainCategory,
		location: Location,
		description: string,
		contactInformation: ContactInformation,
		imageUrls: string[],
		timestampCreatedUtc: Date,
		timestampModifiedUtc: Date,
		assignedTeam: WorkerTeam
	) {
		this.id = id;
		this.status = status;
		this.chosenMainCategory = chosenMainCategory;
		this.location = location;
		this.description = description;
		this.contactInformation = contactInformation;
		this.imageUrls = imageUrls;
		this.timestampCreatedUtc = timestampCreatedUtc;
		this.timestampModifiedUtc = timestampModifiedUtc;
		this.assignedTeam = assignedTeam;
	}
}
