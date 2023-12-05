import { FC } from 'react';
import { Modal } from 'react-bootstrap';

interface Props {
	show: boolean;
}

export const PopupComponent: FC<Props> = (props) => {
	return (
		<Modal id='ModalID' show={props.show} animation={false}>
			{props.children}
		</Modal>
	);
};

export default PopupComponent;
