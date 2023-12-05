import { FC } from 'react';
import { useHistory } from 'react-router-dom';

export const ErrorPage: FC = () => {
    const history = useHistory();
    const error = history.location.state as string;
    const errorOccuredMessage = 'An error occured.\nPlease try again or contact the administrator if the problem persists';
    console.log('From error page');
    console.log(error);

    return (
        <div>
            <h1>{errorOccuredMessage}</h1>
            <h3>{error}</h3>
        </div>
    );
}

export default ErrorPage;
