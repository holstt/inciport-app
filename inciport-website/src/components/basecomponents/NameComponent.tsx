import { FC, useState } from 'react';

interface Props {
	name: string;
	nameOfWhat?: string;
	handleOnChange: (event) => void;
}

export const NameComponent: FC<Props> = (props) => {
	const [nameOfWhat] = useState<string>(() => {
		return props.nameOfWhat === undefined ? 'Name' : props.nameOfWhat;
	});

	return (
		<div id='NameContainer' className='ContactInformationElementContainer'>
			<label className='ContactElementLabel'>{nameOfWhat}:</label>
			<input className='form-control InputField' onChange={props.handleOnChange} defaultValue={props.name}></input>
		</div>
	);
};

export default NameComponent;
