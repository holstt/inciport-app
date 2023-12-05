import { Formik, Form, Field, ErrorMessage } from 'formik';
import { FC, useState } from 'react';
import { APIClient } from '../../../services/ApiClient';
import PopupComponent from '../../basecomponents/PopupComponent';
import * as Yup from 'yup';
import UserDTO from '../../../models/UserDTO';
import AuthorizedUser from '../../../models/AuthorizedUser';
import { useHistory } from 'react-router-dom';
import PopupHeaderComponent from '../../basecomponents/PopupHeaderComponent';
import PopupCreateSubmitComponent from '../../basecomponents/PopupCreateSubmitComponent';

interface Props {
	show: boolean;
	handleHasChanged: () => void;
	handleToggle: () => void;
	currentUser: AuthorizedUser | null;
	roleOptions: string[];
}

export const CreateUserPopupComponent: FC<Props> = (props) => {
	const history = useHistory();
	const [initialValues, setInitialValues] = useState({
		fullName: '',
		email: '',
		password: '',
		role: 'Manager',
	});

	const handleCreate = async () => {
		await saveNewUser();
		props.handleHasChanged();
		props.handleToggle();
		return false;
	};

	const handleClose = async () => props.handleToggle();

	const saveNewUser = async () => {
		try {
			const newUser = new UserDTO(initialValues.fullName, initialValues.email, initialValues.password, initialValues.role);
			await APIClient.createUser(newUser);
		} catch (error: any) {
			const errorMessage = 'Users (POST) - ' + error.toString();
			console.log(errorMessage);
			history.push('/error', errorMessage);
		}
	};

	const handleNameChange = (value: string) => setInitialValues({ ...initialValues, fullName: value });

	const handleEmailChange = (value: string) => setInitialValues({ ...initialValues, email: value });

	const handlePasswordChange = (value: string) => setInitialValues({ ...initialValues, password: value });

	const handleRoleChange = (value: string) => setInitialValues({ ...initialValues, role: value });

	const validationSchema = () => {
		return Yup.object().shape({
			fullName: Yup.string().max(256, 'Full name must be less than 256 characters').required('Full name is required'),
			email: Yup.string().email('Email is not valid').max(256, 'Email must be less than 256 characters').required('Email is required'),
			password: Yup.string()
				.test(
					'',
					'The password must be atleast 8 characters.\n' +
						'The password must be less than 40 characters\n' +
						'The password must contain atleast one digit\n' +
						'The password must contain atleast one uppercase letter\n' +
						'The password must contain one non alphanumeric character',
					(val: any) =>
						val &&
						val.toString().length >= 8 &&
						val.toString().length <= 40 &&
						/\d/.test(val.toString()) &&
						/[A-Z]/.test(val.toString())
				)
				.required('Password is required'),
		});
	};

	return (
		<PopupComponent show={props.show}>
			<PopupHeaderComponent headerMessage='Create a new user:' handleClose={handleClose} />
			<Formik
				enableReinitialize={true}
				initialValues={initialValues}
				validationSchema={validationSchema}
				onSubmit={handleCreate}
			>
				<Form>
					{/* Full name */}
					<div className='form-group'>
						<label htmlFor='fullName'>Full name</label>
						<Field
							name='fullName'
							type='text'
							onChange={(event) => {
								handleNameChange(event.currentTarget.value);
							}}
							className='form-control'
						/>
						<ErrorMessage name='fullName' component='div' className='alert alert-danger ErrorMessage' />
					</div>

					{/* Email */}
					<div className='form-group'>
						<label htmlFor='email'>Email</label>
						<Field
							name='email'
							type='text'
							onChange={(event) => {
								handleEmailChange(event.currentTarget.value);
							}}
							className='form-control'
						/>
						<ErrorMessage name='email' component='div' className='alert alert-danger ErrorMessage' />
					</div>

					{/* Password */}
					<div className='form-group'>
						<label htmlFor='password'>Password</label>
						<Field
							name='password'
							type='text'
							onChange={(event) => {
								handlePasswordChange(event.currentTarget.value);
							}}
							className='form-control'
						/>
						<ErrorMessage name='password' component='div' className='alert alert-danger ErrorMessage' />
					</div>

					{/* Role */}
					{props.currentUser?.role.toLowerCase() === 'maintainer' ? (
						<div className='form-group'>
							<label htmlFor='role'>Role</label>
							<Field 
								as='select'
								name='role'
								className='form-control'
								onChange={(event) => {
									handleRoleChange(event.currentTarget.value);
								}}
							>
								{props.roleOptions.map((role, index) => {
									return <option key={index} value={role}>{role}</option>
								})}
							</Field>
							<ErrorMessage name='role' component='div' className='alert alert-danger ErrorMessage' />
						</div>
					) : null}

					{/* Submit */}
					<PopupCreateSubmitComponent/>
				</Form>
			</Formik>
		</PopupComponent>
	);
};

export default CreateUserPopupComponent;
