import { FC, useState } from 'react';
import authService from '../../services/authService';
import MunicipalityDashboardComponent from '../compositecomponents/dashboardcomponents/MunicipalityDashboardComponent';

interface Props {
    handleUnauthorized: () => null;
}

export const MaintainerPage: FC<Props> = (props) => {
    const [isAuthorized] = useState(authService.getCurrentUser() != null ? true : false);
    
    return (
        <div>
            {isAuthorized ? <MunicipalityDashboardComponent /> : props.handleUnauthorized()}
        </div>
        
    );
};

export default MaintainerPage;
