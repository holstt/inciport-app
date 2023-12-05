import FilterComponent from '../components/basecomponents/FilterComponent';
import { render, screen } from '@testing-library/react';
import { shallow } from 'enzyme';
import { expect } from 'chai'
import sinon from 'sinon';


describe('FilterComponent', () => {
    describe('At Rendering', () => {
        it('Renders an input field', () => {
            let text: string = '';
            const mockCallBack = jest.fn();
            
            const wrapper = shallow(<FilterComponent onFilter={e => text = e.target.value} onClear={mockCallBack} filterText={text} />);
            expect(wrapper.find('#search')).to.have.lengthOf(1);
        })

        it('Renders a Clear button', () => {
            let text: string = '';
            const mockCallBack = jest.fn();
            render(<FilterComponent onFilter={e => text = e.target.value} onClear={mockCallBack} filterText={text} />);
            
            const wrapper = shallow(<FilterComponent onFilter={e => text = e.target.value} onClear={mockCallBack} filterText={text} />);
            expect(wrapper.find('.ClearButton')).to.have.lengthOf(1);
        })
    })

    it('simulates click events', () => {
        let text: string = '';            
        const onButtonClick = sinon.spy();
        const wrapper = shallow(<FilterComponent onFilter={e => text = e.target.value} onClear={onButtonClick} filterText={text} />);
        wrapper.find('button').simulate('click');
        expect(onButtonClick).to.have.property('callCount', 1);
      });

      it('Simulates the Change of Value in input', () => {
        let text: string = '';            
        const onButtonClick = sinon.spy();
        const wrapper = shallow(<FilterComponent onFilter={e => text = e.target.value} onClear={onButtonClick} filterText={text} />);
        wrapper.find('#search').simulate('change', { target: { value: 'Test Value' } })
        expect(text).to.equal('Test Value');
    })
})