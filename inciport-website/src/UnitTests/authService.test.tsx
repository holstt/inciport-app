import authService from '../services/authService';
import { render, waitFor } from "@testing-library/react";
import axios, { AxiosResponse } from 'axios';
import sinon from 'sinon';
import { APIClient } from '../services/ApiClient';

// const mockedAxios = axios as jest.Mocked<typeof axios>;
//jest.mock('axios')
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

describe('AuthService', () => {

  beforeEach(() => {
      window.sessionStorage.clear();
      jest.restoreAllMocks();
  });

  it('should get the authorized user from session storage', () => {
      window.sessionStorage.setItem('AuthorizedUser', JSON.stringify(user));
      const actualValue = authService.getCurrentUser();

      expect(actualValue).toEqual(user);
  });

  it('should get null if no user info in session storage', () => {
      const actualValue = authService.getCurrentUser();
      expect(actualValue).toEqual(null);
  });

  it('should remove the user from session storage, when logging out', () => {
      window.sessionStorage.setItem('AuthorizedUser', JSON.stringify(user));
      authService.logout();

      const actualValue = authService.getCurrentUser();
      expect(actualValue).toBeNull();
  })

  it('should set the authorize token, when calling login()', async () => {
    const mockedResponse: object = {
      data: user,
      status: 200,
      statusText: 'OK',
      headers: {},
      config: {},
    };

    // Create mock resolve and stub all post call and returns the mock resolve
    const resolved = new Promise((r) => r(mockedResponse));
    sinon.stub(axios, 'post').returns(resolved);

    // Calls the functions that makes the api call
    await authService.login("admin@inciport.rocks", "Pass_123");

    // Waits for the api call to finish, before getting the user, and testing if it is
    await waitFor(() => {
      const result = window.sessionStorage.getItem('AuthorizedUser');
      if (result) {
        expect(JSON.parse(result)).toEqual(user);
      }
    });
  });  
})