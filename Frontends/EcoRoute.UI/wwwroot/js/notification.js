window.showToast = function (title, message, type, duration) {
    console.log("showToast called:", title, message, type);

    let toastContainer = document.getElementById('toast-container');
    if (!toastContainer) {
        toastContainer = document.createElement('div');
        toastContainer.id = 'toast-container';
        toastContainer.className = 'fixed top-4 right-4 z-50 flex flex-col gap-2';
        document.body.appendChild(toastContainer);
    }

    
    const toast = document.createElement('div');
    toast.className = "flex items-start p-4 mb-2 max-w-xs rounded-lg shadow-lg transform transition-all duration-300 ease-in-out translate-x-full opacity-0";

    switch (type) {
        case 'success':
            toast.classList.add('bg-green-50', 'dark:bg-green-900/30', 'border-l-4', 'border-green-500');
            break;
        case 'warning':
            toast.classList.add('bg-yellow-50', 'dark:bg-yellow-900/30', 'border-l-4', 'border-yellow-500');
            break;
        case 'error':
            toast.classList.add('bg-red-50', 'dark:bg-red-900/30', 'border-l-4', 'border-red-500');
            break;
        default: // info
            toast.classList.add('bg-blue-50', 'dark:bg-blue-900/30', 'border-l-4', 'border-blue-500');
    }

    let icon;
    switch (type) {
        case 'success':
            icon = '<i class="fas fa-check text-green-500 dark:text-green-400"></i>';
            break;
        case 'warning':
            icon = '<i class="fas fa-exclamation-triangle text-yellow-500 dark:text-yellow-400"></i>';
            break;
        case 'error':
            icon = '<i class="fas fa-times-circle text-red-500 dark:text-red-400"></i>';
            break;
        default: 
            icon = '<i class="fas fa-info-circle text-blue-500 dark:text-blue-400"></i>';
    }

    toast.innerHTML = `
        <div class="flex-shrink-0 w-6 h-6 flex items-center justify-center mr-3">
            ${icon}
        </div>
        <div class="flex-1">
            <p class="font-medium text-gray-900 dark:text-white">${title}</p>
            <p class="mt-1 text-sm text-gray-700 dark:text-gray-300">${message}</p>
        </div>
        <button class="ml-3 flex-shrink-0 text-gray-400 hover:text-gray-500 dark:hover:text-gray-300">
            <i class="fas fa-times"></i>
        </button>
    `;

    toastContainer.appendChild(toast);

    const closeButton = toast.querySelector('button');
    closeButton.addEventListener('click', function () {
        removeToast(toast);
    });

    setTimeout(function () {
        toast.classList.remove('translate-x-full', 'opacity-0');
    }, 10);

    setTimeout(function () {
        removeToast(toast);
    }, duration || 5000);
};

function removeToast(toast) {
    toast.classList.add('translate-x-full', 'opacity-0');
    setTimeout(function () {
        if (toast.parentNode) {
            toast.parentNode.removeChild(toast);
        }
    }, 300);
}