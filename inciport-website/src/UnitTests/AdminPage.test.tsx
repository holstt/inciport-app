import ReactDOM from 'react-dom';
import React from 'react';
import { fireEvent, screen } from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { mount } from 'enzyme';
import AdminPage from '../components/pages/AdminPage';
import { Router } from 'react-router-dom';
import { useHistory } from 'react-router-dom';

describe('Admin Page', () => {
    let container: HTMLDivElement;
    //const history = useHistory();

    const handleUnauthorized = () => {
        //history.push('/unauthorized');
        return null;
    };
    beforeEach(() => {
        container = document.createElement('div');
        document.body.appendChild(container);
        
    });

    afterEach(() => {
        document.body.removeChild(container);
        container.remove();
    });

    // mount() goes deeper and tests a component's children. https://auth0.com/blog/testing-react-applications-with-jest/
    it('renders without crashing', () => {
        const historyMock = { push: jest.fn(), location: {}, listen: jest.fn() };

        mount(<Router history={historyMock}><AdminPage handleUnauthorized={handleUnauthorized}/></Router>);
    });

})