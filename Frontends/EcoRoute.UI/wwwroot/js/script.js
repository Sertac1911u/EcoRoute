//// User dropdown toggle
//const userMenuButton = document.getElementById('user-menu-button');
//const userDropdown = document.getElementById('user-dropdown');

//userMenuButton?.addEventListener('click', () => {
//    userDropdown?.classList.toggle('hidden');
//});

//// Close the dropdown when clicking outside
//document.addEventListener('click', (event) => {
//    if (!userMenuButton?.contains(event.target) && !userDropdown?.contains(event.target)) {
//        userDropdown?.classList.add('hidden');
//    }
//});

//// Dark mode toggle
//const darkModeToggle = document.getElementById('darkModeToggle');

//darkModeToggle?.addEventListener('click', () => {
//    document.documentElement.classList.toggle('dark');
//    localStorage.setItem('darkMode', document.documentElement.classList.contains('dark'));
//});

//// Set initial dark mode preference
//if (localStorage.getItem('darkMode') === 'true' ||
//    (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
//    document.documentElement.classList.add('dark');
//}

//// Mobile menu toggle
//const mobileMenuButton = document.getElementById('mobile-menu-button');

//mobileMenuButton?.addEventListener('click', () => {
//    const sidebar = document.querySelector('.md\\:flex-shrink-0');
//    if (sidebar) {
//        sidebar.classList.toggle('hidden');
//        sidebar.classList.toggle('block');
//    }
//});

//// Simulate some real-time data updates (progress bar animation)
//setInterval(() => {
//    const progressBars = document.querySelectorAll('.bg-primary-500, .bg-blue-500, .bg-yellow-500, .bg-green-500');
//    const randomBar = progressBars[Math.floor(Math.random() * progressBars.length)];

//    if (randomBar) {
//        const currentWidth = parseInt(randomBar.style.width) || 50;
//        const newWidth = Math.max(5, Math.min(100, currentWidth + (Math.random() > 0.5 ? 2 : -2)));
//        randomBar.style.width = `${newWidth}%`;

//        const parentDiv = randomBar.closest('div')?.parentElement;
//        const textElement = parentDiv?.querySelector('span:last-child');
//        if (textElement) {
//            textElement.textContent = `${newWidth}%`;
//        }
//    }
//}, 5000);
