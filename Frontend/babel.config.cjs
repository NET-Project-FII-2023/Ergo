// babel.config.cjs
module.exports = {
  presets: [
    '@babel/preset-env',
    ['@babel/preset-react', { runtime: 'automatic' }],
    '@babel/preset-typescript', // Adaugă acest rând
  ],
  // Restul configurației tale Babel...
};
