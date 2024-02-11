// jest.config.cjs sau jest.config.js
module.exports = {
    moduleNameMapper: {
      // Mapează importurile de stiluri și imagini pentru a preveni erori în timpul testelor
      '\\.(css|less|sass|scss)$': 'identity-obj-proxy',
      '\\.(gif|ttf|eot|svg|png|jpg)$': '<rootDir>/__mocks__/fileMock.js',
      "^@/(.*)$": "<rootDir>/src/$1"
    },
    transform: {
      // Utilizează babel-jest pentru transpilarea codului JS și JSX
      '^.+\\.[t|j]sx?$': 'babel-jest'
    },
    testEnvironment: 'jsdom', // Setează mediul de testare ca un browser
    
  };
  