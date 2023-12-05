import { FC } from 'react';
import '../../css/FilterStyles.css';

interface Props {
	filterText: string;
	onFilter: (e) => void;
	onClear: () => void;
}

export const FilterComponent: FC<Props> = ({ filterText, onFilter, onClear }) => (
	<div className='FilterComponentContainer'>
		<input
			id='search'
			className='FilterTextClass'
			type='text'
			placeholder='Filter By Description'
			aria-label='Search Input'
			value={filterText}
			onChange={onFilter}
		/>
		<button className='btn btn-secondary ClearButton' type='button' onClick={onClear}>
			Clear
		</button>
	</div>
);

export default FilterComponent;
