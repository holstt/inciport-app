import { FC, useState, useEffect, useContext } from 'react';
import WorkerTeam from '../../models/WorkerTeam';
import { APIClient } from '../../services/ApiClient';
import { Context } from '../../Context';
import { useHistory } from 'react-router-dom';
import '../../css/TeamStyles.css';

const defaultWorkerTeam = new WorkerTeam(0, '');

export const TeamComponent: FC = () => {
	const history = useHistory();
	const { incidentReport, updateCurrentIncidentReport, updateIsIncidentReportUpdated } = useContext(Context);
	const [assignedTeam] = useState(() => {
		return incidentReport.assignedTeam == null ? defaultWorkerTeam : incidentReport.assignedTeam;
	});
	const [options, setOptions] = useState<WorkerTeam[]>();
	const [match, setMatch] = useState<boolean>(false);

	const handleOnChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
		updateIsIncidentReportUpdated(true);
		const newTeam: WorkerTeam | undefined = options?.find((x) => x.name === event.target.value);
		if (newTeam !== undefined) {
			incidentReport.assignedTeam = newTeam;
		}
		updateCurrentIncidentReport(incidentReport);
	};

	useEffect(() => {
		const getWorkerTeams = async () => {
			try {
				const response = await APIClient.getWorkerTeams();
				setOptions(response.data);
				for (let i = 0; i < response.data.length; i++) {
					if (response.data[i].id === assignedTeam.id) {
						setMatch(true);
					}
				}
			} catch (error: any) {
				const errorMessage = 'Worker teams (GET) - ' + error.toString();
				console.log(errorMessage);
				history.push('/error', errorMessage);
			}
		};
		getWorkerTeams();
	}, []);

	return (
		<div className='TeamContainer'>
			<h4 className='OverviewMessage'>Team:</h4>
			<select className='form-control DropDown' onChange={handleOnChange}>
				{options?.map((item) => {
					if (item.name === assignedTeam.name) {
						return (
							<option selected={true} value={item.name}>
								{item.name}
							</option>
						);
					}
					return <option value={item.name}>{item.name}</option>;
				})}
				{match ? '' : <option selected={true} value=''></option>}
			</select>
		</div>
	);
};

export default TeamComponent;
