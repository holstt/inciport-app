import { ButtonsComponent } from '../components/basecomponents/ButtonsComponent'
import { render, screen } from '@testing-library/react';
import { shallow } from 'enzyme';


describe('ButtonsComponent', () => {
    describe('At Rendering', () => {
        it('A Save Changes button should be rendered', () => {
            // render(<ButtonsComponent />);
            // const button = screen.getByText(/Save Changes/i);
            // expect(button).toBeInTheDocument();
        })

        it('A Cancel button should be rendered', () => {
            // render(<ButtonsComponent />);
            // const button = screen.getByText(/Cancel/i);
            // expect(button).toBeInTheDocument();
        })

        it('A Delete button should should be rendered', () => {
            // render(<ButtonsComponent />);
            // const button = screen.getByText(/Delete/i);
            // expect(button).toBeInTheDocument();
        })
    })
})