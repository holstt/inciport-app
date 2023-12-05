import { FC, useContext, useState, useEffect, useMemo } from 'react';
import { Context } from '../../../Context';
import Category from '../../../models/Category';
import { APIClient } from '../../../services/ApiClient';
import FilterComponent from '../../basecomponents/FilterComponent';
import Dashboard from './Dashboard';
import EditSubCategoryPopupComponent from '../popupcomponents/EditSubCategoryPopupComponent';
import CreateSubCategoryPopupComponent from '../popupcomponents/CreateSubCategoryPopupComponent';
import { useHistory } from 'react-router-dom';
import EditIcon from '@mui/icons-material/Edit';

export const SubCategoryDashboardComponent: FC = () => {
	const history = useHistory();
	const { mainCategory, updateSelectedSubCategory } = useContext(Context);
	const defaultSubCategories: Category[] = [];
	const [subCategories, setSubCategories] = useState<Category[]>(defaultSubCategories);
	const [filterText, setFilterText] = useState('');
	const [resetPaginationToggle, setResetPaginationToggle] = useState(false);
	const [showEdit, setShowEdit] = useState<boolean>(false);
	const [showCreate, setShowCreate] = useState<boolean>(false);
	const [hasChanged, setHasChanged] = useState<boolean>(false);

	const handleEditToggle = () => setShowEdit(!showEdit);

	const handleCreateToggle = () => setShowCreate(!showCreate);

	const handleHasChanged = () => setHasChanged(!hasChanged);

	useEffect(() => {
		const getSubCategories = async () => {
			try {
				const response = await APIClient.getSubCategories(mainCategory.id);
				setSubCategories(response.data);
			} catch (error: any) {
				const errorMessage = 'Subcategories (GET) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		};
		getSubCategories();
	}, [hasChanged, mainCategory.id]);

	const columns = [
		{
			name: 'ID',
			selector: (row) => row.id,
			sortable: true,
			reorder: true,
			grow: 0.6,
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

	const onEditClick = async (row: Category) => {
		updateSelectedSubCategory(row);
		setShowEdit(true);
	};

	const isFilterMatch = (inputString: string): boolean => inputString.toLowerCase().includes(filterText.toLowerCase());

	const filteredItems = subCategories.filter((item) => isFilterMatch(`${item.id}`) || isFilterMatch(item.title));

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
			<EditSubCategoryPopupComponent show={showEdit} handleHasChanged={handleHasChanged} handleToggle={handleEditToggle} />
			<CreateSubCategoryPopupComponent
				show={showCreate}
				handleHasChanged={handleHasChanged}
				handleToggle={handleCreateToggle}
			/>
			<Dashboard
				title={'Sub categories'}
				data={subCategories}
				filteredItems={filteredItems}
				subHeaderComponentMemo={subHeaderComponentMemo}
				resetPaginationToggle={resetPaginationToggle}
				columns={columns}
				onRowDoubleClicked={onEditClick}
			/>
		</div>
	);
};

export default SubCategoryDashboardComponent;
