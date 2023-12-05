import { Formik, Form, Field, ErrorMessage } from 'formik';
import { FC, useState } from 'react';
import { APIClient } from '../../../services/ApiClient';
import PopupComponent from '../../basecomponents/PopupComponent';
import * as Yup from 'yup';
import MainCategoryOptions from '../../../models/MainCategoryOptions';
import { useHistory } from 'react-router-dom';
import PopupHeaderComponent from '../../basecomponents/PopupHeaderComponent';
import PopupCreateSubmitComponent from '../../basecomponents/PopupCreateSubmitComponent';

interface Props {
	show: boolean;
	handleHasChanged: () => void;
	handleToggle: () => void;
}

export const CreateMainCategoryPopupComponent: FC<Props> = (props) => {
	const history = useHistory();
	const [initialValues, setInitialValues] = useState({
		title: '',
		subCategories: [],
	});

	const handleCreate = async () => {
		await saveNewMainCategory();
		props.handleHasChanged();
		props.handleToggle();
		return false;
	};

	const handleClose = async () => props.handleToggle();

	const saveNewMainCategory = async () => {
		try {
			const newMainCategory = new MainCategoryOptions(0, initialValues.title, initialValues.subCategories);
			await APIClient.createNewMainCategory(newMainCategory);
		} catch (error: any) {
			const errorMessage = 'Categories (POST) - ' + error.toString();
			console.log(errorMessage);
			history.push('/error', errorMessage);
		}
	};

	const handleTitleChange = (value: string) => setInitialValues({ ...initialValues, title: value });

	const validationSchema = () =>
		Yup.object().shape({
			title: Yup.string().max(256, 'Title must be less than 256 characters').required('Title is required'),
		});

	return (
		<PopupComponent show={props.show}>
			<PopupHeaderComponent headerMessage='Create a new category:' handleClose={handleClose} />
			<Formik
				enableReinitialize={true}
				initialValues={initialValues}
				validationSchema={validationSchema}
				onSubmit={handleCreate}
			>
				<Form>
					{/* Title */}
					<div className='form-group'>
						<label htmlFor='title'>Title</label>
						<Field
							name='title'
							type='text'
							onChange={(event) => {
								handleTitleChange(event.currentTarget.value);
							}}
							className='form-control'
						/>
						<ErrorMessage name='title' component='div' className='alert alert-danger ErrorMessage' />
					</div>

					{/* Submit */}
					<PopupCreateSubmitComponent/>
				</Form>
			</Formik>
		</PopupComponent>
	);
};

export default CreateMainCategoryPopupComponent;
