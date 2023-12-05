import { FC, useState } from 'react';
import authService from '../../services/authService';
import IncidentReportDashboardComponent from '../compositecomponents/dashboardcomponents/IncidentReportDashboardComponent';

interface Props {
	handleUnauthorized: () => null;
}

export const DashboardPage: FC<Props> = (props) => {
	const [isAuthorized] = useState(() => {
		return authService.getCurrentUser() != null ? true : false;
	});

	return <div>{isAuthorized ? <IncidentReportDashboardComponent /> : props.handleUnauthorized()}</div>;
};

export default DashboardPage;
