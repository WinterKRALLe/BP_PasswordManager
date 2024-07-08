/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Components/*.razor',
        './Layout/*.razor',
        './Pages/**/*.razor',
        './wwwroot/index.html'
    ],
    theme: {
        fontFamily: {
            'quicksand': ["Quicksand"],
        },
        extend: {
            gridTemplateColumns: {
                'wide-screen': 'minmax(320px, 1fr) 5fr',
            },
            colors: {
                skin: {
                    accent: 'var(--color-accent)',
                    accent_muted: 'var(--color-accent-muted)',
                    base: 'var(--color-base)',
                    base2: 'var(--color-base2)',
                    muted: 'var(--color-muted)',
                    inverted: 'var(--color-inverted)',
                    fill: 'var(--color-fill)',
                    fill_secondary: 'var(--color-fill-secondary)',
                    button_accent: 'var(--color-button-accent)',
                    button_accent_hover: 'var(--color-button-accent-hover)',
                    button_muted: 'var(--color-button-muted)',
                    login: 'var(--color-login)',
                    password: 'var(--color-password)',
                    password_alpha: 'var(--color-password-alpha)',
                    password_muted: 'var(--color-password-muted)',
                    group: 'var(--color-group)',
                    group_alpha: 'var(--color-group-alpha)',
                    group_muted: 'var(--color-group-muted)',
                    border: 'var(--color-border)',
                },
            },
            textColor: {
                skin: {
                    accent: 'var(--color-accent)',
                    base: 'var(--color-text-base)',
                    muted: 'var(--color-text-muted)',
                    inverted: 'var(--color-inverted)',
                },
            },
            backgroundColor: {
                skin: {
                    accent: 'var(--color-accent)',
                    accent_alpha: 'var(--color-accent-alpha)',
                    border_base: 'var(--color-border)',
                    base: 'var(--color-base)',
                    base2: 'var(--color-base2)',
                    muted: 'var(--color-muted)',
                    inverted: 'var(--color-inverted)',
                    fill: 'var(--color-fill)',
                    fill_accent: 'var(--color-fill-accent)',
                    button_accent: 'var(--color-button-accent)',
                    button_accent_hover: 'var(--color-button-accent-hover)',
                    button_muted: 'var(--color-button-muted)',
                    button_muted_hover: 'var(--color-button-muted-hover)',
                    alpha_background: 'var(--alpha-background)',
                },
            },
            borderColor: {
                skin: {
                    accent: 'var(--color-accent)',
                    base: 'var(--color-border)',
                    inverted: 'var(--color-inverted)',
                    group: 'var(--color-group)',
                }
            },
            fillColor: {
                skin: {
                    accent: 'var(--color-accent)',
                    muted: 'var(--color-button-muted-hover)',
                }
            },
        },
    },
    plugins: [],
}