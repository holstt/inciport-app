import { FC, useEffect, useState } from 'react';

export const MarkerComponent: FC<google.maps.MarkerOptions> = (options) => {
	const [marker, setMarker] = useState<google.maps.Marker>();

	useEffect(() => {
		if (!marker) {
			setMarker(new google.maps.Marker({ position: options.position, map: options.map }));
		}
	}, [marker, options]);

	useEffect(() => {
		if (marker) {
			marker.setOptions(options);
		}
	}, [marker, options]);

	return null;
};

export default MarkerComponent;
