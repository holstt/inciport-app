import { FC } from 'react';

interface Props {
	title: string;
}

export const TabComponent: FC<Props> = (Props) => {
	return <div>{Props.children}</div>;
};

export default TabComponent;
