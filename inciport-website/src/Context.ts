import { createContext } from 'react';
import Address from './models/Address';
import ContactInformation from './models/ContactInformation';
import IncidentReport from './models/IncidentReport';
import Location from './models/Location';
import ChosenMainCategory from './models/ChosenMainCategory';
import MainCategoryOptions from './models/MainCategoryOptions';
import WorkerTeam from './models/WorkerTeam';
import User from './models/User';
import Category from './models/Category';
import AuthorizedUser from './models/AuthorizedUser';
import Token from './models/Token';

export interface ContextState {
	incidentReport: IncidentReport;
	updateCurrentIncidentReport: (incidentReport: IncidentReport) => void;
	isIncidentReportUpdated: Boolean;
	updateIsIncidentReportUpdated: (newBoolean: Boolean) => void;
	user: User;
	updateSelectedUser: (user: User) => void;
	team: WorkerTeam;
	updateSelectedWorkerTeam: (team: WorkerTeam) => void;
	mainCategory: MainCategoryOptions;
	updateSelectedMainCategory: (category: MainCategoryOptions) => void;
	isMainCategoryUpdated: Boolean;
	updateIsMainCategoryUpdated: (newBoolean: Boolean) => void;
	subCategory: Category;
	updateSelectedSubCategory: (category: Category) => void;
	currentUser: AuthorizedUser;
	updateCurrentUser: (user: AuthorizedUser) => void;
	isMunicipalityUpdated: Boolean;
	updateIsMunicipalityUpdated: (newBoolean: Boolean) => void;
	currentTab: number;
	updateCurrentTab: (index: number) => void;
}

export const defaultInciport = new IncidentReport(
	0,
	'',
	new ChosenMainCategory(0, '', null),
	new Location(0, 0, new Address('', '', '', '', '')),
	'',
	new ContactInformation('', '', ''),
	[],
	new Date(),
	new Date(),
	new WorkerTeam(0, '')
);
export const defaultUser = new User(0, '', '', 0, '', '');
export const defaultTeam = new WorkerTeam(0, '');
export const defaultMainCategory = new MainCategoryOptions(0, '', []);
export const defaultSubCategory = new Category(0, '');
export const defaultCurrentUser = new AuthorizedUser(0, '', '', 0, '', '', new Token('', ''));

export const contextDefaultValues: ContextState = {
	incidentReport: defaultInciport,
	updateCurrentIncidentReport: () => {},
	isIncidentReportUpdated: false,
	updateIsIncidentReportUpdated: () => {},
	user: defaultUser,
	updateSelectedUser: () => {},
	team: defaultTeam,
	updateSelectedWorkerTeam: () => {},
	mainCategory: defaultMainCategory,
	updateSelectedMainCategory: () => {},
	isMainCategoryUpdated: false,
	updateIsMainCategoryUpdated: () => {},
	subCategory: defaultSubCategory,
	updateSelectedSubCategory: () => {},
	currentUser: defaultCurrentUser,
	updateCurrentUser: () => {},
	isMunicipalityUpdated: false,
	updateIsMunicipalityUpdated: () => {},
	currentTab: 0,
	updateCurrentTab: () => {}
};

export const Context = createContext<ContextState>(contextDefaultValues);
