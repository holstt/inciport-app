import { FC } from 'react';

interface Props {
	email: string;
	handleOnChange: (event) => void;
}

export const EmailComponent: FC<Props> = (props) => {
	return (
		<div id='EmailContainer' className='ContactInformationElementContainer'>
			<label className='ContactElementLabel'>Email:</label>
			<input name='email' className='form-control InputField' onChange={props.handleOnChange} defaultValue={props.email}></input>
		</div>
	);
};

export default EmailComponent;
