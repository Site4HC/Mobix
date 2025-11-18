const catalog = document.getElementById("catalog");
const noResults = document.getElementById("noResults");
const searchInput = document.getElementById("search");
const searchInputMobile = document.getElementById("searchMobile");
const sortSelect = document.getElementById("sort");
const sortSelectMobile = document.getElementById("sortMobile");
const authButtons = document.getElementById("authButtons");

const manufacturerFilter = document.querySelectorAll(".manufacturer-checkbox");
const minPriceFilter = document.getElementById("minPriceFilter");
const maxPriceFilter = document.getElementById("maxPriceFilter");

const filterSidebar = document.getElementById("filterSidebar");
const openFilterBtn = document.getElementById("openFilterBtn");
const closeFilterBtn = document.getElementById("closeFilterBtn");
const applyFilterBtn = document.getElementById("applyFilterBtn");
const resetFilterBtn = document.getElementById("resetFilterBtn");

const themeToggleBtn = document.getElementById('themeToggleBtn');
const themeToggleSpan = document.getElementById('themeToggleSpan');
const htmlElement = document.documentElement;
const bodyElement = document.body;

const imageModalOverlay = document.getElementById('imageModalOverlay');
const modalImage = document.getElementById('modalImage');
const modalCloseBtn = document.getElementById('modalCloseBtn');

const scrollBtn = document.getElementById('scrollBtn');
const scrollBtnIcon = document.getElementById('scrollBtnIcon');

let allSmartphones = [];
let userFavorites = new Set();
let token = localStorage.getItem('mobix_jwt_token');

if (themeToggleBtn && themeToggleSpan && htmlElement && bodyElement) {

    function applyTheme(theme) {
        if (theme === 'dark') {
            htmlElement.classList.add('dark');
            themeToggleBtn.classList.remove('bg-gray-200');
            themeToggleBtn.classList.add('bg-blue-600');
            themeToggleSpan.classList.remove('translate-x-1', 'bg-white');
            themeToggleSpan.classList.add('translate-x-6', 'bg-gray-600');
            themeToggleSpan.innerHTML = '🌙';

            bodyElement.classList.add('bg-dark-theme');
            bodyElement.classList.remove('bg-light-theme');

            localStorage.setItem('mobix_theme', 'dark');
        } else {
            htmlElement.classList.remove('dark');
            themeToggleBtn.classList.remove('bg-blue-600');
            themeToggleBtn.classList.add('bg-gray-200');
            themeToggleSpan.classList.remove('translate-x-6', 'bg-gray-600');
            themeToggleSpan.classList.add('translate-x-1', 'bg-white');
            themeToggleSpan.innerHTML = '☀️';

            bodyElement.classList.add('bg-light-theme');
            bodyElement.classList.remove('bg-dark-theme');

            localStorage.setItem('mobix_theme', 'light');
        }
    }

    themeToggleBtn.addEventListener('click', () => {
        const currentTheme = htmlElement.classList.contains('dark') ? 'dark' : 'light';
        applyTheme(currentTheme === 'dark' ? 'light' : 'dark');
    });

    const savedTheme = localStorage.getItem('mobix_theme') || 'light';
    applyTheme(savedTheme);

} else {
    console.error("Елементи перемикача теми не знайдено.");
}


function renderAuthButtons(isLoggedIn) {
    authButtons.innerHTML = '';
    if (isLoggedIn) {
        authButtons.innerHTML = `
            <div class="relative">
                <button id="profileDropdownBtn" class="px-2 sm:px-3 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 flex items-center gap-1 text-sm dark:bg-blue-600 dark:hover:bg-blue-700 font-medium">
                    <img src="public/assets/icon-user.png" alt="Profile" class="h-4 w-4">
                    <span class="hidden sm:inline ml-1">Мій профіль</span>
                </button>
                <div id="dropdownMenu" class="absolute right-0 mt-2 w-48 bg-white rounded-md shadow-lg py-1 hidden z-20 border border-gray-100 dark:bg-gray-800 dark:border-gray-700">
                    <a href="account.html" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:text-gray-200 dark:hover:bg-gray-700">Профіль</a>
                    <button id="logoutDropdownBtn" class="w-full text-left block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:text-gray-200 dark:hover:bg-gray-700">Вихід</button>
                </div>
            </div>
        `;

        const profileDropdownBtn = document.getElementById('profileDropdownBtn');
        const dropdownMenu = document.getElementById('dropdownMenu');
        const logoutDropdownBtn = document.getElementById('logoutDropdownBtn');

        profileDropdownBtn.addEventListener('click', () => {
            dropdownMenu.classList.toggle('hidden');
        });

        logoutDropdownBtn.addEventListener('click', () => {
            localStorage.removeItem('mobix_jwt_token');
            token = null;
            userFavorites = new Set();
            renderAuthButtons(false);
            renderCards(allSmartphones);
            dropdownMenu.classList.add('hidden');
        });

        document.addEventListener('click', (event) => {
            if (profileDropdownBtn && dropdownMenu && !profileDropdownBtn.contains(event.target) && !dropdownMenu.contains(event.target)) {
                dropdownMenu.classList.add('hidden');
            }
        });

    } else {
        authButtons.innerHTML = `
        <a href="register.html" class="px-3 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 text-sm font-medium">
            <span class="hidden sm:inline">Реєстрація</span>
            <span class="sm:hidden">Рег.</span>
        </a>
        <a href="login.html" class="px-3 py-2 bg-green-200 rounded-lg hover:bg-green-300 text-sm font-medium dark:bg-green-900 dark:text-gray-200 dark:hover:bg-green-800">
            <span class="hidden sm:inline">Вхід</span>
            <span class="sm:hidden">Вхід</span>
        </a>
      `;
    }
}

async function handleLogin() {
    const storedEmail = "nemkogilevskii@gmail.com";
    const storedPassword = "Strings";

    try {
        const response = await fetch("http://localhost:5152/api/auth/login", {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email: storedEmail, password: storedPassword })
        });

        const data = await response.json();

        if (response.ok && data.token) {
            token = data.token;
            localStorage.setItem('mobix_jwt_token', token);
            renderAuthButtons(true);
            await fetchFavoritesList();
            fetchSmartphones(sortSelect.value);
        } else {
            alert(data.message || "Помилка входу.");
        }
    } catch (error) {
        alert("Помилка підключення до API.");
    }
}

async function fetchFavoritesList() {
    userFavorites = new Set();
    if (!token) return;

    try {
        const response = await fetch('http://localhost:5152/api/users/profile', {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (response.ok) {
            const profile = await response.json();
            userFavorites = new Set(profile.favorites.map(f => f.id));
        } else if (response.status === 401) {
            localStorage.removeItem('mobix_jwt_token');
            token = null;
            renderAuthButtons(false);
        }
    } catch (error) {
        console.warn("Не вдалося завантажити вибране, продовжуємо без нього.");
    }
}

async function toggleFavorite(smartphoneId, isCurrentlyFavorite) {
    if (!token) {
        alert("Для керування вибраним необхідно увійти.");
        return;
    }

    const method = isCurrentlyFavorite ? 'DELETE' : 'POST';
    const url = `http://localhost:5152/api/users/favorites/${smartphoneId}`;

    try {
        const response = await fetch(url, {
            method: method,
            headers: { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' }
        });

        if (response.ok) {
            if (isCurrentlyFavorite) {
                userFavorites.delete(smartphoneId);
            } else {
                userFavorites.add(smartphoneId);
            }
            renderCards(allSmartphones);
        } else {
            const errorData = await response.json();
            alert(`Помилка: ${errorData.message || 'Не вдалося виконати операцію.'}`);
        }
    } catch (error) {
        alert("Помилка підключення до API.");
    }
}

function renderCards(items) {
    catalog.innerHTML = "";
    if (items.length === 0) {
        noResults.classList.remove("hidden");
        return;
    } else {
        noResults.classList.add("hidden");
    }

    items.forEach(phone => {
        const storeUrl = phone.minPrice > 0 ? phone.storeUrl : '#';
        const isFavorite = userFavorites.has(phone.id);

        const card = document.createElement("div");
        card.className = "bg-white rounded-xl shadow-md overflow-hidden flex flex-col transition hover:shadow-xl duration-300 dark:bg-gray-800";

        card.innerHTML = `
                <div class="flex justify-center bg-gray-50 p-3 dark:bg-gray-700">
                    <img class="h-44 object-contain cursor-pointer card-image-trigger" src="${phone.imageUrl || 'https://placehold.co/300x200'}" alt="${phone.name}">
                </div>
                <div class="p-4 flex flex-col justify-between flex-1">
                    <div>
                        <h2 class="text-lg font-semibold text-gray-800 dark:text-gray-100">${phone.name}</h2>
                        <p class="text-gray-500 text-sm dark:text-gray-400">${phone.manufacturer || ''}</p>
                        
                        <p class="mt-2 text-green-600 text-lg font-bold">${phone.minPrice > 0 ? `від <span class="underline">${phone.minPrice.toFixed(0)}</span> грн` : 'Ціна не знайдена'}</p>
                        
                        <p class="text-gray-500 text-sm mt-1 dark:text-gray-400">
                            ${phone.storeName ? `Найнижча ціна у: <b>${phone.storeName}</b>` : ''}
                        </p>
                    </div>
                    
                    <div class="flex mt-3 gap-2">
                        <button 
                            data-id="${phone.id}" 
                            data-isfavorite="${isFavorite}"
                            class="favorite-toggle-btn flex-1 px-3 py-2 rounded-full text-sm transition whitespace-nowrap flex items-center justify-center gap-1.5 font-medium
                            ${isFavorite ? 'bg-green-500 text-white hover:bg-green-600' : 'bg-gray-100 text-gray-700 hover:bg-gray-200 dark:bg-gray-700 dark:text-gray-200 dark:hover:bg-gray-600'}">
                            <img src="${isFavorite ? 'public/assets/icon-star-selected.png' : 'public/assets/icon-star.png'}" alt="Star" class="w-4 h-4">
                            <span>${isFavorite ? 'У вибраному' : 'Додати'}</span>
                        </button>
                        
                        <a href="${storeUrl}" target="_blank"
                            class="flex-1 text-center bg-blue-500 text-white py-2 px-3 rounded-full hover:bg-blue-600 text-sm font-medium
                            ${phone.minPrice === 0 ? 'opacity-50 pointer-events-none' : ''}">
                            ${phone.minPrice > 0 ? 'Купити' : 'Н/Д'}
                        </a>
                    </div>
                </div>
            `;
        catalog.appendChild(card);
    });

    document.querySelectorAll('.favorite-toggle-btn').forEach(btn => {
        btn.addEventListener('click', (e) => {
            const id = parseInt(e.currentTarget.dataset.id);
            const isFav = e.currentTarget.dataset.isfavorite === 'true';
            toggleFavorite(id, isFav);
        });
    });

    document.querySelectorAll('.card-image-trigger').forEach(img => {
        img.addEventListener('click', (e) => {
            openImageModal(e.currentTarget.src);
        });
    });
}

function fetchSmartphones(sortBy = '') {
    const selectedManufacturers = Array.from(manufacturerFilter)
        .filter(checkbox => checkbox.checked)
        .map(checkbox => checkbox.value)
        .join(',');

    const minPrice = minPriceFilter.value ? parseInt(minPriceFilter.value) : '';
    const maxPrice = maxPriceFilter.value ? parseInt(maxPriceFilter.value) : '';

    const API_BASE_URL = 'https://mobix.onrender.com';
    const url = `${API_BASE_URL}/api/smartphones?sortBy=${sortBy}&manufacturer=${selectedManufacturers}&minPrice=${minPrice}&maxPrice=${maxPrice}`;

    fetchFavoritesList().then(() => {
        fetch(url).then(response => {
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            return response.json();
        })
            .then(data => {
                allSmartphones = data;
                filterAndRender();
            })
            .catch(error => {
                console.error("Не вдалося завантажити смартфони:", error);
                noResults.classList.remove("hidden");
            });
    });
}

function filterAndRender() {
    const currentSearchValue = searchInput.value || searchInputMobile.value;
    const query = currentSearchValue.trim().toLowerCase();
    let items = allSmartphones.filter(p => p.name.toLowerCase().includes(query));
    renderCards(items);
}

function resetFilters() {
    searchInput.value = '';
    searchInputMobile.value = '';
    sortSelect.value = '';
    sortSelectMobile.value = '';

    manufacturerFilter.forEach(checkbox => { checkbox.checked = false; });
    minPriceFilter.value = '';
    maxPriceFilter.value = '';

    fetchSmartphones();
}

if (openFilterBtn && filterSidebar && closeFilterBtn && applyFilterBtn && resetFilterBtn) {
    openFilterBtn.addEventListener('click', () => {
        filterSidebar.classList.toggle('open');
    });
    closeFilterBtn.addEventListener('click', () => {
        filterSidebar.classList.remove('open');
    });

    applyFilterBtn.addEventListener('click', () => {
        filterSidebar.classList.remove('open');
        fetchSmartphones(sortSelect.value);
    });

    resetFilterBtn.addEventListener('click', resetFilters);
} else {
    console.error("Елементи фільтрації не знайдено.");
}

function updateSortIcons(selectElement) {
    selectElement.classList.remove('icon-sort', 'icon-sort-name', 'icon-sort-ascending', 'icon-sort-descending');

    switch (selectElement.value) {
        case 'name':
            selectElement.classList.add('icon-sort-name');
            break;
        case 'cheap':
            selectElement.classList.add('icon-sort-ascending');
            break;
        case 'expensive':
            selectElement.classList.add('icon-sort-descending');
            break;
        default:
            selectElement.classList.add('icon-sort');
    }
}

sortSelect.addEventListener("change", (e) => {
    sortSelectMobile.value = e.target.value;
    fetchSmartphones(e.target.value);
    updateSortIcons(sortSelect);
    updateSortIcons(sortSelectMobile);
});
sortSelectMobile.addEventListener("change", (e) => {
    sortSelect.value = e.target.value;
    fetchSmartphones(e.target.value);
    updateSortIcons(sortSelectMobile);
    updateSortIcons(sortSelect);
});

manufacturerFilter.forEach(checkbox => {
    checkbox.addEventListener("change", () => {
    });
});

minPriceFilter.addEventListener("input", () => {
    fetchSmartphones(sortSelect.value);
});
maxPriceFilter.addEventListener("input", () => {
    fetchSmartphones(sortSelect.value);
});

searchInput.addEventListener("input", filterAndRender);
searchInputMobile.addEventListener("input", filterAndRender);

renderAuthButtons(!!token);
fetchSmartphones();

function openImageModal(src) {
    modalImage.src = src;
    imageModalOverlay.classList.remove('hidden');
    imageModalOverlay.classList.add('flex');
}

function closeImageModal() {
    imageModalOverlay.classList.add('hidden');
    imageModalOverlay.classList.remove('flex');
    modalImage.src = "";
}

modalCloseBtn.addEventListener('click', (e) => {
    e.stopPropagation();
    closeImageModal();
});

modalImage.addEventListener('click', (e) => {
    e.stopPropagation();
    closeImageModal();
});

imageModalOverlay.addEventListener('click', (e) => {
    if (e.target === imageModalOverlay) {
        closeImageModal();
    }
});

if (scrollBtn && scrollBtnIcon) {

    window.addEventListener('scroll', () => {
        const scrollTop = window.scrollY || document.documentElement.scrollTop;
        const docHeight = document.documentElement.scrollHeight;
        const winHeight = window.innerHeight;

        const scrollPercent = (scrollTop / (docHeight - winHeight)) * 100;

        if (scrollTop > 200) {
            scrollBtn.classList.remove('opacity-0', 'pointer-events-none');
        } else {
            scrollBtn.classList.add('opacity-0', 'pointer-events-none');
        }

        if (scrollPercent >= 65) {
            scrollBtnIcon.src = 'public/assets/icon-scroll-up.png';
            scrollBtn.dataset.action = 'up';
        } else {
            scrollBtnIcon.src = 'public/assets/icon-scroll-down.png';
            scrollBtn.dataset.action = 'down';
        }
    });

    scrollBtn.addEventListener('click', () => {
        if (scrollBtn.dataset.action === 'up') {
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        } else {
            window.scrollTo({
                top: document.body.scrollHeight,
                behavior: 'smooth'
            });
        }
    });

} else {
    console.error("Елементи кнопки скроллу не знайдено.");
}