import { FC, useContext, useState } from 'react';
import { Context } from '../../Context';
import '../../css/TimestampStyles.css';

export const TimestampComponent: FC = () => {
	const { incidentReport } = useContext(Context);
	const [createdAt] = useState(new Date(incidentReport.timestampCreatedUtc));
	const [lastModified] = useState(new Date(incidentReport.timestampModifiedUtc));

	return (
		<div className='TimeStampContainer'>
			<h4 className='OverviewMessage TimeStampMessage'>Timestamps:</h4>
			<div className='Grid'>
				<text className='text-start TimeStamp'><b>Created at:</b><br/> {createdAt.toLocaleString()}</text>
				<text className='text-start TimeStamp'><b>Last modified:</b><br/> {lastModified.toLocaleString()}</text>
			</div>
		</div>
	);
};

export default TimestampComponent;
