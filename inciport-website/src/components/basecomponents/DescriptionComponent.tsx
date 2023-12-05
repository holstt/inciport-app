import { FC } from 'react';
import '../../css/DescriptionStyles.css';

interface Props {
	description: string;
	handleOnChange: (event) => void;
}

export const DescriptionComponent: FC<Props> = (props) => {
	return (
		<div className='DescriptionContainer'>
			<h4 className='OverviewMessage'>Description:</h4>
			<textarea
				id='DescriptionTextArea'
				className='form-control DescriptionTextArea'
				defaultValue={props.description}
				onChange={props.handleOnChange}
			></textarea>
		</div>
	);
};

export default DescriptionComponent;
