import React from 'react';
import { render, screen } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import NotFound from '../NotFound'; // Ajustează calea de import dacă este necesar
import '@testing-library/jest-dom'
describe('NotFound Page', () => {
  test('renders 404 error message and go back home link', () => {
    render(
      <BrowserRouter>
        <NotFound />
      </BrowserRouter>
    );

    // Verifică dacă textul "404" este prezent
    const errorCode = screen.getByText('404');
    expect(errorCode).toBeInTheDocument();

    // Verifică dacă mesajul "Uh-oh!" este prezent
    const uhOhMessage = screen.getByText('Uh-oh!');
    expect(uhOhMessage).toBeInTheDocument();

    // Verifică dacă textul "We can't find that page." este prezent
    const notFoundMessage = screen.getByText("We can't find that page.");
    expect(notFoundMessage).toBeInTheDocument();

    // Verifică dacă există un link pentru a reveni la pagina de acasă și dacă acesta are textul corect
    const goBackHomeLink = screen.getByRole('link', { name: 'Go Back Home' });
    expect(goBackHomeLink).toBeInTheDocument();
    expect(goBackHomeLink).toHaveAttribute('href', '/dashboard/home');
  });
});
