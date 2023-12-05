import { FC, useContext } from 'react';
import DescriptionComponent from '../basecomponents/DescriptionComponent';
import { StatusTeamComponent } from './StatusTeamComponent';
import '../../css/StatusTeamDescriptionStyles.css';
import { Context } from '../../Context';

export const StatusTeamDescriptionComponent: FC = () => {
	const { incidentReport, updateCurrentIncidentReport, updateIsIncidentReportUpdated } = useContext(Context);

	const handleOnChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
		updateIsIncidentReportUpdated(true);
		updateCurrentIncidentReport({
			...incidentReport,
			description: event.target.value,
		});
	};
	
	return (
		<div className='StatusTeamDescriptionContainer'>
			<StatusTeamComponent />
			<DescriptionComponent description={incidentReport.description} handleOnChange={handleOnChange}/>
		</div>
	);
};

export default StatusTeamDescriptionComponent;
