import { FC, useContext, useState } from 'react';
import { EmailComponent } from '../basecomponents/EmailComponent';
import { NameComponent } from '../basecomponents/NameComponent';
import { PhoneNumberComponent } from '../basecomponents/PhoneNumberComponent';
import { Context } from '../../Context';
import * as Yup from 'yup';
import '../../css/ContactInformationStyles.css';

export const ContactInformationComponent: FC = () => {
	const { incidentReport, updateCurrentIncidentReport, updateIsIncidentReportUpdated } = useContext(Context);
	const [name] = useState(() => {
		return incidentReport.contactInformation == null ? '' : incidentReport.contactInformation.name;
	});
	const [phoneNumber] = useState(() => {
		return incidentReport.contactInformation == null ? '' : incidentReport.contactInformation.phoneNumber;
	});
	const [email] = useState(() => {
		return incidentReport.contactInformation == null ? '' : incidentReport.contactInformation.email;
	});

	const handleNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		if (event.target.value.length <= 256) {
			event.target.style.border = '';
			updateIsIncidentReportUpdated(true);
			updateCurrentIncidentReport({
				...incidentReport,
				contactInformation: {
					...incidentReport.contactInformation,
					name: event.target.value,
				},
			});
		} else {
			event.target.style.border = 'solid red 2px';
		}
	};

	const handlePhoneNumberChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		if (event.target.value.length === 8) {
			event.target.style.border = '';
			updateIsIncidentReportUpdated(true);
			updateCurrentIncidentReport({
				...incidentReport,
				contactInformation: {
					...incidentReport.contactInformation,
					phoneNumber: event.target.value,
				},
			});
		} else {
			event.target.style.border = 'solid red 2px';
		}
	};

	const handleEmailChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
		console.log(Yup.string().email().isValidSync(event.target.value));
		if (
			event.target.value.length !== 0 &&
			event.target.value.length <= 256 &&
			Yup.string().email().isValidSync(event.target.value)
		) {
			event.target.style.border = '';
			updateIsIncidentReportUpdated(true);
			updateCurrentIncidentReport({
				...incidentReport,
				contactInformation: {
					...incidentReport.contactInformation,
					email: event.target.value,
				},
			});
		} else {
			event.target.style.border = 'solid red 2px';
		}
	};

	return (
		<div id='ContactInformation' className='ContactInformationContainer'>
			<h4>Contact information:</h4>
			<NameComponent name={name} handleOnChange={handleNameChange} />
			<PhoneNumberComponent phoneNumber={phoneNumber} handleOnChange={handlePhoneNumberChange} />
			<EmailComponent email={email} handleOnChange={handleEmailChange} />
		</div>
	);
};

export default ContactInformationComponent;
