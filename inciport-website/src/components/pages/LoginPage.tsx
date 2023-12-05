import { FC, useState, useContext } from 'react';
import { useHistory } from 'react-router-dom';
import { Formik, Field, Form, ErrorMessage } from 'formik';
import AuthService from '../../services/authService';
import AuthorizedUser from '../../models/AuthorizedUser';
import { Context } from '../../Context';
import '../../css/LoginStyles.css';

export const LoginPage: FC = () => {
	const { updateCurrentUser } = useContext(Context);
	const history = useHistory();
	const [loading, setLoading] = useState(false);
	const [statusMessage, setStatusMessage] = useState('');
	const initialValues = {
		email: '',
		password: '',
	};

	const handleLogin = (formValue: { email: string; password: string }) => {
		setStatusMessage('');
		setLoading(true);

		const login = async () => {
			try {
				const authorizedUser: AuthorizedUser = await AuthService.login(formValue.email, formValue.password);
				updateCurrentUser(authorizedUser);
				if (authorizedUser) {
					switch (authorizedUser.role) {
						case 'MANAGER':
							history.push(`/municipalities/${authorizedUser.municipalityId}/dashboard`);
							window.location.reload();
							break;
						case 'ADMIN':
							history.push(`/municipalities/${authorizedUser.municipalityId}/admin`);
							window.location.reload();
							break;
						case 'MAINTAINER':
							history.push('/municipalities/');
							window.location.reload();
							break;
						default:
							break;
					}
				} else {
					setLoading(false);
				}
			} catch (error: any) {
				const errorMessage = error.toString();
				setStatusMessage(errorMessage);
				setLoading(false);
			}
		};
		login();
	};

	return (
		<div className='col-md-12'>
			<div className='card card-container'>
			<label className='LoginLabel'>Inciport login</label>

				<Formik initialValues={initialValues} onSubmit={handleLogin}>
					<Form>
						{/* email */}
						<div className='form-group '>
							<label htmlFor='email'>Email</label>
							<Field name='email' id='email' type='text' className='form-control' />
							<ErrorMessage name='email' component='div' className='alert alert-danger' />
						</div>

						{/* Password */}
						<div className='form-group'>
							<label htmlFor='password'>Password</label>
							<Field name='password' id='password' type='password' className='form-control' />
							<ErrorMessage name='password' component='div' className='alert alert-danger' />
						</div>

						{/* Submit */}
						<div className='form-group'>
							<button type='submit' className='btn btn-primary SubmitButton' disabled={loading}>
								{loading && <span className='spinner-border spinner-border-sm'></span>}
								<span className='LoginSpan'>Login</span>
							</button>
						</div>

						{statusMessage && (
							<div className='form-group'>
								<div className='alert alert-danger' role='alert'>
									{statusMessage}
								</div>
							</div>
						)}
					</Form>
				</Formik>
			</div>
		</div>
	);
};

export default LoginPage;
