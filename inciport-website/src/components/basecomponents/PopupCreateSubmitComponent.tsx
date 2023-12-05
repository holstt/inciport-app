import { FC } from 'react';
import SaveIcon from '@mui/icons-material/Save';

export const PopupCreateSubmitComponent: FC = () => {
    return (
        <div className='form-group'>
            <button type='submit' className='btn btn-primary DeleteSaveButton IconTextButton'>
                <SaveIcon fontSize='small' />
                <text className='TextIconTextButton'>Save</text>
            </button>
        </div>
    );
}

export default PopupCreateSubmitComponent;