const purgecss = require('@fullhuman/postcss-purgecss')({
    content: ['./src/**/*.svelte', './src/**/*.html'],
    safelist: ["bg-green-500", "focus:ring-green-300", "bg-pink-500", "focus:ring-pink-300", "bg-blue-500", "focus:ring-blue-300", "bg-red-500"],
    whitelistPatterns: [/svelte-/],
    defaultExtractor: (content) => content.match(/[A-Za-z0-9-_:/]+/g) || []
})

module.exports = {
    plugins: [
        require('tailwindcss'),
        ...(!process.env.ROLLUP_WATCH ? [purgecss] : [])
    ]
}
