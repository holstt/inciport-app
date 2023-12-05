import { Formik, Form, Field, ErrorMessage } from 'formik';
import { FC, useState } from 'react';
import { APIClient } from '../../../services/ApiClient';
import PopupComponent from '../../basecomponents/PopupComponent';
import * as Yup from 'yup';
import WorkerTeam from '../../../models/WorkerTeam';
import { useHistory } from 'react-router-dom';
import PopupHeaderComponent from '../../basecomponents/PopupHeaderComponent';
import PopupCreateSubmitComponent from '../../basecomponents/PopupCreateSubmitComponent';

interface Props {
	show: boolean;
	handleHasChanged: () => void;
	handleToggle: () => void;
}

export const CreateWorkerTeamPopupComponent: FC<Props> = (props) => {
	const history = useHistory();
	const [initialValue, setInitialValue] = useState({
		name: '',
	});

	const handleCreate = async () => {
		await saveNewWorkerTeam();
		props.handleHasChanged();
		props.handleToggle();
		return false;
	};

	const handleClose = async () => props.handleToggle();

	const saveNewWorkerTeam = async () => {
		try {
			const newWorkerTeam = new WorkerTeam(0, initialValue.name);
			await APIClient.createWorkerTeam(newWorkerTeam);
		} catch (error: any) {
			const errorMessage = 'Worker teams (POST) - ' + error.toString();
			console.log(errorMessage);
			history.push('/error', errorMessage);
		}
	};

	const handleNameChange = (value: string) => setInitialValue({ name: value });

	const validationSchema = () => {
		return Yup.object().shape({
			name: Yup.string().max(256, 'Name must be less than 256 characters').required('Name is required'),
		});
	};

	return (
		<PopupComponent show={props.show}>
			<PopupHeaderComponent headerMessage='Create a new worker team:' handleClose={handleClose} />
			<Formik
				enableReinitialize={true}
				initialValues={initialValue}
				validationSchema={validationSchema}
				onSubmit={handleCreate}
			>
				<Form>
					{/* Name */}
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
					<PopupCreateSubmitComponent/>
				</Form>
			</Formik>
		</PopupComponent>
	);
};

export default CreateWorkerTeamPopupComponent;
