import { FC, useContext, useEffect, useMemo, useState } from 'react';
import { APIClient } from '../../../services/ApiClient';
import Dashboard from './Dashboard';
import IncidentReport from '../../../models/IncidentReport';
import { Context, contextDefaultValues } from '../../../Context';
import { useHistory } from 'react-router-dom';
import { FilterComponent } from '../../basecomponents/FilterComponent';
import EditIcon from '@mui/icons-material/Edit';

export const IncidentReportDashboardComponent: FC = () => {
	const { currentUser, updateCurrentIncidentReport } = useContext(Context);
	const history = useHistory();
	const [incidentReports, setIncidentReports] = useState<IncidentReport[]>([]);
	const [filterText, setFilterText] = useState('');
	const [resetPaginationToggle, setResetPaginationToggle] = useState(false);
	const [hasChanged] = useState<boolean>(false);

	useEffect(() => {
		const getIncidentReports = async () => {
			try {
				const response = await APIClient.getIncidentReports();
				setIncidentReports(response.data);
			} catch (error: any) {
				const errorMessage = 'Incident reports (GET) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		};
		getIncidentReports();
	}, [hasChanged]);

	const columns = [
		{
			name: 'ID',
			selector: (row) => row.id,
			sortable: true,
			reorder: true,
			grow: 0.2
		},
		{
			name: 'Address',
			selector: (row) =>
				'' +
				row.location.address.street +
				', ' +
				row.location.address.zipCode +
				', ' +
				row.location.address.city +
				', ' +
				row.location.address.country,
			sortable: true,
			reorder: true,
			grow: 2,
		},
		{
			name: 'Category',
			selector: (row) => row.chosenMainCategory.title,
			sortable: true,
			reorder: true,
			grow: 0.6,
		},
		{
			name: 'SubCategory',
			selector: (row) => row.chosenMainCategory.chosenSubCategory?.title,
			sortable: true,
			reorder: true,
			grow: 0.6,
		},
		{
			name: 'Status',
			selector: (row) => row.status,
			sortable: true,
			reorder: true,
			grow: 0.6,
		},
		{
			name: 'Description',
			selector: (row) => row.description,
			sortable: true,
			reorder: true,
			grow: 2,
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

	const onEditClick = async (row: IncidentReport) => {
		updateCurrentIncidentReport(row);
		return history.push(`/municipalities/${currentUser.municipalityId}/inciports/${row.id}/edit`);
	};

	// Needs to check for undefined due to mainCategories have the possibility to not have a subCategory.
	const isFilterMatch = (inputString: string | undefined): boolean => {
		if (inputString === undefined) {
			return false;
		}

		return inputString.toLowerCase().includes(filterText.toLowerCase());
	};

	const filteredItems = incidentReports.filter(
		(item) =>
			isFilterMatch(item.location.address.street) ||
			isFilterMatch(item.location.address.city) ||
			isFilterMatch(item.location.address.zipCode) ||
			isFilterMatch(item.location.address.country) ||
			
			isFilterMatch(item.chosenMainCategory.title) ||
			isFilterMatch(item.chosenMainCategory.chosenSubCategory?.title) ||
			isFilterMatch(item.status) ||
			isFilterMatch(item.description)
	);

	const subHeaderComponentMemo = useMemo(() => {
		const handleClear = () => {
			if (filterText) {
				setResetPaginationToggle(!resetPaginationToggle);
				setFilterText('');
			}
		};

		const onCreateClick = async () => {
			updateCurrentIncidentReport(contextDefaultValues.incidentReport);
			return history.push(`/municipalities/${currentUser.municipalityId}/inciports/create`);
		};

		return (
			<div className='SubHeaderMemoContainer'>
				<button className='btn btn-primary CreateButton' onClick={onCreateClick}>
					Create
				</button>
				<FilterComponent onFilter={(e) => setFilterText(e.target.value)} onClear={handleClear} filterText={filterText} />
			</div>
		);
	}, [filterText, resetPaginationToggle]);

	return (
		<Dashboard
			title={''}
			data={incidentReports}
			filteredItems={filteredItems}
			subHeaderComponentMemo={subHeaderComponentMemo}
			resetPaginationToggle={resetPaginationToggle}
			columns={columns}
			onRowDoubleClicked={onEditClick}
		/>
	);
};

export default IncidentReportDashboardComponent;
