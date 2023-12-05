import { FC } from 'react';
import DeleteIcon from '@mui/icons-material/Delete';
import SaveIcon from '@mui/icons-material/Save';

interface Props {
	handleDelete: () => void;
}

export const PopupEditSubmitComponent: FC<Props> = ({ handleDelete }) => {
    return (
        <div className='form-group'>
            <button type='submit' className='btn btn-primary DeleteSaveButton IconTextButton'>
                <SaveIcon fontSize='small' />
                <text className='TextIconTextButton'>Save</text>
            </button>
            <button className='btn btn-danger DeleteSaveButton IconTextButton' id='DeleteButton' type='button' onClick={handleDelete}>
                <DeleteIcon fontSize='small' />
                <text className='TextIconTextButton'>Delete</text>
            </button>
        </div>
    );
};

export default PopupEditSubmitComponent;