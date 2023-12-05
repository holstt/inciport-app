import { FC, useContext, useState } from 'react';
import { GoogleMapComponent } from '../compositecomponents/GoogleMapComponent';
import StatusTeamDescriptionCategoryTimestampContactInformationPicturesComponent from '../compositecomponents/StatusTeamDescriptionCategoryTimestampContactInformationPicturesComponent';
import AddressButtonsComponent from '../compositecomponents/AddressButtonsComponent';
import authService from '../../services/authService';
import { useHistory } from 'react-router-dom';
import { APIClient } from '../../services/ApiClient';
import { Context } from '../../Context';
import Address from '../../models/Address';
import '../../css/EditPageStyles.css';

interface Props {
	handleUnauthorized: () => null;
}

export const EditPage: FC<Props> = (props) => {
	const [isAuthorized] = useState(() => {
		return authService.getCurrentUser() != null ? true : false;
	});

	const history = useHistory();
	const { incidentReport, updateCurrentIncidentReport, isIncidentReportUpdated, updateIsIncidentReportUpdated } =
		useContext(Context);

	const onSave = async () => {
		if (window.confirm(`Are you sure you want to save the following changes?`)) {
			try {
				const response = await APIClient.updateIncidentReport(incidentReport);
				updateCurrentIncidentReport(response.data);
				updateIsIncidentReportUpdated(false);
				return history.goBack();
			} catch (error: any) {
				const errorMessage = 'Incident reports (PUT) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		}
	};

	const onCancel = () => {
		if (isIncidentReportUpdated) {
			if (window.confirm(`Are you sure you want to cancel and delete the changes that you've made?`)) {
				return history.goBack();
			}
		} else {
			return history.goBack();
		}
	};

	const onDelete = async () => {
		const tempAddress = incidentReport.location.address;
		const address = new Address(tempAddress.street, tempAddress.city, tempAddress.zipCode, tempAddress.country, tempAddress.municipality);
		const confirmationMessage =
			'Are you sure you want to delete the following incident?\n' +
			`ID: ${incidentReport.id}\n` +
			`Address: ${address.toString()}\n` +
			`Description: ${incidentReport.description}`;
		if (window.confirm(confirmationMessage)) {
			try {
				await APIClient.deleteIncidentReport(incidentReport.id);
				return history.goBack();
			} catch (error: any) {
				const errorMessage = 'Incident reports (DELETE) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		}
	};

	return (
		<div>
			{isAuthorized ? (
				<div className='EditContainer'>
					<AddressButtonsComponent
						onSave={onSave}
						onCancel={onCancel}
						onDelete={onDelete}
						disabledCondition={!isIncidentReportUpdated}
					/>
					<div className='StatusTeamDescriptionCategoryTimestampContactInformationPicturesMapContainer'>
						<StatusTeamDescriptionCategoryTimestampContactInformationPicturesComponent />
						<GoogleMapComponent />
					</div>
				</div>
			) : (
				props.handleUnauthorized()
			)}
		</div>
	);
};

export default EditPage;
