--- README.md ---
# TaskFlow.Web (Vite + React + TypeScript)


## Запуск
1. npm install
2. npm run dev


По-умолчанию API базовый URL настроен на `http://localhost:5295/api`.


## Что реализовано
- Аутентификация (login/register) с сохранением токена в localStorage
- Axios с interceptor для передачи Authorization: Bearer <token>
- Сервисы для всех контроллеров, которые ты описал
- Страницы: Login, Register, TasksList, TaskDetails, ProjectsList, ProjectDetails, Profile
- ProtectedRoute для защиты маршрутов


--- END ---