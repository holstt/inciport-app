import { FC, useCallback } from 'react';
import '../../css/TabTitleStyles.css';

interface Props {
	title: string;
	index: number;
	setCurrentTab: (index: number) => void;
	isActive: boolean
}

export const TabTitleComponent: FC<Props> = ({title, index, setCurrentTab, isActive}) => {
	const handleOnClick = useCallback(() => {
		setCurrentTab(index);
	}, [setCurrentTab, index]);

	return (
		<li className='nav-item'>
			<a className={isActive ? 'nav-link active tab' : 'nav-link tab'} aria-current='page' onClick={handleOnClick}>{title}</a>
		</li>
	);
};

export default TabTitleComponent;