import { FC, useContext, useEffect, useMemo, useState } from 'react';
import { APIClient } from '../../../services/ApiClient';
import Dashboard from './Dashboard';
import { FilterComponent } from '../../basecomponents/FilterComponent';
import User from '../../../models/User';
import EditUserPopupComponent from '../popupcomponents/EditUserPopupComponent';
import CreateUserPopupComponent from '../popupcomponents/CreateUserPopupComponent';
import { Context } from '../../../Context';
import AuthorizedUser from '../../../models/AuthorizedUser';
import authService from '../../../services/authService';
import { useHistory } from 'react-router-dom';
import EditIcon from '@mui/icons-material/Edit';

export const UserDashboardComponent: FC = () => {
	const history = useHistory();
	const { updateSelectedUser } = useContext(Context);
	const [users, setUsers] = useState<User[]>([]);
	const [filterText, setFilterText] = useState('');
	const [resetPaginationToggle, setResetPaginationToggle] = useState(false);
	const [showEdit, setShowEdit] = useState<boolean>(false);
	const [showCreate, setShowCreate] = useState<boolean>(false);
	const [hasChanged, setHasChanged] = useState<boolean>(false);
	const [currentUser] = useState<AuthorizedUser | null>(authService.getCurrentUser());
	const roleOptions = ['manager', 'admin'];

	const handleEditToggle = () => setShowEdit(!showEdit);

	const handleCreateToggle = () => setShowCreate(!showCreate);

	const handleHasChanged = () => setHasChanged(!hasChanged);

	useEffect(() => {
		const getUsers = async () => {
			try {
				const response = await APIClient.getUsers();
				setUsers(response.data);
			} catch (error: any) {
				const errorMessage = 'Users (GET) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		};
		getUsers();
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
			name: 'Full name',
			selector: (row) => row.fullName,
			sortable: true,
			reorder: true,
			grow: 0.6,
		},
		{
			name: 'Email',
			selector: (row) => row.email,
			sortable: true,
			reorder: true,
			grow: 0.6,
		},
		{
			name: 'Municipality name',
			selector: (row) => row.municipalityName,
			sortable: true,
			reorder: true,
			grow: 0.6,
		},
		{
			name: 'Role',
			selector: (row) => row.role,
			sortable: true,
			reorder: true,
			grow: 0.6,
		},
		{
			name: 'Edit',
			cell: (row) =>
				hasEditPermissions(row) ? (
					<button
						className='btn btn-secondary EditButton'
						onClick={async () => {
							onEditClick(row);
						}}
					>
						<EditIcon fontSize='small'/>
					</button>
				) : (
					<button
						className='btn btn-secondary EditButton'
						disabled={true}
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

	const onEditClick = async (row: User) => {
		if (hasEditPermissions(row)) {
			updateSelectedUser(row);
			setShowEdit(true);
		}
	};

	const hasEditPermissions = (row: User) => {
		if (currentUser?.role.toLowerCase() === 'maintainer' || row.role.toLowerCase() === 'manager') {
			return true;
		} else {
			return false;
		}
	};

	const isFilterMatch = (inputString: string): boolean => inputString.toLowerCase().includes(filterText.toLowerCase());

	const filteredItems = users.filter(
		(item) =>
			isFilterMatch(item.fullName) ||
			isFilterMatch(item.email) ||
			isFilterMatch(item.municipalityName) ||
			isFilterMatch(item.role)
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
			<EditUserPopupComponent
				show={showEdit}
				handleHasChanged={handleHasChanged}
				handleToggle={handleEditToggle}
				currentUser={currentUser}
				roleOptions={roleOptions}
			/>
			<CreateUserPopupComponent
				show={showCreate}
				handleHasChanged={handleHasChanged}
				handleToggle={handleCreateToggle}
				currentUser={currentUser}
				roleOptions={roleOptions}
			/>
			<Dashboard
				title={''}
				data={users}
				filteredItems={filteredItems}
				subHeaderComponentMemo={subHeaderComponentMemo}
				resetPaginationToggle={resetPaginationToggle}
				columns={columns}
				onRowDoubleClicked={onEditClick}
			/>
		</div>
	);
};

export default UserDashboardComponent;
