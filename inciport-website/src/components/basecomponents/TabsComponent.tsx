import { FC, ReactElement } from 'react';
import TabTitleComponent from './TabTitleComponent';

interface Props {
	currentTab: number;
	updateCurrentTab: (index: number) => void;
	children: ReactElement[];
}

export const TabsComponent: FC<Props> = (props) => {
	const isActive = (index: number) => {
		return props.currentTab === index;
	};
	
	return (
		<div>
			<ul className='nav nav-tabs TabsUL'>
				{props.children.map((item, index) => (
					<TabTitleComponent
						title={item.props.title}
						key={index}
						index={index}
						setCurrentTab={props.updateCurrentTab}
						isActive={isActive(index)}
					/>
				))}
			</ul>
			{props.children[props.currentTab]}
		</div>
	);
};

export default TabsComponent;
