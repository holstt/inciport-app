import { FC } from 'react';
import ContactInformationComponent from './ContactInformationComponent';
import CategoryTimestampComponent from './CategoryTimestampComponent';
import '../../css/CategoryTimestampContactInformationStyles.css';

export const CategoryTimestampContactInformationComponent: FC = () => {
	return (
		<div className='CategoryTimestampContactInformationContainer'>
			<CategoryTimestampComponent />
			<ContactInformationComponent />
		</div>
	);
};

export default CategoryTimestampContactInformationComponent;
