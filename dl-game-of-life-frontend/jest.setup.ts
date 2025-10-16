// jest.setup.js
import "@testing-library/jest-dom";

jest.mock("next/navigation", () => ({
    useRouter() {
        return {
            push: jest.fn(),
            replace: jest.fn(),
            prefetch: jest.fn(),
            back: jest.fn(),
            pathname: "/",
        };
    },
}));



// Mock the global fetch function to avoid ReferenceError in tests
beforeEach(() => {
  global.fetch = jest.fn(() =>
    Promise.resolve({
      ok: true, // Adicionado para simular uma resposta bem-sucedida
      status: 200, // Adicionado para simular um status HTTP OK
      json: () => Promise.resolve({}), // Resposta de mock padrão
    })
  ) as jest.Mock;
});

