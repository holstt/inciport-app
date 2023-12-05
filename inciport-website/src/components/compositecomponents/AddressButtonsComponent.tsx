import { FC } from 'react';
import AddressComponent from '../basecomponents/AddressComponent';
import ButtonsComponent from '../basecomponents/ButtonsComponent';
import '../../css/AddressButtonsStyles.css';

interface Props {
	onSave: () => void;
	onCancel: () => void;
	onDelete: () => void;
	disabledCondition: boolean;
}

export const AddressButtonsComponent: FC<Props> = (props) => {
	return (
		<div className='AddressButtonsContainer'>
			<AddressComponent />
			<ButtonsComponent
				onSave={props.onSave}
				onCancel={props.onCancel}
				onDelete={props.onDelete}
				disabledCondition={props.disabledCondition}
			/>
		</div>
	);
};

export default AddressButtonsComponent;
