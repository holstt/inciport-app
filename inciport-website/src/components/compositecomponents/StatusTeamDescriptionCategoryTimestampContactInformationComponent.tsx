import { FC } from 'react';
import CategoryTimestampContactInformationComponent from './CategoryTimestampContactInformationComponent';
import StatusTeamDescriptionComponent from './StatusTeamDescriptionComponent';
import '../../css/StatusTeamDescriptionCategoryTimestampContactInformationStyles.css';

export const StatusTeamDescriptionCategoryTimestampContactInformationComponent: FC = () => {
	return (
		<div className='StatusTeamDescriptionCategoryTimestampContactInformationContainer'>
			<StatusTeamDescriptionComponent />
			<CategoryTimestampContactInformationComponent />
		</div>
	);
};

export default StatusTeamDescriptionCategoryTimestampContactInformationComponent;
