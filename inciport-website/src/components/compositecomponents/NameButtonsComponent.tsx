import { FC } from 'react';
import ButtonsComponent from '../basecomponents/ButtonsComponent';
import NameComponent from '../basecomponents/NameComponent';
import '../../css/NameButtonsStyles.css';

interface Props {
	name: string;
	nameOfWhat: string;
	handleOnChange: (event) => void;
	onSave: () => void;
	onCancel: () => void;
	onDelete: () => void;
	disabledCondition: boolean;
}

export const NameButtonsComponent: FC<Props> = (props) => {
	return (
		<div className='NameButtonsContainer'>
			<div className='NameButtonsNameContainer'>
				<NameComponent name={props.name} nameOfWhat={props.nameOfWhat} handleOnChange={props.handleOnChange} />
			</div>
			<ButtonsComponent
				onSave={props.onSave}
				onCancel={props.onCancel}
				onDelete={props.onDelete}
				disabledCondition={props.disabledCondition}
			/>
		</div>
	);
};

export default NameButtonsComponent;
