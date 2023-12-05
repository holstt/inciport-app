import { FC, useContext, useState } from 'react';
import authService from '../../services/authService';
import SubCategoryDashboardComponent from '../compositecomponents/dashboardcomponents/SubCategoryDashboardComponent';
import { useHistory } from 'react-router-dom';
import { Context } from '../../Context';
import { APIClient } from '../../services/ApiClient';
import NameButtonsComponent from '../compositecomponents/NameButtonsComponent';

interface Props {
	handleUnauthorized: () => null;
}

export const CategoryPage: FC<Props> = (props) => {
	const [isAuthorized] = useState(authService.getCurrentUser() != null ? true : false);
	const { mainCategory, updateSelectedMainCategory, isMainCategoryUpdated, updateIsMainCategoryUpdated } =
		useContext(Context);
	const history = useHistory();

	const handleTitleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		if (event.target.value.length <= 256) {
			event.target.style.border = 'solid black 2px';
			updateIsMainCategoryUpdated(true);
			updateSelectedMainCategory({
				...mainCategory,
				title: event.target.value,
			});
		} else {
			event.target.style.border = 'solid red 2px';
		}
	}

	const onSave = async () => {
		if (window.confirm('Are you sure you want to save the following changes?')) {
			try {
				const response = await APIClient.updateMainCategory(mainCategory);
				updateSelectedMainCategory(response.data);
				updateIsMainCategoryUpdated(false);
			} catch (error: any) {
				const errorMessage = 'Categories (PUT) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		}
	};

	const onCancel = () => {
		if (isMainCategoryUpdated) {
			if (window.confirm(`Are you sure you want to cancel and delete the changes that you've made?`)) {
				history.goBack();
			}
		} else {
			history.goBack();
		}
	};

	const onDelete = async () => {
		const confirmationMessage =
			'Are you sure you want to delete the following main category?\n' +
			`ID: ${mainCategory.id}\n` +
			`Title: ${mainCategory.title}\n`;
		if (window.confirm(confirmationMessage)) {
			try {
				await APIClient.deleteMainCategory(mainCategory.id);
				history.goBack();
			} catch (error: any) {
				const errorMessage = 'Categories (DELETE) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		}
	};

	return (
		<div>
			{isAuthorized ? (
				<div className='CategoryContainer'>
					<NameButtonsComponent
						name={mainCategory.title}
						nameOfWhat='Category'
						handleOnChange={handleTitleChange}
						onSave={onSave}
						onCancel={onCancel}
						onDelete={onDelete}
						disabledCondition={!isMainCategoryUpdated}
					/>
					<SubCategoryDashboardComponent />
				</div>
			) : (
				props.handleUnauthorized()
			)}
		</div>
	);
};

export default CategoryPage;
