import { useState, FC, useEffect } from 'react';
import IncidentReport from './models/IncidentReport';
import { Context, contextDefaultValues } from './Context';
import User from './models/User';
import WorkerTeam from './models/WorkerTeam';
import MainCategoryOptions from './models/MainCategoryOptions';
import Category from './models/Category';
import AuthorizedUser from './models/AuthorizedUser';

const ContextProvider: FC = ({ children }) => {
	const [incidentReport, setIncidentReport] = useState<IncidentReport>(contextDefaultValues.incidentReport);
	const [isIncidentReportUpdated, setIsIncidentReportUpdated] = useState<Boolean>(
		contextDefaultValues.isIncidentReportUpdated
	);
	const [user, setUser] = useState<User>(contextDefaultValues.user);
	const [team, setTeam] = useState<WorkerTeam>(contextDefaultValues.team);
	const [mainCategory, setMainCategory] = useState<MainCategoryOptions>(contextDefaultValues.mainCategory);
	const [isCategoryUpdated, setIsCategoryUpdated] = useState<Boolean>(contextDefaultValues.isMainCategoryUpdated);
	const [subCategory, setSubCategory] = useState<Category>(contextDefaultValues.subCategory);
	const [currentUser, setCurrentUser] = useState<AuthorizedUser>(contextDefaultValues.currentUser);
	const [isMunicipalityUpdated, setIsMunicipalityUpdated] = useState<Boolean>(contextDefaultValues.isMunicipalityUpdated);
	const [currentTab, setCurrentTab] = useState<number>(contextDefaultValues.currentTab);

	const updateCurrentIncidentReport = (newIncidentReport: IncidentReport) => setIncidentReport(newIncidentReport);

	const updateIsIncidentReportUpdated = (newBoolean: Boolean) => setIsIncidentReportUpdated(newBoolean);

	const updateSelectedUser = (newUser: User) => setUser(newUser);

	const updateSelectedWorkerTeam = (newTeam: WorkerTeam) => setTeam(newTeam);

	const updateSelectedMainCategory = (newMainCategory: MainCategoryOptions) => setMainCategory(newMainCategory);

	const updateIsCategoryUpdated = (newBoolean: Boolean) => setIsCategoryUpdated(newBoolean);

	const updateSelectedSubCategory = (newSubCategory: Category) => setSubCategory(newSubCategory);

	const updateCurrentUser = (newCurrentUser: AuthorizedUser) => setCurrentUser(newCurrentUser);

	const updateIsMunicipalityUpdated = (newBoolean: Boolean) => setIsMunicipalityUpdated(newBoolean);

	const updateCurrentTab = (index: number) => setCurrentTab(index);

	return (
		<Context.Provider
			value={{
				incidentReport,
				updateCurrentIncidentReport: updateCurrentIncidentReport,
				isIncidentReportUpdated,
				updateIsIncidentReportUpdated: updateIsIncidentReportUpdated,
				user,
				updateSelectedUser: updateSelectedUser,
				team,
				updateSelectedWorkerTeam: updateSelectedWorkerTeam,
				mainCategory: mainCategory,
				updateSelectedMainCategory: updateSelectedMainCategory,
				isMainCategoryUpdated: isCategoryUpdated,
				updateIsMainCategoryUpdated: updateIsCategoryUpdated,
				subCategory: subCategory,
				updateSelectedSubCategory: updateSelectedSubCategory,
				currentUser: currentUser,
				updateCurrentUser: updateCurrentUser,
				isMunicipalityUpdated: isMunicipalityUpdated,
				updateIsMunicipalityUpdated: updateIsMunicipalityUpdated,
				currentTab,
				updateCurrentTab: updateCurrentTab,
			}}
		>
			{children}
		</Context.Provider>
	);
};

export default ContextProvider;
