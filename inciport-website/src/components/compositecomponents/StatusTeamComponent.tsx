/* eslint-disable import/first */
import { FC } from 'react';
import { StatusComponent } from '../basecomponents/StatusComponent';
import { TeamComponent } from '../basecomponents/TeamComponent';
import '../../css/StatusTeamStyles.css';

export const StatusTeamComponent: FC = () => {
	return (
		<div className='StatusTeamContainer'>
			<StatusComponent />
			<TeamComponent />
		</div>
	);
};

export default StatusTeamComponent;
