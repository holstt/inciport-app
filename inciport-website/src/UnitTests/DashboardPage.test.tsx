import ReactDOM from 'react-dom';
import React from 'react';
import { fireEvent, screen } from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { mount } from 'enzyme';
import DashboardPage from '../components/pages/DashboardPage';


describe('', () => {
    let container: HTMLDivElement;

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
        mount(<DashboardPage handleUnauthorized={handleUnauthorized}/>);
    });

})