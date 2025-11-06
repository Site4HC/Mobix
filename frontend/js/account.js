const token = localStorage.getItem('mobix_jwt_token');
const loadingDiv = document.getElementById('loading');
const contentDiv = document.getElementById('content');
const favoritesList = document.getElementById('favoritesList');
const logoutBtn = document.getElementById('logoutBtn');

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

} else {
    console.error("Елементи перемикача теми не знайдено.");
}


logoutBtn.addEventListener('click', () => {
 localStorage.removeItem('mobix_jwt_token');
 window.location.href = 'index.html';
});

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
  const card = document.createElement('div');
  card.className =
   "bg-gray-50 rounded-xl shadow-sm hover:shadow-lg transition p-4 flex flex-col items-center text-center border border-gray-100 dark:bg-gray-700 dark:border-gray-600";
  card.innerHTML = `
   <div class="flex justify-center bg-white rounded-xl p-3 w-full dark:bg-gray-800">
    <img src="${phone.imageUrl || 'https://placehold.co/150x150'}"
      alt="${phone.name}" class="h-36 object-contain">
   </div>
   <h4 class="font-semibold text-gray-800 mt-3 text-sm sm:text-base dark:text-gray-100">${phone.name}</h4>
   <p class="text-xs sm:text-sm text-gray-500 mb-3 dark:text-gray-400">${phone.manufacturer || ''}</p>
   <button data-id="${phone.id}"
      class="remove-favorite w-full bg-red-500 text-white py-2 rounded-full hover:bg-red-600 transition text-sm">
    Видалити з вибраного
   </button>
  `;
  favoritesList.appendChild(card);
 });

 document.querySelectorAll('.remove-favorite').forEach(btn => {
  btn.addEventListener('click', async e => {
   const id = parseInt(e.currentTarget.dataset.id);
   const res = await fetch(`http://localhost:5152/api/users/favorites/${id}`, {
    method: 'DELETE',
    headers: { 'Authorization': `Bearer ${token}` }
   });
   if (res.ok) {
    fetchProfile();
   } else {
    alert('Не вдалося видалити товар.');
   }
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
  const response = await fetch('http://localhost:5152/api/users/profile', {
   headers: { 'Authorization': `Bearer ${token}` }
  });

  if (response.status === 401) {
   alert('Сесія закінчилась. Увійдіть знову.');
   localStorage.removeItem('mobix_jwt_token');
   window.location.href = 'index.html';
   return;
  }

  if (!response.ok) throw new Error('Не вдалося отримати дані.');

  const profile = await response.json();

  document.getElementById('userEmail').textContent = `Привіт, ${profile.email}!`;
  document.getElementById('userRole').textContent = profile.role;
  document.getElementById('userId').textContent = profile.id;

  renderFavorites(profile.favorites);

  loadingDiv.classList.add('hidden');
  contentDiv.classList.remove('hidden');

 } catch (err) {
  console.error(err);
  loadingDiv.textContent = 'Помилка: не вдалося з’єднатися з сервером.';
 }
}

fetchProfile();

// loadingDiv.classList.add('hidden');
// contentDiv.classList.remove('hidden');
// renderFavorites([]);