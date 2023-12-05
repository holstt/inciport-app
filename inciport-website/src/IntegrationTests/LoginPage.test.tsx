import React from 'react';
import { fireEvent } from '@testing-library/react';
import { render, screen, waitFor } from '@testing-library/react';
import axios from 'axios';
import LoginPage from '../components/pages/LoginPage';
import { Router } from 'react-router-dom';

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

describe('LoginPage integration tests', () => {
    it('successful login', async () => {
        //setup
        const mockedResponse: object = {
            data: user,
            status: 200,
            statusText: 'OK',
            headers: {},
            config: {},
          };
        const mockPost = jest.spyOn(axios, 'post')

        mockPost.mockImplementation((url) => {
                return Promise.resolve(mockedResponse);
        }); 

        const historyMock = { push: jest.fn(), location: {}, listen: jest.fn() };

        render(<Router history={historyMock}><LoginPage /></Router>);

        const emailField = screen.getByRole('textbox', { name: 'Email' });
        const passwordField = screen.getByLabelText('Password');
        const button = screen.getByRole('button');

         // fill out and submit form
        fireEvent.change(emailField, { target: { value: 'test@email.com' } });
        fireEvent.change(passwordField, { target: { value: 'password' } });
        fireEvent.click(button);

        // Assert if login page has trying to move to the admin page
        await waitFor(() => {
            expect(historyMock.push.mock.calls[0][0]).toEqual(`/municipalities/${user.municipalityId}/admin`); // https://stackoverflow.com/questions/58392815/how-to-mock-usehistory-hook-in-jest
        });
    });
});

