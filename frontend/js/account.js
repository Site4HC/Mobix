const token = localStorage.getItem('mobix_jwt_token');
const profileSkeleton = document.getElementById('profileSkeleton');
const contentDiv = document.getElementById('content');
const favoritesList = document.getElementById('favoritesList');
const logoutBtn = document.getElementById('logoutBtn');

const imageModalOverlay = document.getElementById('imageModalOverlay');
const modalImage = document.getElementById('modalImage');
const modalCloseBtn = document.getElementById('modalCloseBtn');
const prevImgBtn = document.getElementById('prevImgBtn');
const nextImgBtn = document.getElementById('nextImgBtn');
const imgCounter = document.getElementById('imgCounter');
const galleryWrapper = document.getElementById('galleryWrapper');
const imageLoader = document.getElementById('imageLoader');

const API_BASE_URL = 'https://mobix.onrender.com';

const themeToggleBtn = document.getElementById('themeToggleBtn');
const themeToggleSpan = document.getElementById('themeToggleSpan');
const htmlElement = document.documentElement;
const bodyElement = document.body;

let enrichedFavorites = []; 
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
}

logoutBtn.addEventListener('click', () => {
    localStorage.removeItem('mobix_jwt_token');
    window.location.href = 'index.html';
});


function openImageModal(smartphoneId) {
    const phone = enrichedFavorites.find(p => p.id === smartphoneId);
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

function renderFavorites(favorites) {
    favoritesList.innerHTML = '';

    if (!favorites || favorites.length === 0) {
        favoritesList.innerHTML = `
            <p class="text-gray-500 col-span-full text-center dark:text-gray-400">
             У вас немає збережених товарів 😕
            </p>`;
        return;
    }

    favorites.forEach(phone => {
        const minPrice = phone.minPrice || 0;
        const storeUrl = minPrice > 0 ? phone.storeUrl : '#';
        const showPriceRange = phone.maxPrice && phone.maxPrice > minPrice;
        const priceAlignmentClass = "text-left";

        let specsHTML = '';
        if (phone.displaySize) specsHTML += `<li class="flex justify-between py-2 border-b border-gray-100 dark:border-gray-700"><span>Дисплей:</span> <span class="font-medium text-gray-900 dark:text-gray-100">${phone.displaySize}" ${phone.displayHz ? `(${phone.displayHz}Hz)` : ''}</span></li>`;
        if (phone.ram) specsHTML += `<li class="flex justify-between py-2 border-b border-gray-100 dark:border-gray-700"><span>RAM:</span> <span class="font-medium text-gray-900 dark:text-gray-100">${phone.ram}</span></li>`;
        if (phone.storage) specsHTML += `<li class="flex justify-between py-2 border-b border-gray-100 dark:border-gray-700"><span>Пам'ять:</span> <span class="font-medium text-gray-900 dark:text-gray-100">${phone.storage}</span></li>`;
        if (specsHTML === '') specsHTML = '<li class="text-center text-gray-400 italic py-10">Характеристики уточнюються</li>';

        const card = document.createElement('div');
        card.id = `card-${phone.id}`;
        card.className = "card-container w-full shadow-md hover:shadow-xl transition-shadow duration-300 rounded-xl border border-gray-100 dark:border-gray-700";

        card.innerHTML = `
            <div class="card-side front-side flex flex-col rounded-xl overflow-hidden">
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
                    <p class="text-gray-500 text-xs uppercase tracking-wide font-bold dark:text-gray-400 mb-2">
                        ${phone.manufacturer || ''}
                    </p>

                    <div class="mb-2">
                         <button class="flip-btn text-base text-blue-600 hover:text-blue-800 dark:text-blue-400 dark:hover:text-blue-300 font-medium flex items-center gap-1.5 focus:outline-none transition-colors py-1" data-card-id="${phone.id}">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-5 h-5">
                              <path stroke-linecap="round" stroke-linejoin="round" d="M11.25 11.25l.041-.02a.75.75 0 011.063.852l-.708 2.836a.75.75 0 001.063.853l.041-.021M21 12a9 9 0 11-18 0 9 9 0 0118 0zm-9-3.75h.008v.008H12V8.25z" />
                            </svg>
                            Характеристики
                        </button>
                    </div>
                    
                    <div class="mt-auto w-full">
                        <div class="${priceAlignmentClass} mb-1">
                            <p class="text-green-600 text-lg font-bold leading-tight">
                                ${minPrice > 0 ? `від <span class="underline">${minPrice.toFixed(0)}</span>` : 'Ціна не знайдена'}
                                ${showPriceRange ? ` до <span class="underline">${phone.maxPrice.toFixed(0)}</span>` : ''}
                                ${minPrice > 0 ? 'грн' : ''}
                            </p>
                        </div>
                        <p class="text-gray-500 text-xs mb-3 dark:text-gray-400 h-4 overflow-hidden text-left whitespace-nowrap text-ellipsis">
                            ${phone.storeName ? `Найнижча ціна у: <b>${phone.storeName}</b>` : '&nbsp;'}
                        </p>
                        
                        <div class="flex gap-2">
                            <button data-id="${phone.id}" class="favorite-remove-btn flex-1 px-3 py-2 rounded-full text-sm transition whitespace-nowrap flex items-center justify-center gap-1.5 font-medium bg-green-500 text-white hover:bg-green-600">
                                <img src="public/assets/icon-star-selected.png" alt="Star" class="w-4 h-4">
                                <span>В обраному</span>
                            </button>
                            <a href="${storeUrl}" target="_blank" class="flex-1 text-center bg-blue-500 text-white py-2 px-3 rounded-full hover:bg-blue-600 text-sm font-medium flex items-center justify-center ${minPrice === 0 ? 'opacity-50 pointer-events-none' : ''}">
                                ${minPrice > 0 ? 'Купити' : 'Н/Д'}
                            </a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="card-side back-side flex flex-col rounded-xl border-2 border-blue-50 dark:border-gray-600 overflow-hidden p-6 items-center justify-center text-center">
                <h3 class="text-lg font-bold text-gray-800 dark:text-white mb-6">Характеристики</h3>
                <ul class="text-sm text-gray-600 dark:text-gray-300 w-full space-y-3 text-left mb-auto">
                    ${specsHTML}
                </ul>
                <button class="flip-btn mt-auto bg-blue-500 text-white py-2 px-6 rounded-full hover:bg-blue-600 text-sm font-medium flex items-center justify-center gap-2 shadow-md transition-colors focus:outline-none" data-card-id="${phone.id}">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-4 h-4">
                      <path stroke-linecap="round" stroke-linejoin="round" d="M9 15L3 9m0 0l6-6M3 9h12a6 6 0 010 12h-3" />
                    </svg>
                    Назад до товару
                </button>
            </div>
        `;
        favoritesList.appendChild(card);
    });

    const toggleInfo = (cardId) => {
        const cardEl = document.getElementById(`card-${cardId}`);
        if(cardEl) cardEl.classList.toggle('show-info');
    };

    document.querySelectorAll('.flip-btn').forEach(btn => {
        btn.addEventListener('click', (e) => {
            e.stopPropagation();
            toggleInfo(e.currentTarget.dataset.cardId);
        });
    });

    document.querySelectorAll('.favorite-remove-btn').forEach(btn => {
        btn.addEventListener('click', async e => {
            const id = parseInt(e.currentTarget.dataset.id);
            const span = e.currentTarget.querySelector('span');
            const originalText = span ? span.textContent : '';
            if (span) span.textContent = '...';
            const res = await fetch(`${API_BASE_URL}/api/users/favorites/${id}`, {
                method: 'DELETE',
                headers: { 'Authorization': `Bearer ${token}` }
            });
            if (res.ok) fetchProfile();
            else {
                alert('Не вдалося видалити товар.');
                if (span) span.textContent = originalText;
            }
        });
    });

    document.querySelectorAll('.card-image-trigger').forEach(img => {
        img.addEventListener('click', (e) => {
            const id = parseInt(e.currentTarget.dataset.id);
            openImageModal(id);
        });
    });
}

async function fetchProfile() {
    if (!token) {
        alert('Ви не авторизовані. Повернення на головну сторінку.');
        window.location.href = 'index.html';
        return;
    }

    try {
        const profileResponse = await fetch(`${API_BASE_URL}/api/users/profile`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (profileResponse.status === 401) {
            alert('Сесія закінчилась. Увійдіть знову.');
            localStorage.removeItem('mobix_jwt_token');
            window.location.href = 'index.html';
            return;
        }
        if (!profileResponse.ok) throw new Error('Не вдалося отримати дані профілю.');
        const profile = await profileResponse.json();

        const phonesResponse = await fetch(`${API_BASE_URL}/api/smartphones`);
        let allPhones = [];
        if (phonesResponse.ok) {
            allPhones = await phonesResponse.json();
        }

        enrichedFavorites = profile.favorites.map(favItem => {
            const detailedItem = allPhones.find(p => p.id === favItem.id);

            return {
                ...favItem,
                minPrice: detailedItem ? detailedItem.minPrice : 0,
                maxPrice: detailedItem ? detailedItem.maxPrice : 0,
                storeName: detailedItem ? detailedItem.storeName : '',
                storeUrl: detailedItem ? detailedItem.storeUrl : '',
                imageUrl2: detailedItem ? detailedItem.imageUrl2 : null,
                imageUrl3: detailedItem ? detailedItem.imageUrl3 : null,
                ram: detailedItem ? detailedItem.ram : null,
                storage: detailedItem ? detailedItem.storage : null,
                displaySize: detailedItem ? detailedItem.displaySize : null,
                displayHz: detailedItem ? detailedItem.displayHz : null
            };
        });

        document.getElementById('userEmail').textContent = `Привіт, ${profile.email}!`;
        document.getElementById('userRole').textContent = profile.role;
        document.getElementById('userId').textContent = profile.id;
        const avatarImg = document.getElementById('userAvatar');
        avatarImg.src = profile.avatarUrl || 'https://placehold.co/100x100?text=👤';

        renderFavorites(enrichedFavorites);

        if (profileSkeleton) profileSkeleton.classList.add('hidden');
        contentDiv.classList.remove('hidden');

    } catch (err) {
        console.error(err);
    }
}

const userAvatarFn = document.getElementById('userAvatar');
if (userAvatarFn) {
    userAvatarFn.parentElement.addEventListener('click', async () => {
        const newAvatarUrl = prompt("Введіть посилання (URL) на нове зображення:", userAvatarFn.src);

        if (!newAvatarUrl || newAvatarUrl.trim() === "") return;

        if (!newAvatarUrl.startsWith('http')) {
            alert("Будь ласка, введіть коректне посилання (починається з http...)");
            return;
        }

        try {
            const response = await fetch(`${API_BASE_URL}/api/users/update`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({ newAvatarUrl: newAvatarUrl })
            });

            if (response.ok) {
                userAvatarFn.src = newAvatarUrl;
                alert("Аватар успішно оновлено!");
            } else {
                alert("Помилка оновлення аватара.");
            }
        } catch (error) {
            console.error(error);
            alert("Помилка з'єднання з сервером.");
        }
    });
}

fetchProfile();