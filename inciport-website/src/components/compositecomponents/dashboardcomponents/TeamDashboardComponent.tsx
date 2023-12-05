import { FC, useEffect, useMemo, useState, useContext } from 'react';
import { APIClient } from '../../../services/ApiClient';
import Dashboard from './Dashboard';
import { FilterComponent } from '../../basecomponents/FilterComponent';
import WorkerTeam from '../../../models/WorkerTeam';
import EditWorkerTeamPopupComponent from '../popupcomponents/EditWorkerTeamPopupComponent';
import { Context } from '../../../Context';
import CreateWorkerTeamPopupComponent from '../popupcomponents/CreateWorkerTeamPopupComponent';
import { useHistory } from 'react-router-dom';
import EditIcon from '@mui/icons-material/Edit';

export const TeamDashboardComponent: FC = () => {
	const history = useHistory();
	const { updateSelectedWorkerTeam } = useContext(Context);
	const defaultTeams: WorkerTeam[] = [];
	const [teams, setTeams] = useState<WorkerTeam[]>(defaultTeams);
	const [filterText, setFilterText] = useState('');
	const [resetPaginationToggle, setResetPaginationToggle] = useState(false);
	const [showEdit, setShowEdit] = useState<boolean>(false);
	const [showCreate, setShowCreate] = useState<boolean>(false);
	const [hasChanged, setHasChanged] = useState<boolean>(false);

	const handleEditToggle = () => {
		setShowEdit(!showEdit);
	};

	const handleCreateToggle = () => {
		setShowCreate(!showCreate);
	};

	const handleHasChanged = () => {
		setHasChanged(!hasChanged);
	};

	useEffect(() => {
		const getWorkerTeams = async () => {
			try {
				const response = await APIClient.getWorkerTeams();
				setTeams(response.data);
			} catch (error: any) {
				const errorMessage = 'Worker teams (GET) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		};
		getWorkerTeams();
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
			name: 'Name',
			selector: (row) => row.name,
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

	const onEditClick = async (row: WorkerTeam) => {
		updateSelectedWorkerTeam(row);
		setShowEdit(true);
	};

	const isFilterMatch = (inputString: string): boolean => {
		if (inputString === undefined) {
			return false;
		}

		return inputString.toLowerCase().includes(filterText.toLowerCase());
	};

	const filteredItems = teams.filter((item) => isFilterMatch(`${item.id}`) || isFilterMatch(item.name));

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
			<EditWorkerTeamPopupComponent show={showEdit} handleHasChanged={handleHasChanged} handleToggle={handleEditToggle} />
			<CreateWorkerTeamPopupComponent
				show={showCreate}
				handleHasChanged={handleHasChanged}
				handleToggle={handleCreateToggle}
			/>
			<Dashboard
				title={''}
				data={teams}
				filteredItems={filteredItems}
				subHeaderComponentMemo={subHeaderComponentMemo}
				resetPaginationToggle={resetPaginationToggle}
				columns={columns}
				onRowDoubleClicked={onEditClick}
			/>
		</div>
	);
};

export default TeamDashboardComponent;
