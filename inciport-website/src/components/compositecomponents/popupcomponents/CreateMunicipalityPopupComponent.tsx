import { ErrorMessage, Field, Form, Formik } from 'formik';
import { FC, useState } from 'react';
import { APIClient } from '../../../services/ApiClient';
import PopupComponent from '../../basecomponents/PopupComponent';
import * as Yup from 'yup';
import { useHistory } from 'react-router-dom';
import PopupHeaderComponent from '../../basecomponents/PopupHeaderComponent';
import '../../../css/PopupStyles.css';
import PopupCreateSubmitComponent from '../../basecomponents/PopupCreateSubmitComponent';

interface Props {
	show: boolean;
	handleHasChanged: () => void;
	handleToggle: () => void;
}

export const CreateMunicipalityPopupComponent: FC<Props> = (props) => {
	const history = useHistory();
	const [initialValues, setInitialValues] = useState({
		name: '',
	});

	const handleCreate = async () => {
		await saveNewMunicipality();
		props.handleHasChanged();
		props.handleToggle();
		return false;
	};

	const handleClose = async () => props.handleToggle();

	const saveNewMunicipality = async () => {
		try {
			await APIClient.createMunicipality(initialValues.name);
		} catch (error: any) {
			const errorMessage = 'Municipalities (POST) - ' + error.toString();
			console.log(errorMessage);
			history.push('/error', errorMessage);
		}
	};

	const handleNameChange = (value: string) => setInitialValues({ ...initialValues, name: value });

	const validationSchema = () =>
		Yup.object().shape({
			name: Yup.string().max(256, 'Name must be less than 256 characters').required('Name is required'),
		});

	return (
		<PopupComponent show={props.show}>
			<PopupHeaderComponent headerMessage='Create a new municipality:' handleClose={handleClose} />
			<Formik
				enableReinitialize={true}
				initialValues={initialValues}
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
					<PopupCreateSubmitComponent />
				</Form>
			</Formik>
		</PopupComponent>
	);
};
