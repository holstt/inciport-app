import { FC, useContext, useEffect, useMemo, useState } from 'react';
import { APIClient } from '../../../services/ApiClient';
import Dashboard from './Dashboard';
import { useHistory } from 'react-router-dom';
import { FilterComponent } from '../../basecomponents/FilterComponent';
import MainCategoryOptions from '../../../models/MainCategoryOptions';
import { Context } from '../../../Context';
import CreateMainCategoryPopupComponent from '../popupcomponents/CreateMainCategoryPopupComponent';
import EditIcon from '@mui/icons-material/Edit';

export const CategoryDashboardComponent: FC = () => {
	const { currentUser, updateSelectedMainCategory } = useContext(Context);
	const history = useHistory();
	const defaultCategories: MainCategoryOptions[] = [];
	const [categories, setCategories] = useState<MainCategoryOptions[]>(defaultCategories);
	const [filterText, setFilterText] = useState('');
	const [resetPaginationToggle, setResetPaginationToggle] = useState(false);
	const [showCreate, setShowCreate] = useState<boolean>(false);
	const [hasChanged, setHasChanged] = useState<boolean>(false);

	const handleCreateToggle = () => {
		setShowCreate(!showCreate);
	};

	const handleHasChanged = () => {
		setHasChanged(!hasChanged);
	};

	useEffect(() => {
		const getCategories = async () => {
			try {
				const response = await APIClient.getMainCategories();
				setCategories(response.data);
			} catch (error: any) {
				const errorMessage = 'Categories (GET) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		};
		getCategories();
	}, [hasChanged]);

	const columns = [
		{
			name: 'ID',
			selector: (row) => row.id,
			sortable: true,
			reorder: true,
			grow: 0.8,
		},
		{
			name: 'Title',
			selector: (row) => row.title,
			sortable: true,
			reorder: true,
			grow: 0.6,
		},
		{
			name: 'Edit',
			cell: (row) => (
				<button
					className='btn btn-secondary EditButton'
					onClick={async () => {
						onEditClick(row);
					}}
				>
					<EditIcon fontSize='small'/>
				</button>
			),
			button: true,
		},
	];

	const onEditClick = async (row: MainCategoryOptions) => {
		updateSelectedMainCategory(row);
		return history.push(`/municipalities/${currentUser.municipalityId}/categories/${row.id}/edit`);
	};

	const isFilterMatch = (inputString: string): boolean => inputString.toLowerCase().includes(filterText.toLowerCase());

	const filteredItems = categories.filter((item) => isFilterMatch(`${item.id}`) || isFilterMatch(item.title));

	const subHeaderComponentMemo = useMemo(() => {
		const handleClear = () => {
			if (filterText) {
				setResetPaginationToggle(!resetPaginationToggle);
				setFilterText('');
			}
		};

		return (
			<div className='SubHeaderMemoContainer'>
				<button
					className='btn btn-primary CreateButton'
					onClick={() => {
						setShowCreate(true);
					}}
				>
					Create
				</button>
				<FilterComponent onFilter={(e) => setFilterText(e.target.value)} onClear={handleClear} filterText={filterText} />
			</div>
		);
	}, [filterText, resetPaginationToggle]);

	return (
		<div>
			<CreateMainCategoryPopupComponent
				show={showCreate}
				handleHasChanged={handleHasChanged}
				handleToggle={handleCreateToggle}
			/>
			<Dashboard
				title={''}
				data={categories}
				filteredItems={filteredItems}
				subHeaderComponentMemo={subHeaderComponentMemo}
				resetPaginationToggle={resetPaginationToggle}
				columns={columns}
				onRowDoubleClicked={onEditClick}
			/>
		</div>
	);
};

export default CategoryDashboardComponent;
