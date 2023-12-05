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

export const EditSubCategoryPopupComponent: FC<Props> = (props) => {
	const history = useHistory();
	const { mainCategory, subCategory, updateSelectedSubCategory } = useContext(Context);

	const handleSave = async () => {
		await updateSubCategory();
		props.handleToggle();
		props.handleHasChanged();
		return false;
	};

	const updateSubCategory = async () => {
		try {
			await APIClient.updateSubCategory(mainCategory.id, subCategory);
		} catch (error: any) {
			const errorMessage = 'Subcategories (PUT) - ' + error.toString();
			console.log(errorMessage);
			history.push('/error', errorMessage);
		}
	};

	const handleDelete = async () => {
		await deleteSubCategory();
		props.handleToggle();
		props.handleHasChanged();
		return false;
	};

	const deleteSubCategory = async () => {
		const confirmationMessage =
			'Are you sure you want to delete the following sub category?\n' +
			`ID: ${subCategory.id}\n` +
			`Title: ${subCategory.title}`;
		if (window.confirm(confirmationMessage)) {
			try {
				await APIClient.deleteSubCategory(mainCategory.id, subCategory.id);
			} catch (error: any) {
				const errorMessage = 'Subcategories (DELETE) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		}
	};

	const handleClose = async () => props.handleToggle();

	const handleTitleChange = (value: string) => updateSelectedSubCategory({ ...subCategory, title: value });

	const validationSchema = () => {
		return Yup.object().shape({
			title: Yup.string().max(256, 'Title must be less than 256 characters').required('Title is required'),
		});
	};

	return (
		<PopupComponent show={props.show}>
			<PopupHeaderComponent headerMessage='Edit subcategory:' handleClose={handleClose} />
			<Formik
				enableReinitialize={true}
				initialValues={subCategory}
				validationSchema={validationSchema}
				onSubmit={handleSave}
			>
				<Form>
					{/* Full name */}
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
					<PopupEditSubmitComponent handleDelete={handleDelete} />
				</Form>
			</Formik>
		</PopupComponent>
	);
};

export default EditSubCategoryPopupComponent;
