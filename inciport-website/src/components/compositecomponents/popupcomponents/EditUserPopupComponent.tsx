import { Formik, Form, Field, ErrorMessage } from 'formik';
import { FC, useContext } from 'react';
import { Context } from '../../../Context';
import { APIClient } from '../../../services/ApiClient';
import PopupComponent from '../../basecomponents/PopupComponent';
import * as Yup from 'yup';
import AuthorizedUser from '../../../models/AuthorizedUser';
import { useHistory } from 'react-router-dom';
import PopupHeaderComponent from '../../basecomponents/PopupHeaderComponent';
import PopupEditSubmitComponent from '../../basecomponents/PopupEditSubmitComponent';

interface Props {
	show: boolean;
	handleHasChanged: () => void;
	handleToggle: () => void;
	currentUser: AuthorizedUser | null;
	roleOptions: string[];
}

export const EditUserPopupComponent: FC<Props> = (props) => {
	const history = useHistory();
	const { user, updateSelectedUser } = useContext(Context);

	const handleSave = async () => {
		await updateUser();
		props.handleToggle();
		props.handleHasChanged();
		return false;
	};

	const updateUser = async () => {
		try {
			await APIClient.updateUser(user);
		} catch (error: any) {
			const errorMessage = 'Users (PUT) - ' + error.toString();
			console.log(errorMessage);
			history.push('/error', errorMessage);
		}
	};

	const handleDelete = async () => {
		await deleteUser();
		props.handleToggle();
		props.handleHasChanged();
		return false;
	};

	const deleteUser = async () => {
		const confirmationMessage =
			'Are you sure you want to delete the following user?\n' +
			`ID: ${user.id}\n` +
			`Full name: ${user.fullName}\n` +
			`Email: ${user.email}\n` +
			`Role: ${user.role}\n`;
		if (window.confirm(confirmationMessage)) {
			try {
				await APIClient.deleteUser(user.id);
			} catch (error: any) {
				const errorMessage = 'Users (DELETE) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		}
	};

	const handleClose = async () => props.handleToggle();

	const handleNameChange = (value: string) => updateSelectedUser({ ...user, fullName: value });

	const handleEmailChange = (value: string) => updateSelectedUser({ ...user, email: value });

	const handleRoleChange = (value: string) => updateSelectedUser({ ...user, role: value });

	const setInitialSelected = () => user.role.toLowerCase();

	const validationSchema = () => {
		return Yup.object().shape({
			fullName: Yup.string().max(256, 'Full name must be less than 256 characters').required('Full name is required'),
			email: Yup.string()
				.email('Email is not valid')
				.max(256, 'Email must be less than 256 characters')
				.required('Email is required'),
		});
	};

	return (
		<PopupComponent show={props.show}>
			<PopupHeaderComponent headerMessage='Edit user:' handleClose={handleClose} />
			<Formik enableReinitialize={true} initialValues={user} onSubmit={handleSave} validationSchema={validationSchema}>
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
								value={setInitialSelected()}
							>
								{props.roleOptions.map((role) => {
									return <option value={role}>{role}</option>;
								})}
							</Field>
							<ErrorMessage name='role' component='div' className='alert alert-danger ErrorMessage' />
						</div>
					) : null}

					{/* Submit */}
					<PopupEditSubmitComponent handleDelete={handleDelete} />
				</Form>
			</Formik>
		</PopupComponent>
	);
};

export default EditUserPopupComponent;
