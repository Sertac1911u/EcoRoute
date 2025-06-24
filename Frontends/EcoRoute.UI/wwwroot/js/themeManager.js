
window.themeManager = {
    updateCssVariables: function (colorValue, colorShades) {
        try {
            document.documentElement.style.setProperty('--color-primary', colorValue);

            if (colorShades) {
                if (colorShades['300']) {
                    document.documentElement.style.setProperty('--color-primary-light', colorShades['300']);
                }

                if (colorShades['700']) {
                    document.documentElement.style.setProperty('--color-primary-dark', colorShades['700']);
                }
            }

            console.log('Theme colors updated successfully');
            return true;
        } catch (error) {
            console.error('Error updating theme colors:', error);
            return false;
        }
    },

    resetToDefault: function () {
        const defaultColor = '#2ba86d';

        const defaultShades = {
            '50': '#e7f8ef',
            '100': '#c5edd8',
            '200': '#9fe3be',
            '300': '#74d8a3',  // light
            '400': '#4acd87',
            '500': '#2ba86d',  // primary
            '600': '#1f8c58',
            '700': '#166e44',  // dark
            '800': '#0e5031',
            '900': '#06321d'
        };

        this.updateCssVariables(defaultColor, defaultShades);

        this.applyTailwindTheme(defaultColor, defaultShades);

        this.saveThemePreference(defaultColor, defaultShades);

        return true;
    },

    applyTailwindTheme: function (colorValue, colorShades) {
        try {
            const styleId = 'dynamic-theme-styles';
            let styleTag = document.getElementById(styleId);

            if (styleTag) {
                styleTag.remove();
            }

            styleTag = document.createElement('style');
            styleTag.id = styleId;

            let cssContent = `
                .bg-primary-500 { background-color: ${colorValue} !important; }
                .text-primary-500 { color: ${colorValue} !important; }
                .border-primary-500 { border-color: ${colorValue} !important; }
                .ring-primary-500 { --tw-ring-color: ${colorValue} !important; }
                .hover\\:bg-primary-600:hover { background-color: ${colorShades && colorShades['600'] ? colorShades['600'] : colorValue} !important; }
            `;

            if (colorShades) {
                Object.keys(colorShades).forEach(shade => {
                    const color = colorShades[shade];
                    cssContent += `
                        .bg-primary-${shade} { background-color: ${color} !important; }
                        .text-primary-${shade} { color: ${color} !important; }
                        .border-primary-${shade} { border-color: ${color} !important; }
                    `;
                });
            }

            styleTag.textContent = cssContent;
            document.head.appendChild(styleTag);

            console.log('Tailwind theme applied successfully');
            return true;
        } catch (error) {
            console.error('Error applying Tailwind theme:', error);
            return false;
        }
    },

    saveThemePreference: function (colorValue, colorShades) {
        try {
            const themeData = {
                primary: colorValue,
                shades: colorShades || null
            };

            localStorage.setItem('themeColors', JSON.stringify(themeData));
            return true;
        } catch (error) {
            console.error('Error saving theme preference:', error);
            return false;
        }
    },

    applyTheme: function (colorValue, colorShades) {
        this.updateCssVariables(colorValue, colorShades);
        this.applyTailwindTheme(colorValue, colorShades);
        this.saveThemePreference(colorValue, colorShades);
    },

    loadSavedTheme: function () {
        try {
            const savedTheme = localStorage.getItem('themeColors');
            if (savedTheme) {
                const themeData = JSON.parse(savedTheme);
                if (themeData.primary) {
                    this.updateCssVariables(themeData.primary, themeData.shades);
                    this.applyTailwindTheme(themeData.primary, themeData.shades);
                }
            }
        } catch (error) {
            console.error('Error loading saved theme:', error);
        }
    }
};

document.addEventListener('DOMContentLoaded', function () {
    window.themeManager.loadSavedTheme();
});