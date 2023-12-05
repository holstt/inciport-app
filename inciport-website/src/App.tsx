import { FC } from 'react';
import { Switch, Route, useHistory } from 'react-router-dom';
import LoginPage from './components/pages/LoginPage';
import EditPage from './components/pages/EditPage';
import AdminPage from './components/pages/AdminPage';
import DashboardPage from './components/pages/DashboardPage';
import CategoryPage from './components/pages/CategoryPage';
import CreatePage from './components/pages/CreatePage';
import MaintainerPage from './components/pages/MaintainerPage';
import NavBarComponent from './components/basecomponents/NavBarComponent';
import UnauthorizedPage from './components/pages/UnauthorizedPage';
import ErrorPage from './components/pages/ErrorPage';
import 'bootstrap/dist/css/bootstrap.min.css';
import './css/styles.css';
import './css/AppStyles.css';


const App: FC = () => {
	const history = useHistory();

	const handleUnauthorized = () => {
		history.push('/unauthorized');
		return null;
	};

	return (
		<div>
			<NavBarComponent/>
			<div className='container mt-3'>
				<Switch>
					<Route exact path={['/', '/login']} component={LoginPage} />
					<Route path='/municipalities/:muniId/dashboard' component={() => <DashboardPage handleUnauthorized={handleUnauthorized}/>} />
					<Route path='/municipalities/:muniId/inciports/:id/edit' component={() => <EditPage handleUnauthorized={handleUnauthorized}/>} />
					<Route path='/municipalities/:muniId/inciports/create' component={() => <CreatePage handleUnauthorized={handleUnauthorized}/>} />
					<Route path='/municipalities/:muniId/admin' component={() => <AdminPage handleUnauthorized={handleUnauthorized} />} />
					<Route path='/municipalities/:muniId/categories/:categoryId/edit' component={() => <CategoryPage handleUnauthorized={handleUnauthorized} />} />
					<Route path='/municipalities/' component={() => <MaintainerPage handleUnauthorized={handleUnauthorized} />} />
					<Route path='/unauthorized/' component={UnauthorizedPage} />
					<Route path='/error/' component={ErrorPage} />
					<Route path='/:x' component={LoginPage} />
				</Switch>
			</div>
		</div>
	);
};

export default App;
