import { FC, useMemo, useState, useContext, useEffect } from 'react';
import { Context } from '../../../Context';
import Municipality from '../../../models/Municipality';
import { APIClient } from '../../../services/ApiClient';
import FilterComponent from '../../basecomponents/FilterComponent';
import Dashboard from './Dashboard';
import { useHistory } from 'react-router-dom';
import { CreateMunicipalityPopupComponent } from '../popupcomponents/CreateMunicipalityPopupComponent';
import authService from '../../../services/authService';
import EditIcon from '@mui/icons-material/Edit';

export const MunicipalityDashboardComponent: FC = () => {
	const { currentUser, updateCurrentUser } = useContext(Context);
	const history = useHistory();
	const [municipalities, setMunicipalities] = useState<Municipality[]>([]);
	const [filterText, setFilterText] = useState('');
	const [resetPaginationToggle, setResetPaginationToggle] = useState(false);
	const [showCreate, setShowCreate] = useState<boolean>(false);
	const [hasChanged, setHasChanged] = useState<boolean>(false);

	const handleCreateToggle = () => setShowCreate(!showCreate);

	const handleHasChanged = () => setHasChanged(!hasChanged);

	useEffect(() => {
		const getMunicipalities = async () => {
			try {
				const response = await APIClient.getMunicipalities();
				setMunicipalities(response.data);
			} catch (error: any) {
				const errorMessage = 'Municipalities (GET) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		};
		getMunicipalities();
	}, [hasChanged]);

	const columns = [
		{
			name: 'ID',
			selector: (row) => row.id,
			sortable: true,
			reorder: true,
			grow: 0.2,
		},
		{
			name: 'Name',
			selector: (row) => row.name,
			sortable: true,
			reorder: true,
			grow: 0.6,
		},
		{
			name: 'Incident report count',
			selector: (row) => row.incidentReportCount,
			sortable: true,
			reorder: true,
			grow: 0.6,
		},
		{
			name: 'Worker count',
			selector: (row) => row.workerCount,
			sortable: true,
			reorder: true,
			grow: 0.6,
		},
		{
			name: 'Category count',
			selector: (row) => row.categoriesCount,
			sortable: true,
			reorder: true,
			grow: 0.6,
		},
		{
			name: 'User count',
			selector: (row) => row.usersCount,
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

	const onEditClick = async (row: Municipality) => {
		updateCurrentUser({
			...currentUser,
            municipalityId: row.id,
            municipalityName: row.name
		});
		const result = authService.setMunicipalityOnUser(row.id, row.name);
		if (result) {
			history.push(`/municipalities/${row.id}/admin`);
			window.location.reload();
		}
	};

	const isFilterMatch = (inputString: string): boolean => inputString.toLowerCase().includes(filterText.toLowerCase());

	const filteredItems = municipalities.filter(
		(item) =>
			isFilterMatch(`${item.id}`) ||
			isFilterMatch(item.name) ||
			isFilterMatch(`${item.incidentReportCount}`) ||
			isFilterMatch(`${item.workerCount}`) ||
			isFilterMatch(`${item.categoriesCount}`) ||
			isFilterMatch(`${item.usersCount}`)
	);

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
			<CreateMunicipalityPopupComponent
				show={showCreate}
				handleHasChanged={handleHasChanged}
				handleToggle={handleCreateToggle}
			/>
			<Dashboard
				title={'Municipalities'}
				data={municipalities}
				filteredItems={filteredItems}
				subHeaderComponentMemo={subHeaderComponentMemo}
				resetPaginationToggle={resetPaginationToggle}
				columns={columns}
				onRowDoubleClicked={onEditClick}
			/>
		</div>
	);
};

export default MunicipalityDashboardComponent;
