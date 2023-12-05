import { Formik, Form, Field, ErrorMessage } from 'formik';
import { FC, useContext, useState } from 'react';
import { APIClient } from '../../../services/ApiClient';
import PopupComponent from '../../basecomponents/PopupComponent';
import * as Yup from 'yup';
import Category from '../../../models/Category';
import { Context } from '../../../Context';
import { useHistory } from 'react-router-dom';
import PopupHeaderComponent from '../../basecomponents/PopupHeaderComponent';
import PopupCreateSubmitComponent from '../../basecomponents/PopupCreateSubmitComponent';

interface Props {
	show: boolean;
	handleHasChanged: () => void;
	handleToggle: () => void;
}

export const CreateSubCategoryPopupComponent: FC<Props> = (props) => {
	const history = useHistory();
	const { mainCategory, subCategory } = useContext(Context);
	const [initialValues, setInitialValues] = useState({
		title: '',
	});

	const handleCreate = async () => {
		await saveNewSubCategory();
		props.handleHasChanged();
		props.handleToggle();
		return false;
	};

	const handleClose = async () => props.handleToggle();

	const saveNewSubCategory = async () => {
		try {
			const newSubCategory = new Category(subCategory.id, initialValues.title);
			await APIClient.createSubCategory(mainCategory.id, newSubCategory);
		} catch (error: any) {
			const errorMessage = 'Subcategories (POST) - ' + error.toString();
			console.log(errorMessage);
			history.push('/error', errorMessage);
		}
	};

	const handleTitleChange = (value: string) => setInitialValues({ title: value });

	const validationSchema = () => {
		return Yup.object().shape({
			title: Yup.string().max(256, 'Title must be less than 256 characters').required('Title is required'),
		});
	};

	return (
		<PopupComponent show={props.show}>
			<PopupHeaderComponent headerMessage='Create a new subcategory:' handleClose={handleClose} />
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

export default CreateSubCategoryPopupComponent;
