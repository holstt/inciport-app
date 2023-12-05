import { Formik, Form, Field, ErrorMessage } from 'formik';
import { FC, useContext } from 'react';
import { Context } from '../../../Context';
import { APIClient } from '../../../services/ApiClient';
import PopupComponent from '../../basecomponents/PopupComponent';
import * as Yup from 'yup';
import { useHistory } from 'react-router-dom';
import PopupHeaderComponent from '../../basecomponents/PopupHeaderComponent';
import PopupEditSubmitComponent from '../../basecomponents/PopupEditSubmitComponent';

interface Props {
	show: boolean;
	handleHasChanged: () => void;
	handleToggle: () => void;
}

export const EditWorkerTeamPopupComponent: FC<Props> = (props) => {
	const history = useHistory();
	const { team, updateSelectedWorkerTeam } = useContext(Context);

	const handleSave = async () => {
		await updateWorkerTeam();
		props.handleToggle();
		props.handleHasChanged();
		return false;
	};

	const updateWorkerTeam = async () => {
		try {
			await APIClient.updateTeam(team);
		} catch (error: any) {
			const errorMessage = 'Worker teams (PUT) - ' + error.toString();
			console.log(errorMessage);
			history.push('/error', errorMessage);
		}
	};

	const handleDelete = async () => {
		await deleteTeam();
		props.handleToggle();
		props.handleHasChanged();
		return false;
	};

	const deleteTeam = async () => {
		const confirmationMessage = `Are you sure you want to delete the following team?\nID: ${team.id}\nName: ${team.name}`;
		if (window.confirm(confirmationMessage)) {
			try {
				await APIClient.deleteWorkerTeam(team.id);
			} catch (error: any) {
				const errorMessage = 'Worker teams (DELETE) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		}
	};

	const handleClose = async () => {
		props.handleToggle();
	};

	const handleNameChange = (value: string) => {
		updateSelectedWorkerTeam({
			...team,
			name: value,
		});
	};

	const validationSchema = () => {
		return Yup.object().shape({
			name: Yup.string().max(256, 'Name must be less than 256 characters').required('Name is required'),
		});
	};

	return (
		<PopupComponent show={props.show}>
			<PopupHeaderComponent headerMessage='Edit worker team:' handleClose={handleClose} />
			<Formik enableReinitialize={true} initialValues={team} validationSchema={validationSchema} onSubmit={handleSave}>
				<Form>
					{/* Full name */}
					<div className='form-group'>
						<label htmlFor='name'>Name</label>
						<Field
							name='name'
							type='text'
							onChange={(event) => {
								handleNameChange(event.currentTarget.value);
							}}
							className='form-control'
						/>
						<ErrorMessage name='name' component='div' className='alert alert-danger ErrorMessage' />
					</div>

					{/* Submit */}
					<PopupEditSubmitComponent handleDelete={handleDelete} />
				</Form>
			</Formik>
		</PopupComponent>
	);
};

export default EditWorkerTeamPopupComponent;
