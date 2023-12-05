import { FC, useEffect, useState } from 'react';
import { Link, useHistory } from 'react-router-dom';
import AuthorizedUser from '../../models/AuthorizedUser';
import authService from '../../services/authService';
import logo from '../../assets/logo.png';
import '../../css/NavBarStyles.css';

export const NavBarComponent: FC = () => {
	const [currentUser, setCurrentUser] = useState<AuthorizedUser | null>(authService.getCurrentUser());
	const [logoLink, setLogoLink] = useState<string>('/login');
	const history = useHistory();

	useEffect(() => {
		const user = authService.getCurrentUser();

		if (user) {
			setCurrentUser(user);
			if (user.role.toLowerCase().includes('manager')) {
				setLogoLink(`/municipalities/${user.municipalityId}/dashboard`);
			} else if (user.role.toLowerCase().includes('admin')) {
				setLogoLink(`/municipalities/${user.municipalityId}/admin`);
			} else if (user.role.toLowerCase().includes('maintainer')) {
				setLogoLink(`/municipalities/`);
			}
		}
	}, []);

	const logOut = () => {
		authService.logout();
		history.push('/login');
		window.location.reload();
	};

	const handleOnClick = () => {
		history.push('/login');
		window.location.reload();
	}

	return (
		<nav className='navbar navbar-expand navbar-dark bg-dark OuterNav'>
			<div className='navbar-nav'>
				<Link to={logoLink} className='navbar-brand'>
					<div className='logo-image'>
						<img
							src={logo}
							className='LogoImage'
							alt='logo'
						/>
					</div>
				</Link>
				<Link to={logoLink}>
					<a className='InciportName'>Inciport</a>
				</Link>
			</div>
			
			{/* Nav bar right */}
			{currentUser ? (
				<div className='navbar-nav ml-auto'>
					<li className='nav-item NavElement'>
						<a className='nav-link NavItem'>{currentUser.fullName}</a>
					</li>
					<li className='nav-item NavElement'>
						<a className='nav-link NavItem'>{currentUser.role[0] + currentUser.role.substring(1).toLowerCase()}</a>
					</li>
					{console.log(currentUser.municipalityName)}
					{currentUser.municipalityName !== null ? (
						<li className='nav-item NavElement'>
							<a className='nav-link NavItem'>{currentUser.municipalityName}</a>
						</li>
					) : null}
					<button className='btn btn-secondary LogButton' onClick={logOut}>
						Log out
					</button>
				</div>
			) : (
				<div className='navbar-nav ml-auto'>
					<li className='nav-item'>
						<button className='btn btn-secondary LogButton' onClick={handleOnClick}>
							Login
						</button>
					</li>
				</div>
			)}
		</nav>
	);
};

export default NavBarComponent;
