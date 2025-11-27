const catalog = document.getElementById("catalog");
const noResults = document.getElementById("noResults");
const searchInput = document.getElementById("search");
const searchInputMobile = document.getElementById("searchMobile");

const sortBtn = document.getElementById('sortBtn');
const sortMenu = document.getElementById('sortMenu');
const sortBtnText = document.getElementById('sortBtnText');
const sortBtnIcon = document.getElementById('sortBtnIcon');
const sortBtnIconDark = document.getElementById('sortBtnIconDark');
const sortOptions = document.querySelectorAll('.sort-option');
const sortSelectMobile = document.getElementById("sortMobile");

const authButtons = document.getElementById("authButtons");

const manufacturerFilter = document.querySelectorAll(".manufacturer-checkbox");
const minPriceFilter = document.getElementById("minPriceFilter");
const maxPriceFilter = document.getElementById("maxPriceFilter");
const ramFilter = document.getElementById("ramFilter");
const storageFilter = document.getElementById("storageFilter");
const displayMinFilter = document.getElementById("displayMinFilter");
const displayMaxFilter = document.getElementById("displayMaxFilter");
const displayHzFilter = document.getElementById("displayHzFilter");

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
const prevImgBtn = document.getElementById('prevImgBtn');
const nextImgBtn = document.getElementById('nextImgBtn');
const imgCounter = document.getElementById('imgCounter');
const galleryWrapper = document.getElementById('galleryWrapper');
const imageLoader = document.getElementById('imageLoader');

const scrollBtn = document.getElementById('scrollBtn');
const scrollBtnIcon = document.getElementById('scrollBtnIcon');

const API_BASE_URL = 'https://mobix.onrender.com';

let allSmartphones = [];
let userFavorites = new Set();
let token = localStorage.getItem('mobix_jwt_token');
let currentSortValue = 'name';

let currentGalleryImages = [];
let currentImageIndex = 0;
let touchStartX = 0;
let touchEndX = 0;

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
        const userEmail = localStorage.getItem('mobix_user_email') || 'Мій профіль';
        const userAvatar = localStorage.getItem('mobix_user_avatar') || 'public/assets/icon-user.png';

        authButtons.innerHTML = `
            <div class="relative group">
                <button id="profileDropdownBtn" class="px-2 sm:px-3 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 flex items-center justify-center gap-1 text-sm dark:bg-blue-600 dark:hover:bg-blue-700 font-medium w-full">
                    <img src="${userAvatar}" alt="Profile" class="h-6 w-6 rounded-full object-cover border border-white/30">
                    <span class="hidden sm:inline ml-1 max-w-[150px] truncate">${userEmail}</span>
                </button>
                
                <div id="dropdownMenu" class="absolute right-0 mt-2 bg-white rounded-md shadow-lg py-2 hidden z-20 border border-gray-100 dark:bg-gray-800 dark:border-gray-700 w-28 sm:w-full text-center flex flex-col gap-1 p-1">
                    <a href="account.html" class="block px-4 py-2 text-sm lg:text-base lg:font-bold text-gray-700 hover:bg-gray-100 rounded-md dark:text-gray-200 dark:hover:bg-gray-700">Профіль</a>
                    <button id="logoutDropdownBtn" class="w-full block px-4 py-2 text-sm lg:text-base lg:font-bold rounded-md bg-red-200 text-gray-800 hover:bg-red-300 transition dark:bg-red-900 dark:text-gray-100 dark:hover:bg-red-800">Вихід</button>
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
            localStorage.removeItem('mobix_user_email');
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
        const response = await fetch(`${API_BASE_URL}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email: storedEmail, password: storedPassword })
        });

        const data = await response.json();

        if (response.ok && data.token) {
            token = data.token;
            localStorage.setItem('mobix_jwt_token', token);
            if (data.User && data.User.email) {
                localStorage.setItem('mobix_user_email', data.User.email);
                if (data.User.avatarUrl) {
        localStorage.setItem('mobix_user_avatar', data.User.avatarUrl);
    }
            }

            renderAuthButtons(true);
            await fetchFavoritesList();
            fetchSmartphones(currentSortValue);
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
        const response = await fetch(`${API_BASE_URL}/api/users/profile`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (response.ok) {
            const profile = await response.json();
            userFavorites = new Set(profile.favorites.map(f => f.id));
            if (profile.email) {
                localStorage.setItem('mobix_user_email', profile.email);
                if (profile.avatarUrl) {
        localStorage.setItem('mobix_user_avatar', profile.avatarUrl);
    }
                renderAuthButtons(true);
            }

        } else if (response.status === 401) {
            localStorage.removeItem('mobix_jwt_token');
            token = null;
            renderAuthButtons(false);
        }
    } catch (error) {
        console.warn("Не вдалося завантажити обране, продовжуємо без нього.");
    }
}

async function toggleFavorite(smartphoneId, isCurrentlyFavorite) {
    if (!token) {
        alert("Для керування обраним необхідно увійти.");
        return;
    }

    const method = isCurrentlyFavorite ? 'DELETE' : 'POST';
    const url = `${API_BASE_URL}/api/users/favorites/${smartphoneId}`;

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
        try {
            const storeUrl = phone.minPrice > 0 ? phone.storeUrl : '#';
            const isFavorite = userFavorites.has(phone.id);
            const showPriceRange = phone.maxPrice && phone.maxPrice > phone.minPrice;
            const priceAlignmentClass = "text-left";

            let specsHTML = '';
            if (phone.displaySize) specsHTML += `<li class="flex justify-between py-2 border-b border-gray-100 dark:border-gray-700"><span>Дисплей:</span> <span class="font-medium text-gray-900 dark:text-gray-100">${phone.displaySize}" ${phone.displayHz ? `(${phone.displayHz}Hz)` : ''}</span></li>`;
            if (phone.ram) specsHTML += `<li class="flex justify-between py-2 border-b border-gray-100 dark:border-gray-700"><span>RAM:</span> <span class="font-medium text-gray-900 dark:text-gray-100">${phone.ram}</span></li>`;
            if (phone.storage) specsHTML += `<li class="flex justify-between py-2 border-b border-gray-100 dark:border-gray-700"><span>Пам'ять:</span> <span class="font-medium text-gray-900 dark:text-gray-100">${phone.storage}</span></li>`;
            if (specsHTML === '') specsHTML = '<li class="text-center text-gray-400 italic py-10">Характеристики уточнюються</li>';

            const card = document.createElement("div");
            card.id = `card-${phone.id}`;
            card.className = "w-full bg-white rounded-xl shadow-md hover:shadow-xl transition-shadow duration-300 dark:bg-gray-800 border border-gray-100 dark:border-gray-700 overflow-hidden flex flex-col h-full";

            card.innerHTML = `
                <div id="front-${phone.id}" class="flex flex-col h-full fade-enter">
                    <div class="flex justify-center bg-gray-50 p-4 dark:bg-gray-700 h-48 flex-shrink-0 relative">
                        <img class="h-full w-full object-contain cursor-pointer card-image-trigger transition-transform duration-300 hover:scale-105" 
                             data-id="${phone.id}" 
                             src="${phone.imageUrl || 'https://placehold.co/300x200'}" 
                             alt="${phone.name}">
                    </div>

                    <div class="p-4 flex flex-col flex-1">
                        <h2 class="text-lg font-semibold text-gray-800 dark:text-gray-100 h-14 overflow-hidden line-clamp-2 leading-tight mb-1" title="${phone.name}">
                            ${phone.name}
                        </h2>
                        
                        <p class="text-gray-500 text-xs uppercase tracking-wide font-bold dark:text-gray-400 mb-3">
                            ${phone.manufacturer || ''}
                        </p>

                        <div class="mb-2">
                             <button class="toggle-specs-btn text-base text-blue-600 hover:text-blue-800 dark:text-blue-400 dark:hover:text-blue-300 font-medium flex items-center gap-1.5 focus:outline-none transition-colors py-1" data-card-id="${phone.id}">
                                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-5 h-5">
                                  <path stroke-linecap="round" stroke-linejoin="round" d="M11.25 11.25l.041-.02a.75.75 0 011.063.852l-.708 2.836a.75.75 0 001.063.853l.041-.021M21 12a9 9 0 11-18 0 9 9 0 0118 0zm-9-3.75h.008v.008H12V8.25z" />
                                </svg>
                                Характеристики
                            </button>
                        </div>
                        
                        <div class="mt-auto w-full">
                            <div class="${priceAlignmentClass} mb-1">
                                <p class="text-green-600 text-lg font-bold leading-tight">
                                    ${phone.minPrice > 0 ? `від <span class="underline">${phone.minPrice.toFixed(0)}</span>` : 'Ціна не знайдена'}
                                    ${showPriceRange ? ` до <span class="underline">${phone.maxPrice.toFixed(0)}</span>` : ''}
                                    ${phone.minPrice > 0 ? 'грн' : ''}
                                </p>
                            </div>
                            
                            <p class="text-gray-500 text-xs mb-3 dark:text-gray-400 h-4 overflow-hidden text-left whitespace-nowrap text-ellipsis">
                                ${phone.storeName ? `Найнижча ціна у: <b>${phone.storeName}</b>` : '&nbsp;'}
                            </p>
                            
                            <div class="flex gap-2">
                                <button data-id="${phone.id}" data-isfavorite="${isFavorite}" class="favorite-toggle-btn flex-1 px-3 py-2 rounded-full text-sm transition whitespace-nowrap flex items-center justify-center gap-1.5 font-medium ${isFavorite ? 'bg-green-500 text-white hover:bg-green-600' : 'bg-gray-100 text-gray-700 hover:bg-gray-200 dark:bg-gray-700 dark:text-gray-200 dark:hover:bg-gray-600'}">
                                    <img src="${isFavorite ? 'public/assets/icon-star-selected.png' : 'public/assets/icon-star.png'}" alt="Star" class="w-4 h-4">
                                    <span>${isFavorite ? 'В обраному' : 'Додати'}</span>
                                </button>
                                
                                <a href="${storeUrl}" target="_blank" class="flex-1 text-center bg-blue-500 text-white py-2 px-3 rounded-full hover:bg-blue-600 text-sm font-medium flex items-center justify-center ${phone.minPrice === 0 ? 'opacity-50 pointer-events-none' : ''}">
                                    ${phone.minPrice > 0 ? 'Купити' : 'Н/Д'}
                                </a>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="back-${phone.id}" class="hidden flex-col h-full bg-gray-50 dark:bg-gray-900 p-6 items-center justify-center text-center fade-enter">
                    <h3 class="text-lg font-bold text-gray-800 dark:text-white mb-6">Характеристики</h3>
                    
                    <ul class="text-sm text-gray-600 dark:text-gray-300 w-full space-y-3 text-left mb-auto">
                        ${specsHTML}
                    </ul>

                    <button class="toggle-specs-btn mt-auto bg-blue-500 text-white py-2 px-6 rounded-full hover:bg-blue-600 text-sm font-medium flex items-center justify-center gap-2 shadow-md transition-colors focus:outline-none" data-card-id="${phone.id}">
                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-4 h-4">
                          <path stroke-linecap="round" stroke-linejoin="round" d="M9 15L3 9m0 0l6-6M3 9h12a6 6 0 010 12h-3" />
                        </svg>
                        Назад до товару
                    </button>
                </div>
            `;
            
            catalog.appendChild(card);
        } catch (err) {
            console.error("Error rendering card:", err);
        }
    });

    document.querySelectorAll('.toggle-specs-btn').forEach(btn => {
        btn.addEventListener('click', (e) => {
            e.stopPropagation();
            const cardId = e.currentTarget.dataset.cardId;
            
            const frontEl = document.getElementById(`front-${cardId}`);
            const backEl = document.getElementById(`back-${cardId}`);

            if (frontEl && backEl) {
                if (!frontEl.classList.contains('hidden')) {
                    frontEl.classList.add('hidden');
                    backEl.classList.remove('hidden');
                    backEl.classList.add('flex');
                } else {
                    backEl.classList.add('hidden');
                    backEl.classList.remove('flex');
                    frontEl.classList.remove('hidden');
                }
            }
        });
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
            const id = parseInt(e.currentTarget.dataset.id);
            openImageModal(id);
        });
    });
}

function fetchSmartphones(sortBy = '') {
    showSkeletons(16);

    const selectedManufacturers = Array.from(manufacturerFilter)
        .filter(checkbox => checkbox.checked)
        .map(checkbox => checkbox.value)
        .join(',');

    const minPrice = minPriceFilter.value ? parseInt(minPriceFilter.value) : '';
    const maxPrice = maxPriceFilter.value ? parseInt(maxPriceFilter.value) : '';

    const ram = ramFilter.value;
    const storage = storageFilter.value;
    const displayHz = displayHzFilter.value;
    
    const url = `${API_BASE_URL}/api/smartphones?sortBy=${sortBy}&manufacturer=${selectedManufacturers}&minPrice=${minPrice}&maxPrice=${maxPrice}&ram=${ram}&storage=${storage}&displayHz=${displayHz}`;

    fetchFavoritesList().then(() => {
        fetch(url).then(response => {
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            return response.json();
        })
        .then(data => {
            const minDisp = parseFloat(displayMinFilter.value) || 0;
            const maxDisp = parseFloat(displayMaxFilter.value) || 100;

            allSmartphones = data.filter(phone => {
                const size = parseFloat(phone.displaySize || 0);
                if (displayMinFilter.value || displayMaxFilter.value) {
                    return size >= minDisp && size <= maxDisp;
                }
                return true;
            });
            
            filterAndRender();
        })
        .catch(error => {
            console.error("Не вдалося завантажити смартфони:", error);
            catalog.innerHTML = "";
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

    currentSortValue = 'name';
    if (sortBtnText) sortBtnText.textContent = "Сортування";

    if (sortSelectMobile) {
        sortSelectMobile.value = "name";
        updateMobileSelectIcon("name");
    }

    manufacturerFilter.forEach(checkbox => { checkbox.checked = false; });
    minPriceFilter.value = '';
    maxPriceFilter.value = '';
    ramFilter.value = '';
    storageFilter.value = '';
    displayMinFilter.value = '';
    displayMaxFilter.value = '';
    displayHzFilter.value = '';

    fetchSmartphones(currentSortValue);
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
        fetchSmartphones(currentSortValue);
    });

    resetFilterBtn.addEventListener('click', resetFilters);
} else {
    console.error("Елементи фільтрації не знайдено.");
}

if (sortBtn && sortMenu) {
    sortBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        const isHidden = sortMenu.classList.contains('hidden');
        if (isHidden) {
            sortMenu.classList.remove('hidden');
            setTimeout(() => {
                sortMenu.classList.remove('opacity-0', 'scale-95');
                sortMenu.classList.add('opacity-100', 'scale-100');
            }, 10);
        } else {
            closeSortMenu();
        }
    });

    document.addEventListener('click', (e) => {
        if (!sortBtn.contains(e.target) && !sortMenu.contains(e.target)) {
            closeSortMenu();
        }
    });

    function closeSortMenu() {
        sortMenu.classList.remove('opacity-100', 'scale-100');
        sortMenu.classList.add('opacity-0', 'scale-95');
        setTimeout(() => {
            sortMenu.classList.add('hidden');
        }, 200);
    }

    sortOptions.forEach(option => {
        option.addEventListener('click', () => {
            const value = option.dataset.value;
            const text = option.innerText.trim();

            currentSortValue = value;
            sortBtnText.textContent = text;

            if (sortSelectMobile) sortSelectMobile.value = value;

            fetchSmartphones(currentSortValue);
            closeSortMenu();
        });
    });
}

function updateMobileSelectIcon(value) {
    if (!sortSelectMobile) return;
    sortSelectMobile.classList.remove(
        'icon-sort', 'icon-sort-name', 'icon-sort-ascending', 'icon-sort-descending',
        'icon-ram', 'icon-memory', 'icon-display-size', 'icon-display-rate'
    );
    switch (value) {
        case 'name':
            sortSelectMobile.classList.add('icon-sort-name');
            break;
        case 'cheap':
            sortSelectMobile.classList.add('icon-sort-ascending');
            break;
        case 'expensive':
            sortSelectMobile.classList.add('icon-sort-descending');
            break;
        case 'ram_desc':
            sortSelectMobile.classList.add('icon-ram');
            break;
        case 'storage_desc':
            sortSelectMobile.classList.add('icon-memory');
            break;
        case 'display_size_desc':
            sortSelectMobile.classList.add('icon-display-size');
            break;
        case 'display_hz_desc':
            sortSelectMobile.classList.add('icon-display-rate');
            break;
        default:
            sortSelectMobile.classList.add('icon-sort-name');
    }
}

if (sortSelectMobile) {
    updateMobileSelectIcon(sortSelectMobile.value || 'name');
    sortSelectMobile.addEventListener('change', (e) => {
        currentSortValue = e.target.value;
        updateMobileSelectIcon(currentSortValue);
        fetchSmartphones(currentSortValue);
        const selectedOption = document.querySelector(`.sort-option[data-value="${currentSortValue}"]`);
        if (selectedOption) sortBtnText.textContent = selectedOption.innerText.trim();
    });
}


manufacturerFilter.forEach(checkbox => {
    checkbox.addEventListener("change", () => {});
});

searchInput.addEventListener("input", filterAndRender);
searchInputMobile.addEventListener("input", filterAndRender);

function openImageModal(smartphoneId) {
    const phone = allSmartphones.find(p => p.id === smartphoneId);
    if (!phone) return;

    currentGalleryImages = [];
    if (phone.imageUrl) currentGalleryImages.push(phone.imageUrl);
    if (phone.imageUrl2) currentGalleryImages.push(phone.imageUrl2);
    if (phone.imageUrl3) currentGalleryImages.push(phone.imageUrl3);

    if (currentGalleryImages.length === 0) {
        currentGalleryImages.push('https://placehold.co/300x400?text=No+Image');
    }

    currentImageIndex = 0;
    
    imageModalOverlay.classList.remove('hidden');
    imageModalOverlay.classList.add('flex');
    document.body.style.overflow = 'hidden';
    
    modalImage.className = "pointer-events-auto max-w-[95vw] max-h-[85vh] object-contain rounded-lg select-none transition-transform duration-300";
    
    updateGalleryView(false); 
}

function updateGalleryView(animate = true) {
    if (imageLoader) imageLoader.classList.remove('hidden');
    modalImage.classList.add('opacity-0');

    const tempImg = new Image();
    const targetSrc = currentGalleryImages[currentImageIndex];
    tempImg.src = targetSrc;

    tempImg.onload = () => {
        modalImage.src = targetSrc;
        if (imageLoader) imageLoader.classList.add('hidden');
        modalImage.classList.remove('opacity-0');
        
        updateControls();
        
        const nextIndex = (currentImageIndex + 1) % currentGalleryImages.length;
        const nextImg = new Image();
        nextImg.src = currentGalleryImages[nextIndex];
    };

    tempImg.onerror = () => {
        if (imageLoader) imageLoader.classList.add('hidden');
        modalImage.src = "https://placehold.co/600x400?text=Error+Loading";
        modalImage.classList.remove('opacity-0');
    };
}

function updateControls() {
    if (currentGalleryImages.length > 1) {
        if(prevImgBtn) prevImgBtn.classList.remove('hidden');
        if(nextImgBtn) nextImgBtn.classList.remove('hidden');
        if(imgCounter) {
            imgCounter.classList.remove('hidden');
            imgCounter.textContent = `${currentImageIndex + 1} / ${currentGalleryImages.length}`;
        }
    } else {
        if(prevImgBtn) prevImgBtn.classList.add('hidden');
        if(nextImgBtn) nextImgBtn.classList.add('hidden');
        if(imgCounter) imgCounter.classList.add('hidden');
    }
}

function changeSlide(direction) {
    if (currentGalleryImages.length <= 1) return;

    if (direction === 'next') {
        currentImageIndex = (currentImageIndex + 1) % currentGalleryImages.length;
    } else {
        currentImageIndex = (currentImageIndex - 1 + currentGalleryImages.length) % currentGalleryImages.length;
    }
    
    updateGalleryView(true);
}

function closeImageModal() {
    imageModalOverlay.classList.add('hidden');
    imageModalOverlay.classList.remove('flex');
    modalImage.src = "";
    currentGalleryImages = [];
    document.body.style.overflow = '';
}

if(prevImgBtn) prevImgBtn.addEventListener('click', (e) => { e.stopPropagation(); changeSlide('prev'); });
if(nextImgBtn) nextImgBtn.addEventListener('click', (e) => { e.stopPropagation(); changeSlide('next'); });

if (modalCloseBtn) {
    modalCloseBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        closeImageModal();
    });
}

imageModalOverlay.addEventListener('click', (e) => {
    if (e.target === imageModalOverlay || e.target === galleryWrapper) {
        closeImageModal();
    }
});

document.addEventListener('keydown', (e) => {
    if (imageModalOverlay.classList.contains('hidden')) return;
    if (e.key === 'ArrowRight') changeSlide('next');
    if (e.key === 'ArrowLeft') changeSlide('prev');
    if (e.key === 'Escape') closeImageModal();
});

// Свайпи
if (modalImage) {
    modalImage.addEventListener('touchstart', (e) => {
        touchStartX = e.changedTouches[0].screenX;
    }, { passive: true });

    modalImage.addEventListener('touchend', (e) => {
        touchEndX = e.changedTouches[0].screenX;
        handleSwipe();
    }, { passive: true });
}

function handleSwipe() {
    const swipeThreshold = 40;
    const swipeDistance = touchEndX - touchStartX;

    if (Math.abs(swipeDistance) > swipeThreshold) {
        if (swipeDistance < 0) {
            changeSlide('next');
        } else {
            changeSlide('prev');
        }
    }
}

if (scrollBtn && scrollBtnIcon) {
    window.addEventListener('scroll', () => {
        const scrollTop = window.scrollY || document.documentElement.scrollTop;
        if (scrollTop > 200) {
            scrollBtn.classList.remove('opacity-0', 'pointer-events-none');
        } else {
            scrollBtn.classList.add('opacity-0', 'pointer-events-none');
        }
        const docHeight = document.documentElement.scrollHeight;
        const winHeight = window.innerHeight;
        const scrollPercent = (scrollTop / (docHeight - winHeight)) * 100;
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
            window.scrollTo({ top: 0, behavior: 'smooth' });
        } else {
            window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });
        }
    });
}

function showSkeletons(count = 16) {
    catalog.innerHTML = "";
    noResults.classList.add("hidden");
    for (let i = 0; i < count; i++) {
        const skeleton = document.createElement("div");
        skeleton.className = "bg-white dark:bg-gray-800 rounded-2xl shadow-md p-5 animate-pulse flex flex-col w-[326px] mx-auto sm:w-full sm:min-w-[240px] md:min-w-[260px] lg:min-w-[294px] h-[440px]";
        skeleton.innerHTML = `
            <div class="w-full h-48 bg-gray-200 dark:bg-gray-700 rounded-xl mb-5"></div>
            <div class="h-4 bg-gray-200 dark:bg-gray-700 rounded mb-3 w-4/5"></div>
            <div class="h-4 bg-gray-200 dark:bg-gray-700 rounded mb-3 w-3/5"></div>
            <div class="h-4 bg-gray-200 dark:bg-gray-700 rounded mb-6 w-2/5"></div>
            <div class="flex justify-between mt-auto">
                <div class="h-9 w-24 bg-gray-200 dark:bg-gray-700 rounded-2xl"></div>
                <div class="h-9 w-24 bg-gray-200 dark:bg-gray-700 rounded-2xl"></div>
            </div>
        `;
        catalog.appendChild(skeleton);
    }
}

renderAuthButtons(!!token);
showSkeletons(16);
fetchSmartphones();