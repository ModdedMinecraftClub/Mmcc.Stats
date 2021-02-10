const purgecss = require('@fullhuman/postcss-purgecss')({
    content: ['./src/**/*.svelte', './src/**/*.html'],
    safelist: ["bg-green-500", "bg-green-300", "bg-pink-500", "bg-pink-300", "bg-blue-500", "bg-blue-300"],
    whitelistPatterns: [/svelte-/],
    defaultExtractor: (content) => content.match(/[A-Za-z0-9-_:/]+/g) || []
})

module.exports = {
    plugins: [
        require('tailwindcss'),
        ...(!process.env.ROLLUP_WATCH ? [purgecss] : [])
    ]
}
