module.exports = {
  preset: 'react-native',
  transform: {
    '^.+\\.ts?$': 'ts-jest',
  },
  transformIgnorePatterns: [
    '<rootDir>/node_modules/(?!(jest-)?@?react-native|@react-native-community|@react-navigation)',
  ],
};
