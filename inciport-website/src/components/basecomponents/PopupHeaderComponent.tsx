import { FC } from 'react';
import CloseIcon from '@mui/icons-material/Close';
import '../../css/PopupHeaderStyles.css';

interface Props {
    headerMessage: string;
    handleClose: () => void;
}

export const PopupHeaderComponent: FC<Props> = ({headerMessage, handleClose}) => {
    return (
        <div className='headerContainer'>
            <label className='HeaderMessage'>{headerMessage}</label>
            <button className='btn btn-outline-secondary CloseButton' onClick={handleClose}>
                <CloseIcon fontSize='small' />
            </button>
        </div>
    );
}

export default PopupHeaderComponent;