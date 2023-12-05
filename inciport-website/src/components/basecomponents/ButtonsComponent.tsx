import { FC } from 'react';
import DeleteIcon from '@mui/icons-material/Delete';
import SaveIcon from '@mui/icons-material/Save';
import '../../css/ButtonsStyles.css';

interface Props {
	onSave: () => void;
	onCancel: () => void;
	onDelete: () => void;
	disabledCondition: boolean;
}

export const ButtonsComponent: FC<Props> = (props) => {
	return (
		<div className='ButtonsContainer'>
			<button
				className='btn btn-primary EditPageButtons IconTextButton'
				id='SaveButton'
				type='button'
				onClick={props.onSave}
				disabled={props.disabledCondition}
			>
				<SaveIcon fontSize='small' />
				<text className='TextIconTextButton'>Save changes</text>
			</button>
			<button className='btn btn-secondary EditPageButtons' id='CancelButton' type='button' onClick={props.onCancel}>
				Cancel
			</button>
			<button className='btn btn-danger EditPageButtons IconTextButton' id='DeleteButton' type='button' onClick={props.onDelete}>
				<DeleteIcon fontSize='small' />
				<text className='TextIconTextButton'>Delete</text>
			</button>
		</div>
	);
};

export default ButtonsComponent;
