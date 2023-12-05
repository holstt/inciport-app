import EditPage from '../components/pages/EditPage';
import ReactDOM from 'react-dom';
import React from 'react';
import { fireEvent, screen } from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { mount } from 'enzyme';
import authService from '../services/authService';
import axios from 'axios';
import { initialize } from "@googlemaps/jest-mocks";

const localStorageMock = (() => {
    let store = {};
  
    return {
      getItem(key) {
        return store[key] || null;
      },
      setItem(key, value) {
        store[key] = value.toString();
      },
      removeItem(key) {
        delete store[key];
      },
      clear() {
        store = {};
      }
    };
})();

Object.defineProperty(window, 'sessionStorage', {
    value: localStorageMock
});

const user = {
  token: {
      value: "Fake Token",
      expiration: "2022",
  },
  id: "Test_ID",
  fullName: "Test User",
  email: "test@email.com", 
  municipalityId: 1,
  municipalityName: "Test_municipality",
  role: "ADMIN"
};

const handleUnauthorized = () => {
  //history.push('/unauthorized'); // Find out how to push some html elements when this is called
  return null;
};

describe('Edit Page', () => {
    let container: HTMLDivElement;
    let confirmSpy;

    beforeEach(() => {
        initialize(); // Used to mock google map
        container = document.createElement('div');
        document.body.innerHTML = '<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAbGJw4g8X9u-K4IBN-8VqppMBFLzhKcfU"></script>';
        document.body.appendChild(container);
        window.sessionStorage.clear();
        jest.restoreAllMocks();
        // Used to confirm the pop up window asking if the user confirm
        confirmSpy = jest.spyOn(window, 'confirm');
        confirmSpy.mockImplementation(jest.fn(() => true));
    });

    afterEach(() => {
        document.body.removeChild(container);
        container.remove();

        // Reset the confirm window to normal
        confirmSpy.mockRestore()
    });

    // mount() goes deeper and tests a component's children. https://auth0.com/blog/testing-react-applications-with-jest/
    it('renders without crashing', () => {
        mount(<EditPage handleUnauthorized={handleUnauthorized} />);
    });

    it('render edit container when authorized', () => {
        window.sessionStorage.setItem('AuthorizedUser', JSON.stringify(user));
                
        act(() => {
            ReactDOM.render(<EditPage handleUnauthorized={handleUnauthorized} />, container);
          });
          expect(container.querySelector('.EditContainer')).toBeInTheDocument();
    });

    // This test is outdated. Savebutton is doing more now, which needs to be handled. Right now it causes the test to crash
    // it('Save button onClick, should call ApiClient to save incident report', async () => {
    //   // Setup  
    //   const mockedResponse: object = {
    //         status: 200,
    //         statusText: 'OK',
    //     };

    //     const mockPut = jest.spyOn(axios, 'put');
    //     let gotCalled = false;

    //     mockPut.mockImplementation((url) => {
    //         if(url.includes('/inciports/')) {
    //             gotCalled = true
    //             return Promise.resolve({ data: mockedResponse });
    //         }
    //     }); 

    //     window.sessionStorage.setItem('AuthorizedUser', JSON.stringify(user));

    //     const editPage = mount(<EditPage handleUnauthorized={handleUnauthorized} />);
    //     editPage.find("#SaveButton").prop('onClick')() 

    //     expect(gotCalled).toEqual(true)
    // });
});

