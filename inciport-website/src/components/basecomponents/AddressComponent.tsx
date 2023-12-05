import { FC, useContext } from 'react';
import { Context } from '../../Context';
import Address from '../../models/Address';
import '../../css/AddressStyles.css';

export const AddressComponent: FC = () => {
	const { incidentReport } = useContext(Context);
	const address = new Address(incidentReport.location.address.street, incidentReport.location.address.city,incidentReport.location.address.zipCode, incidentReport.location.address.country, incidentReport.location.address.municipality);

	return <h3 className='AddressContainer'>{address.toString()}</h3>;
};

export default AddressComponent;
