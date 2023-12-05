import { FC } from 'react';
import PicturesComponent from '../compositecomponents/PicturesComponent';
import StatusTeamDescriptionCategoryTimestampContactInformationComponent from './StatusTeamDescriptionCategoryTimestampContactInformationComponent';
import '../../css/StatusTeamDescriptionCategoryTimestampContactInformationPicturesStyles.css';

export const StatusTeamDescriptionCategoryTimestampContactInformationPicturesComponent: FC = () => {
	return (
		<div className='StatusTeamDescriptionCategoryTimestampContactInformationPicturesContainer'>
			<StatusTeamDescriptionCategoryTimestampContactInformationComponent />
			<PicturesComponent />
		</div>
	);
};

export default StatusTeamDescriptionCategoryTimestampContactInformationPicturesComponent;
