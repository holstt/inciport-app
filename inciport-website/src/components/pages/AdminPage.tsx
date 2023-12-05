import { FC, useContext, useState } from 'react';
import { Context } from '../../Context';
import { APIClient } from '../../services/ApiClient';
import TabsDashboardComponent from '../compositecomponents/dashboardcomponents/TabsDashboardComponent';
import NameButtonsComponent from '../compositecomponents/NameButtonsComponent';
import { useHistory } from 'react-router-dom';
import authService from '../../services/authService';
import AuthorizedUser from '../../models/AuthorizedUser';
import '../../css/AdminPageStyles.css';

interface Props {
	handleUnauthorized: () => null;
}

export const AdminPage: FC<Props> = (props) => {
	const { isMunicipalityUpdated, updateIsMunicipalityUpdated } = useContext(Context);
	const [currentUser, setCurrentUser] = useState<AuthorizedUser | null>(authService.getCurrentUser());
	const [isAuthorized] = useState(currentUser != null ? true : false);
	const history = useHistory();

	const handleNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		if (event.target.value.length <= 256) {
			event.target.style.border = 'solid black 2px';
			updateIsMunicipalityUpdated(true);
			console.log(event.target.value);
			setCurrentUser({
				...currentUser!,
				municipalityName: event.target.value,
			});
		} else {
			event.target.style.border = 'solid red 2px';
		}
	};

	const onSave = async () => {
		if (window.confirm('Are you sure you want to save the following changes?')) {
			try {
				const response = await APIClient.updateMunicipality(currentUser!.municipalityId, currentUser!.municipalityName);
				authService.setMunicipalityOnUser(response.data.id, response.data.name);
				updateIsMunicipalityUpdated(false);
			} catch (error: any) {
				const errorMessage = 'Municipalities (PUT) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		}
	};

	const onCancel = () => {
		if (isMunicipalityUpdated) {
			if (window.confirm("Are you sure you want to cancel and delete the changes that you've made?")) {
				authService.setMunicipalityOnUser(0, '');
				return history.goBack();
			}
		} else {
			authService.setMunicipalityOnUser(0, '');
			return history.goBack();
		}
	};

	const onDelete = async () => {
		const confirmationMessage =
			'Are you sure you want to delete the following municipality?\n' +
			`ID: ${currentUser!.municipalityId}\n` +
			`Name: ${currentUser!.municipalityName}\n`;
		if (window.confirm(confirmationMessage)) {
			try {
				await APIClient.deleteMunicipality(currentUser!.municipalityId);
				authService.setMunicipalityOnUser(0, '');
				return history.goBack();
			} catch (error: any) {
				const errorMessage = 'Municipalities (DELETE) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		}
	};

	return (
		<div>
			{isAuthorized ? (
				<div className='AdminContainer'>
					{currentUser!.role.toLowerCase().includes('maintainer') ? (
						<NameButtonsComponent
							name={currentUser!.municipalityName}
							nameOfWhat='Municipality'
							handleOnChange={handleNameChange}
							onSave={onSave}
							onCancel={onCancel}
							onDelete={onDelete}
							disabledCondition={!isMunicipalityUpdated}
						/>
					) : null}
					<TabsDashboardComponent />
				</div>
			) : (
				props.handleUnauthorized()
			)}
		</div>
	);
};

export default AdminPage;
