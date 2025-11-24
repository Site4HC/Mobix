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

        authButtons.innerHTML = `
            <div class="relative group">
                <button id="profileDropdownBtn" class="px-2 sm:px-3 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 flex items-center justify-center gap-1 text-sm dark:bg-blue-600 dark:hover:bg-blue-700 font-medium w-full">
                    <img src="public/assets/icon-user.png" alt="Profile" class="h-4 w-4">
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
        const storeUrl = phone.minPrice > 0 ? phone.storeUrl : '#';
        const isFavorite = userFavorites.has(phone.id);

        const showPriceRange = phone.maxPrice && phone.maxPrice > phone.minPrice;
        const priceAlignmentClass = showPriceRange ? "text-left" : "text-center";

        let specsHTML = '';

        if (phone.displaySize) {
            specsHTML += `<div class="flex justify-between"><span class="text-gray-500">Дисплей:</span> <span class="font-medium text-gray-900 dark:text-gray-200">${phone.displaySize}" ${phone.displayHz ? `(${phone.displayHz}Hz)` : ''}</span></div>`;
        }
        if (phone.ram) {
            specsHTML += `<div class="flex justify-between"><span class="text-gray-500">RAM:</span> <span class="font-medium text-gray-900 dark:text-gray-200">${phone.ram}</span></div>`;
        }
        if (phone.storage) {
            specsHTML += `<div class="flex justify-between"><span class="text-gray-500">Пам'ять:</span> <span class="font-medium text-gray-900 dark:text-gray-200">${phone.storage}</span></div>`;
        }

        if (specsHTML === '') {
            specsHTML = '<span class="text-gray-400 italic">Характеристики не вказані</span>';
        }

        const card = document.createElement("div");
        card.className = "bg-white rounded-xl shadow-md overflow-hidden flex flex-col transition hover:shadow-xl duration-300 dark:bg-gray-800 border border-gray-100 dark:border-gray-700 h-full";

        card.innerHTML = `
                <div class="flex justify-center bg-gray-50 p-3 dark:bg-gray-700 h-48 flex-shrink-0 relative">
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

                    <div class="text-xs space-y-1 mb-4 border-t border-gray-100 dark:border-gray-700 pt-2">
                        ${specsHTML}
                    </div>
                    
                    <div class="mt-auto w-full">
                        
                        <div class="${priceAlignmentClass} mb-1">
                            <p class="text-green-600 text-lg font-bold leading-tight">
                                ${phone.minPrice > 0
                ? `від <span class="underline">${phone.minPrice.toFixed(0)}</span>`
                : 'Ціна не знайдена'}
                                
                                ${showPriceRange
                ? ` до <span class="underline">${phone.maxPrice.toFixed(0)}</span>`
                : ''}
                                
                                ${phone.minPrice > 0 ? 'грн' : ''}
                            </p>
                        </div>
                        
                        <p class="text-gray-500 text-xs mb-3 dark:text-gray-400 h-4 overflow-hidden text-left whitespace-nowrap text-ellipsis">
                            ${phone.storeName ? `Найнижча ціна у: <b>${phone.storeName}</b>` : '&nbsp;'}
                        </p>
                        
                        <div class="flex gap-2">
    <button 
        data-id="${phone.id}" 
        data-isfavorite="${isFavorite}"
        class="favorite-toggle-btn flex-1 px-3 py-2 rounded-full text-sm transition whitespace-nowrap flex items-center justify-center gap-1.5 font-medium
        ${isFavorite ? 'bg-green-500 text-white hover:bg-green-600' : 'bg-gray-100 text-gray-700 hover:bg-gray-200 dark:bg-gray-700 dark:text-gray-200 dark:hover:bg-gray-600'}">
        <img src="${isFavorite ? 'public/assets/icon-star-selected.png' : 'public/assets/icon-star.png'}" alt="Star" class="w-4 h-4">
        <span>${isFavorite ? 'В обраному' : 'Додати'}</span>
    </button>
    
    <a href="${storeUrl}" target="_blank"
        class="flex-1 text-center bg-blue-500 text-white py-2 px-3 rounded-full hover:bg-blue-600 text-sm font-medium flex items-center justify-center
        ${phone.minPrice === 0 ? 'opacity-50 pointer-events-none' : ''}">
        ${phone.minPrice > 0 ? 'Купити' : 'Н/Д'}
    </a>
</div>
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

    sortSelectMobile.classList.remove('icon-sort', 'icon-sort-name', 'icon-sort-ascending', 'icon-sort-descending');

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
        if (selectedOption) {
            sortBtnText.textContent = selectedOption.innerText.trim();
        }
    });
}


manufacturerFilter.forEach(checkbox => {
    checkbox.addEventListener("change", () => {
    });
});

minPriceFilter.addEventListener("input", () => {
    fetchSmartphones(currentSortValue);
});
maxPriceFilter.addEventListener("input", () => {
    fetchSmartphones(currentSortValue);
});

searchInput.addEventListener("input", filterAndRender);
searchInputMobile.addEventListener("input", filterAndRender);



const galleryWrapper = document.getElementById('galleryWrapper');

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

    modalImage.src = currentGalleryImages[currentImageIndex];
    modalImage.className = "pointer-events-auto max-w-[95vw] max-h-[85vh] object-contain rounded-lg select-none shadow-2xl transition-transform duration-300"; // Скидаємо класи анімації
    updateControls();
}

function updateControls() {
    if (currentGalleryImages.length > 1) {
        if (prevImgBtn) prevImgBtn.classList.remove('hidden');
        if (nextImgBtn) nextImgBtn.classList.remove('hidden');
        if (imgCounter) {
            imgCounter.classList.remove('hidden');
            imgCounter.textContent = `${currentImageIndex + 1} / ${currentGalleryImages.length}`;
        }
    } else {
        if (prevImgBtn) prevImgBtn.classList.add('hidden');
        if (nextImgBtn) nextImgBtn.classList.add('hidden');
        if (imgCounter) imgCounter.classList.add('hidden');
    }
}

function changeSlide(direction) {
    if (currentGalleryImages.length <= 1) return;

    const img = document.getElementById('modalImage');

    if (direction === 'next') {
        img.classList.add('slide-out-left');
    } else {
        img.classList.add('slide-out-right');
    }

    setTimeout(() => {
        if (direction === 'next') {
            currentImageIndex = (currentImageIndex + 1) % currentGalleryImages.length;
        } else {
            currentImageIndex = (currentImageIndex - 1 + currentGalleryImages.length) % currentGalleryImages.length;
        }

        img.src = currentGalleryImages[currentImageIndex];

        img.classList.remove('slide-out-left', 'slide-out-right');

        if (direction === 'next') {
            img.classList.add('slide-in-from-right');
            setTimeout(() => img.classList.remove('slide-in-from-right'), 300);
        } else {
            img.classList.add('slide-in-from-left');
            setTimeout(() => img.classList.remove('slide-in-from-left'), 300);
        }

        updateControls();

    }, 200);
}

function closeImageModal() {
    imageModalOverlay.classList.add('hidden');
    imageModalOverlay.classList.remove('flex');
    modalImage.src = "";
    modalImage.classList.remove('slide-out-left', 'slide-out-right', 'slide-in-from-right', 'slide-in-from-left');
    currentGalleryImages = [];
    document.body.style.overflow = '';
}

if (prevImgBtn) prevImgBtn.addEventListener('click', (e) => { e.stopPropagation(); changeSlide('prev'); });
if (nextImgBtn) nextImgBtn.addEventListener('click', (e) => { e.stopPropagation(); changeSlide('next'); });

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

function showSkeletons(count = 16) {
    catalog.innerHTML = "";
    noResults.classList.add("hidden");

    for (let i = 0; i < count; i++) {
        const skeleton = document.createElement("div");

        skeleton.className =
            "bg-white dark:bg-gray-800 rounded-2xl shadow-md p-5 animate-pulse flex flex-col " +
            "w-[326px] mx-auto sm:w-full " +
            "sm:min-w-[240px] md:min-w-[260px] lg:min-w-[294px] h-[416px]";

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