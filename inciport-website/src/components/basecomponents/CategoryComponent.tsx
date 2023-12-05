import { FC, useState, useEffect, useContext } from 'react';
import { APIClient } from '../../services/ApiClient';
import { Context } from '../../Context';
import MainCategoryOptions from '../../models/MainCategoryOptions';
import Category from '../../models/Category';
import ChosenMainCategory from '../../models/ChosenMainCategory';
import { useHistory } from 'react-router-dom';
import '../../css/CategoryStyles.css';

export const CategoryComponent: FC = () => {
	const history = useHistory();
	const { incidentReport, updateCurrentIncidentReport, updateIsIncidentReportUpdated } = useContext(Context);
	const [currentlySelectedMainCategory, setCurrentlySelectedMainCategory] = useState<number>(
		incidentReport.chosenMainCategory.id
	);
	const [currentlySelectedSubCategory, setCurrentlySelectedSubCategory] = useState<number | undefined>(
		incidentReport.chosenMainCategory.chosenSubCategory?.id
	);
	const [options, setOptions] = useState<MainCategoryOptions[]>([]);
	const [subCategoryOptions, setSubCategoryOptions] = useState<Category[]>([]);
	const [mainMatch, setMainMatch] = useState<boolean>(false);
	const [subMatch, setSubMatch] = useState<boolean>(false);

	useEffect(() => {
		const getCategories = async () => {
			try {
				const response = await APIClient.getMainCategories();
				setOptions(response.data);
				for (let i = 0; i < response.data.length; i++) {
					if (response.data[i].id === currentlySelectedMainCategory) {
						setMainMatch(true);
						setSubCategoryOptions(response.data[i].subCategories);
						for (let j = 0; j < response.data[i].subCategories.length; j++) {
							if (response.data[i].subCategories[j].id === currentlySelectedSubCategory) {
								setSubMatch(true);
							}
						}
					}
				}
			} catch (error: any) {
				const errorMessage = 'Categories (GET) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		};
		getCategories();
	}, []);

	const handleOnMainCategoryChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
		const id = Number(event.currentTarget.value);
		const chosenMainCategory: MainCategoryOptions | undefined = options.find((x) => x.id === id);
		if (chosenMainCategory === undefined || chosenMainCategory == null) {
			return;
		}
		updateIsIncidentReportUpdated(true);
		setMainMatch(true);
		const newMainCategory = new ChosenMainCategory(chosenMainCategory?.id, chosenMainCategory?.title, null);
		updateCurrentIncidentReport({
			...incidentReport,
			chosenMainCategory: { ...newMainCategory, chosenSubCategory: chosenMainCategory.subCategories[0] },
		});

		setCurrentlySelectedMainCategory(id);
		determineSubCategoryOptions(id);
	};

	const determineSubCategoryOptions = (mainCategoryId: number) => {
		for (let i = 0; i < options.length; i++) {
			if (options[i].id === mainCategoryId) {
				setSubCategoryOptions(options[i].subCategories);
			}
		}
	};

	const handleOnSubCategoryChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
		const id = Number(event.currentTarget.value);
		updateIsIncidentReportUpdated(true);
		setSubMatch(true);
		const newSubCategory: Category | undefined = subCategoryOptions?.find((x) => x.id === id);
		if (newSubCategory !== undefined || newSubCategory != null) {
			updateCurrentIncidentReport({
				...incidentReport,
				chosenMainCategory: { ...incidentReport.chosenMainCategory, chosenSubCategory: newSubCategory },
			});
			setCurrentlySelectedSubCategory(id);
		}
	};

	return (
		<div className='CategoryContainer'>
			<h4 className='OverviewMessage'>Category:</h4>
			<div className='Grid'>
				<select className='form-control DropDown Block' onChange={handleOnMainCategoryChange}>
					{options?.map((item) => {
						if (item.id === currentlySelectedMainCategory) {
							return (
								<option selected={true} value={item.id}>
									{item.title}
								</option>
							);
						}
						return <option value={item.id}>{item.title}</option>;
					})}
					{mainMatch ? '' : <option selected={true} value={0}></option>}
				</select>
				<select className='form-control DropDown Block' onChange={handleOnSubCategoryChange}>
					{subCategoryOptions?.map((item) => {
						if (item.id === currentlySelectedSubCategory) {
							return (
								<option selected={true} value={item.id}>
									{item.title}
								</option>
							);
						}
						return <option value={item.id}>{item.title}</option>;
					})}
					{subMatch ? '' : <option selected={true} value={0}></option>}
				</select>
			</div>
		</div>
	);
};

export default CategoryComponent;
