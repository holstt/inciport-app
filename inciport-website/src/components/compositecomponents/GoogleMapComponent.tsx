import { FC, useContext, useState } from 'react';
import { Context } from '../../Context';
import MapComponent from '../basecomponents/MapComponent';
import MarkerComponent from '../basecomponents/MarkerComponent';
import IncidentReport from '../../models/IncidentReport';
import Location from '../../models/Location';
import Address from '../../models/Address';
import '../../css/GoogleMapStyles.css';

export const GoogleMapComponent: FC = () => {
	const { incidentReport, updateCurrentIncidentReport, updateIsIncidentReportUpdated } = useContext(Context);
	const [clickLatLng, setClickLatLng] = useState<google.maps.LatLng>(() => {
		return incidentReport.location.latitude
			? new google.maps.LatLng(incidentReport.location.latitude, incidentReport.location.longitude)
			: new google.maps.LatLng(0, 0);
	});
	const [zoom] = useState(10);
	const [center] = useState<google.maps.LatLngLiteral>({
		lat: 57.046707,
		lng: 9.935932,
	});
	const [address, setAddress] = useState<Address>(() => {
		return new Address(incidentReport.location.address.street, incidentReport.location.address.city,incidentReport.location.address.zipCode, incidentReport.location.address.country, incidentReport.location.address.municipality);
	});

	const onClick = async (event: google.maps.MapMouseEvent) => {
		updateIsIncidentReportUpdated(true);
		let newAddress: Address | null = await coordinateToAddress(event.latLng!);
		if (newAddress === null) {
			newAddress = new Address('', '', '', '', '');
		}
		setAddress(newAddress);
		const newIncidentReport = new IncidentReport(
			incidentReport.id,
			incidentReport.status,
			incidentReport.chosenMainCategory,
			new Location(event.latLng?.lng()!, event.latLng?.lat()!, newAddress),
			incidentReport.description,
			incidentReport.contactInformation,
			incidentReport.imageUrls,
			incidentReport.timestampCreatedUtc,
			incidentReport.timestampModifiedUtc,
			incidentReport.assignedTeam
		);
		updateCurrentIncidentReport(newIncidentReport);
		setClickLatLng(event.latLng!);
	};

	const coordinateToAddress = async (coordinate: google.maps.LatLng) => {
		let newAddress: Address | null = null;
		const geoCoder = new google.maps.Geocoder();
		await geoCoder.geocode({ location: coordinate }, (results, status) => {
			if (status === 'OK') {
				if (results) {
					const municipality = results.find((x) => x.types.includes('administrative_area_level_2'))?.address_components[0]
						.long_name;
					const country = results.find((x) => x.types.includes('country'))?.address_components[0].long_name;
					const zipCode = results.find((x) => x.types.includes('postal_code'))?.address_components[0].long_name;
					const city = results.find((x) => x.types.includes('postal_town'))?.address_components[0].long_name;
					let street = results
						.find((x) => x.types.includes('street_address'))
						?.formatted_address.toString()
						.split(',')[0];
					if (street === undefined) {
						street = results
							.find((x) => x.types.includes('premise'))
							?.address_components.find((x) => x.types.includes('route'))?.long_name;
					}
					if (street === undefined) {
						street = '';
					}
					newAddress = new Address(street!, city!, zipCode!, country!, municipality!);
				}
			}
		});
		return newAddress;
	};

	return (
		<div className='MapContainer'>
			<div className='MapDiv'>
				<h4 className='InlineText OverviewMessage'>Map:</h4>
				<text className='MapText'>
					{address.toString()}
				</text>
				<text className='MapText'>
					{incidentReport.location.latitude}, {incidentReport.location.longitude}
				</text>
			</div>
			<MapComponent center={center} onClick={onClick} zoom={zoom}>
				<MarkerComponent position={clickLatLng} />
			</MapComponent>
		</div>
	);
};

export default GoogleMapComponent;
