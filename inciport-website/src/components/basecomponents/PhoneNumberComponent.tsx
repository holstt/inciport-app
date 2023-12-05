import { FC } from 'react';

interface Props {
	phoneNumber: string;
	handleOnChange: (event) => void;
}

export const PhoneNumberComponent: FC<Props> = (props) => {
	return (
		<div id='PhoneNumberContainer' className='ContactInformationElementContainer'>
			<label className='ContactElementLabel'>Phone number:</label>
			<input name='inputField' className='form-control InputField' onChange={props.handleOnChange} defaultValue={props.phoneNumber}></input>
		</div>
	);
};

export default PhoneNumberComponent;
