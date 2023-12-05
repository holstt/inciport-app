import { FC } from 'react';
import DataTable from 'react-data-table-component';
import '../../../css/DashboardStyles.css';

interface Props {
	title: string;
	data: any[];
	filteredItems: any[];
	subHeaderComponentMemo: JSX.Element;
	resetPaginationToggle: boolean;
	columns: (
		| {
				name: string;
				selector: (row: any) => any;
				sortable: boolean;
				reorder: boolean;
				grow: number;
				cell?: undefined;
				button?: undefined;
		  }
		| {
				name: string;
				cell: (row: any) => JSX.Element;
				button: boolean;
				selector?: undefined;
				sortable?: undefined;
				reorder?: undefined;
				grow?: undefined;
		  }
	)[];
	onRowDoubleClicked: (row: any, event: any) => void;
}

export const Dashboard: FC<Props> = (props) => {
	return (
		<div>
			<DataTable
				title={props.title}
				className='DataTableClass'
				columns={props.columns}
				data={props.filteredItems}
				pagination
				paginationResetDefaultPage={props.resetPaginationToggle}
				subHeader
				subHeaderComponent={props.subHeaderComponentMemo}
				persistTableHead
				onRowDoubleClicked={props.onRowDoubleClicked}
				pointerOnHover={true}
				highlightOnHover={true}
				striped={true}
			/>
		</div>
	);
};

export default Dashboard;
