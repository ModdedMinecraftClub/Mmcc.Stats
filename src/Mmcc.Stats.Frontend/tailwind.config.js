const colors = require('tailwindcss/colors');

module.exports = {
  purge: [],
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {
      fontFamily: {
        body: ["Inter", "sans-serif"],
      },
      colors: {
        gray: colors.blueGray
      },
    },
  },
  variants: {
    extend: {},
  },
  plugins: [],
}
