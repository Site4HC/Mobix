const token = localStorage.getItem('mobix_jwt_token');
const profileSkeleton = document.getElementById('profileSkeleton');
const contentDiv = document.getElementById('content');
const favoritesList = document.getElementById('favoritesList');
const logoutBtn = document.getElementById('logoutBtn');

const imageModalOverlay = document.getElementById('imageModalOverlay');
const modalImage = document.getElementById('modalImage');
const modalCloseBtn = document.getElementById('modalCloseBtn');

const API_BASE_URL = 'https://mobix.onrender.com';

const themeToggleBtn = document.getElementById('themeToggleBtn');
const themeToggleSpan = document.getElementById('themeToggleSpan');
const htmlElement = document.documentElement;
const bodyElement = document.body;

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

function openImageModal(src) {
    if (!imageModalOverlay) return;
    modalImage.src = src;
    imageModalOverlay.classList.remove('hidden');
    imageModalOverlay.classList.add('flex');
}

function closeImageModal() {
    if (!imageModalOverlay) return;
    imageModalOverlay.classList.add('hidden');
    imageModalOverlay.classList.remove('flex');
    modalImage.src = "";
}

if (modalCloseBtn) {
    modalCloseBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        closeImageModal();
    });
}
if (modalImage) {
    modalImage.addEventListener('click', (e) => {
        e.stopPropagation();
        closeImageModal();
    });
}
if (imageModalOverlay) {
    imageModalOverlay.addEventListener('click', (e) => {
        if (e.target === imageModalOverlay) {
            closeImageModal();
        }
    });
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

        const card = document.createElement('div');
        card.className = "bg-white rounded-xl shadow-md overflow-hidden flex flex-col transition hover:shadow-xl duration-300 dark:bg-gray-800";

        card.innerHTML = `
            <div class="flex justify-center bg-gray-50 p-3 dark:bg-gray-700">
                <img class="h-44 object-contain cursor-pointer card-image-trigger" src="${phone.imageUrl || 'https://placehold.co/300x200'}" alt="${phone.name}">
            </div>
            <div class="p-4 flex flex-col justify-between flex-1">
                <div>
                    <h2 class="text-lg font-semibold text-gray-800 dark:text-gray-100">${phone.name}</h2>
                    <p class="text-gray-500 text-sm dark:text-gray-400">${phone.manufacturer || ''}</p>
                    
                    <p class="mt-2 text-green-600 text-lg font-bold">
    ${minPrice > 0
                ? `від <span class="underline">${minPrice.toFixed(0)}</span>`
                : 'Ціна не знайдена'}
        
    ${phone.maxPrice && phone.maxPrice > minPrice
                ? ` до <span class="underline">${phone.maxPrice.toFixed(0)}</span>`
                : ''}
        
    ${minPrice > 0 ? 'грн' : ''}
</p>
                    
                    <p class="text-gray-500 text-sm mt-1 dark:text-gray-400">
                        ${phone.storeName ? `Найнижча ціна у: <b>${phone.storeName}</b>` : ''}
                    </p>
                </div>
                
                <div class="flex mt-3 gap-2">
                    <!-- Кнопка без анімації наведення тексту, завжди "В обраному" -->
                    <button 
                        data-id="${phone.id}" 
                        class="favorite-remove-btn flex-1 px-3 py-2 rounded-full text-sm transition whitespace-nowrap flex items-center justify-center gap-1.5 font-medium bg-green-500 text-white hover:bg-green-600">
                        <img src="public/assets/icon-star-selected.png" alt="Star" class="w-4 h-4">
                        <span>В обраному</span>
                    </button>
                    
                    <a href="${storeUrl}" target="_blank"
                        class="flex-1 text-center bg-blue-500 text-white py-2 px-3 rounded-full hover:bg-blue-600 text-sm font-medium
                        ${minPrice === 0 ? 'opacity-50 pointer-events-none' : ''}">
                        ${minPrice > 0 ? 'Купити' : 'Н/Д'}
                    </a>
                </div>
            </div>
        `;
        favoritesList.appendChild(card);
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

            if (res.ok) {
                fetchProfile();
            } else {
                alert('Не вдалося видалити товар.');
                if (span) span.textContent = originalText;
            }
        });
    });

    document.querySelectorAll('.card-image-trigger').forEach(img => {
        img.addEventListener('click', (e) => {
            openImageModal(e.currentTarget.src);
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

        const enrichedFavorites = profile.favorites.map(favItem => {
            const detailedItem = allPhones.find(p => p.id === favItem.id);

            return {
                ...favItem,
                minPrice: detailedItem ? detailedItem.minPrice : 0,

                maxPrice: detailedItem ? detailedItem.maxPrice : 0,

                storeName: detailedItem ? detailedItem.storeName : '',
                storeUrl: detailedItem ? detailedItem.storeUrl : ''
            };
        });

        document.getElementById('userEmail').textContent = `Привіт, ${profile.email}!`;
        document.getElementById('userRole').textContent = profile.role;
        document.getElementById('userId').textContent = profile.id;

        renderFavorites(enrichedFavorites);

        if (profileSkeleton) profileSkeleton.classList.add('hidden');
        contentDiv.classList.remove('hidden');

    } catch (err) {
        console.error(err);
    }
}

fetchProfile();