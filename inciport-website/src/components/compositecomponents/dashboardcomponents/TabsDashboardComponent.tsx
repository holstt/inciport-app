import { FC, useContext } from 'react';
import { Context } from '../../../Context';
import TabComponent from '../../basecomponents/TabComponent';
import TabsComponent from '../../basecomponents/TabsComponent';
import CategoryDashboardComponent from './CategoryDashboardComponent';
import IncidentReportDashboardComponent from './IncidentReportDashboardComponent';
import TeamDashboardComponent from './TeamDashboardComponent';
import UserDashboardComponent from './UserDashboardComponent';

export const TabsDashboardComponent: FC = () => {
	const { currentTab, updateCurrentTab } = useContext(Context);

	return (
		<div>
			<TabsComponent currentTab={currentTab} updateCurrentTab={updateCurrentTab}>
				<TabComponent title='Users'>
					<UserDashboardComponent />
				</TabComponent>
				<TabComponent title='Workers'>
					<TeamDashboardComponent />
				</TabComponent>
				<TabComponent title='Categories'>
					<CategoryDashboardComponent />
				</TabComponent>
				<TabComponent title='Reports'>
					<IncidentReportDashboardComponent />
				</TabComponent>
			</TabsComponent>
		</div>
	);
};

export default TabsDashboardComponent;
