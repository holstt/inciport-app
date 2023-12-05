import { FC } from 'react';
import CategoryComponent from '../basecomponents/CategoryComponent';
import TimestampComponent from '../basecomponents/TimestampComponent';
import '../../css/CategoryTimestampStyles.css';

export const CategoryTimestampComponent: FC = () => {
	return (
		<div className='CategoryTimestampContainer'>
			<CategoryComponent />
			<TimestampComponent />
		</div>
	);
};

export default CategoryTimestampComponent;
