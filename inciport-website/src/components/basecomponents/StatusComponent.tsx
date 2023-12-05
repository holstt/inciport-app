import { FC, useState, useEffect, useContext } from 'react';
import { APIClient } from '../../services/ApiClient';
import { Context } from '../../Context';
import { useHistory } from 'react-router-dom';
import '../../css/StatusStyles.css';

export const StatusComponent: FC = () => {
	const history = useHistory();
	const { incidentReport, updateCurrentIncidentReport, updateIsIncidentReportUpdated } = useContext(Context);
	const [status] = useState(() => {
		return incidentReport.status === '' ? '' : incidentReport.status;
	});
	const [options, setOptions] = useState<string[]>([]);
	const [match, setMatch] = useState<boolean>(false);

	const handleOnChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
		updateIsIncidentReportUpdated(true);
		updateCurrentIncidentReport({
			...incidentReport,
			status: event.target.value,
		});
	};

	useEffect(() => {
		const getIncidentStatusses = async () => {
			try {
				const response = await APIClient.getIncidentStatusses();
				setOptions(response.data);
				for (let i = 0; i < response.data.length; i++) {
					if (response.data[i] === status) {
						setMatch(true);
					}
				}
			} catch (error: any) {
				const errorMessage = 'Incident statusses (GET) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		};
		getIncidentStatusses();
	}, []);

	return (
		<div className='StatusContainer'>
			<h4 className='OverviewMessage'>Status:</h4>
			<select className='form-control DropDown' onChange={handleOnChange}>
				{options.map((item) => {
					if (item === incidentReport.status) {
						return (
							<option selected={true} value={item}>
								{item}
							</option>
						);
					}
					return <option value={item}>{item}</option>;
				})}
				{match ? '' : <option selected={true} value=''></option>}
			</select>
		</div>
	);
};

export default StatusComponent;
