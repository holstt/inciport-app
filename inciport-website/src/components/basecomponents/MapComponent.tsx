import { FC, useRef, useState, useEffect, Children, cloneElement, isValidElement } from 'react';

interface Props extends google.maps.MapOptions {
	onClick?: (event: google.maps.MapMouseEvent) => void;
}

export const MapComponent: FC<Props> = (props) => {
	const ref = useRef<HTMLDivElement>(null);
	const [map, setMap] = useState<google.maps.Map>();

	useEffect(() => {
		if (ref.current && !map) {
			setMap(
				new window.google.maps.Map(ref.current, {
					center: props.center,
					zoom: props.zoom,
				})
			);
		}
	}, [ref, map]);

	useEffect(() => {
		if (map) {
			google.maps.event.clearListeners(map, 'click');
			if (props.onClick) {
				map.addListener('click', props.onClick);
			}
		}
	}, [map, props.onClick]);

	return (
		<div>
			<div ref={ref} className='MapContainerClass' />
			{Children.map(props.children, (child) => {
				if (isValidElement(child)) {
					return cloneElement(child, { map });
				}
			})}
		</div>
	);
};

export default MapComponent;
