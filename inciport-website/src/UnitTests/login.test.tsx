import { LoginPage } from '../components/pages/LoginPage';
import ReactDOM from 'react-dom';
import React from 'react';
import { fireEvent, getByText } from '@testing-library/react';
import { act } from 'react-dom/test-utils';


describe('Component tests for LoginPage', () => {

    let container: HTMLDivElement;

    beforeEach(() => {
        container = document.createElement('div');
        document.body.appendChild(container);
        ReactDOM.render(<LoginPage/>, container);
    });

    afterEach(() => {
        document.body.removeChild(container);
        container.remove();
    });

    // Should add you add data-test='login-email' e.g. to the fields and then test that way?
    it('Initial render is correct', () => {
        expect(container.querySelectorAll('.form-group')).toHaveLength(3);
        expect(container.querySelector("[name='email']")).toBeInTheDocument();
        expect(container.querySelector("[name='password']")).toBeInTheDocument();
        expect(container.querySelector("[type='submit']")).toBeInTheDocument();
        expect(container.querySelector('.alert.alert-danger')).not.toBeInTheDocument();
    });

});