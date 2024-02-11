import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import SignIn from '../SignIn';
import '@testing-library/jest-dom';
import { toast } from 'react-toastify';
import api from '@/services/api';

// Mock pentru serviciul API


jest.mock('@/services/api', () => ({
    post: jest.fn(),
  }));

// Mock pentru biblioteca react-toastify
jest.mock('react-toastify', () => ({
    toast: {
        error: jest.fn(),
        success: jest.fn()
    }
}));

jest.mock('@/services/api', () => ({
    post: jest.fn().mockResolvedValue({ status: 200, data: 'fake_data' })
}));

describe('SignIn Component', () => {

    beforeEach(() => {
        jest.clearAllMocks();
    });

    test('shows error message on API error', async () => {
        // Setează mock-ul pentru apelul API să returneze o eroare
        api.post.mockRejectedValue({
          isAxiosError: true,
          response: {
            data: {
              validationsErrors: ['Error message from server'],
            },
          },
        });
    
        render(
          <BrowserRouter>
            <SignIn />
          </BrowserRouter>
        );
    
        // Completează inputurile și declanșează funcția de submit
        fireEvent.change(screen.getByPlaceholderText('Username'), { target: { value: 'testuser' } });
        fireEvent.change(screen.getByPlaceholderText('********'), { target: { value: 'testpass' } });
        fireEvent.click(screen.getByRole('button', { name: 'Sign In' }));
    
        // Așteaptă să se afișeze mesajul de eroare
        await waitFor(() => {
          expect(toast.error).toHaveBeenCalledWith('Login failed: Error message from server');
        });
      });
    });


    test('renders input fields and sign in button', () => {
        render(
            <BrowserRouter>
                <SignIn />
            </BrowserRouter>
        );

        const usernameInput = screen.getByPlaceholderText('Username');
        expect(usernameInput).toBeInTheDocument();

        const passwordInput = screen.getByPlaceholderText('********');
        expect(passwordInput).toBeInTheDocument();

        const signInButton = screen.getByRole('button', { name: 'Sign In' });
        expect(signInButton).toBeInTheDocument();
    });

    test('shows error message when fields are empty and sign in button is clicked', async () => {
        render(
          <BrowserRouter>
            <SignIn />
          </BrowserRouter>
        );
      
        const signInButton = screen.getByRole('button', { name: 'Sign In' });
        await waitFor(() => {
          fireEvent.click(signInButton);
        });
      
        expect(require('react-toastify').toast.error).toHaveBeenCalledWith('Please fill all fields');
      });

     
      test('submits valid username and password and navigates on successful login', async () => {
        jest.spyOn(api, 'post').mockResolvedValue({ status: 200, data: 'fake_data' });
        jest.spyOn(toast, 'success');
        render(
            <BrowserRouter>
                <SignIn />
            </BrowserRouter>
        );

        // Completează câmpurile și trimite formularul...
        fireEvent.change(screen.getByPlaceholderText('Username'), { target: { value: 'testUser' } });
        fireEvent.change(screen.getByPlaceholderText('********'), { target: { value: 'testPassword' } });
        fireEvent.click(screen.getByRole('button', { name: 'Sign In' }));

        // Verifică că s-a afișat mesajul de succes și că navigarea a fost apelată...
        await waitFor(() => {
            expect(toast.success).toHaveBeenCalledWith('Login Successful');
        }); // Poți ajusta timeout-ul dacă este necesar
        jest.restoreAllMocks();

    });


    test('shows generic error message on non-Axios error', async () => {
        // Creează un mesaj de eroare generic
        const errorMessage = 'Generic error message';
      
        // Mock-uiește apelul API pentru a respinge cu o eroare generică, nu una specifică Axios
        jest.spyOn(api, 'post').mockRejectedValue(new Error(errorMessage));
      
        render(
          <BrowserRouter>
            <SignIn />
          </BrowserRouter>
        );
      
        // Completează inputurile și declanșează funcția de submit
        fireEvent.change(screen.getByPlaceholderText('Username'), { target: { value: 'testuser' } });
        fireEvent.change(screen.getByPlaceholderText('********'), { target: { value: 'testpass' } });
        fireEvent.click(screen.getByRole('button', { name: 'Sign In' }));
      
        // Așteaptă să se afișeze mesajul de eroare
        await waitFor(() => {
          expect(toast.error).toHaveBeenCalledWith(`Login failed: ${errorMessage}`);
        });
      });
      
    


      

