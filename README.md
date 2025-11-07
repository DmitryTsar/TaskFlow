🚀 TaskFlow

TaskFlow — кроссплатформенное приложение для управления задачами.
Бэкенд — ASP.NET Core 8 (CQRS, EF Core, JWT).
Фронтенд — React + Vite.
База данных — MS SQL Server.

TaskFlow/
├─ TaskFlow.API/              # ASP.NET Core Web API
├─ TaskFlow.Application/      # CQRS, DTO, Handlers
├─ TaskFlow.Domain/           # Сущности и интерфейсы
├─ TaskFlow.Infrastructure/   # EF Core, DbContext, репозитории
├─ TaskFlow.Client/           # React + Vite фронтенд
├─ TaskFlow.Tests/            # Unit-тесты
├─ docker-compose.yml         # Контейнеризация всех сервисов
├─ .env.example               # Пример переменных окружения
├─ TaskFlow.sln
└─ README.md

⚙️ Требования

Docker + Docker Compose
Git (для клонирования репозитория)
(опционально) .NET SDK 8.0 и Node.js 20 для локальной разработки вне контейнеров

🐳 Запуск через Docker Compose

1️. Клонировать репозиторий
git clone https://github.com/yourusername/TaskFlow.git
cd TaskFlow
2.Создайте файл .env на основе примера:
	cp .env.example .env  # Linux / macOS
	copy .env.example .env # Windows PowerShell
3. Отредактируйте .env, указав свои значения:
	SA_PASSWORD=YourStrongPassword123!
	ASPNETCORE_ENVIRONMENT=Development
	ConnectionStrings__DefaultConnection=Server=sqlserver;Database=TaskFlowDb;User Id=sa;Password=YourStrongPasswordHere;TrustServerCertificate=True;
4. Собрать и запустить контейнеры
docker compose up --build
5. Проверить работу
Сервис	URL	Описание
🌐 Клиент (React)	http://localhost:5173
	Веб-интерфейс
⚙️ API (Swagger)	http://localhost:5000/swagger
	Документация API
🐘 SQL Server	localhost:1433	Внутренний контейнер
6. Остановить контейнеры
	docker compose down
7. Просмотр логов
	docker compose logs -f api
	docker compose logs -f client
	docker compose logs -f sqlserver

🧩 Автоматическая миграция базы данных
При старте контейнера TaskFlow.API выполняется:

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TaskFlowDbContext>();
    db.Database.Migrate();
}

🧱 Разработка vs Продакшн
  Среда	              Конфигурация	                     База данных	        JWT ключ	              URL
Development	   ASPNETCORE_ENVIRONMENT=Development	LocalDB / Docker SQL	DevSuperSecretKey...	https://localhost:7295

Production	   ASPNETCORE_ENVIRONMENT=Production	    Docker SQL	        ProdSuperSecretKey...

🧰 Полезные команды:
	Команда	Описание
	docker compose build	        Собрать образы без запуска
	docker compose up -d	        Запуск в фоне
	docker compose down -v	        Удалить контейнеры и volume базы
	docker compose ps	            Проверить состояние
	docker exec -it 
	taskflow-sql 
	/opt/mssql-tools/bin
	/sqlcmd -S localhost -U 
	sa -P "Your_strong!Passw0rd"	Подключиться к SQL Server в контейнере
	dotnet ef migrations add Init	Добавить миграцию вручную
	dotnet ef database update	    Применить миграции вручную

🧾 Переменные окружения(.env.example):
	Переменная	                            Описание                    Пример
	ConnectionStrings__DefaultConnection	Строка подключения к MSSQL	Server=sqlserver;Database=TaskFlowDb;User Id=sa;Password=${SA_PASSWORD};TrustServerCertificate=True;
	ASPNETCORE_ENVIRONMENT	                Окружение	                Development / Production
	SA_PASSWORD	                            ${SA_PASSWORD}

✅ Готово
	После запуска:
		API автоматически применяет EF Core миграции;
		клиент доступен по адресу http://localhost:5173;
		Swagger доступен на http://localhost:5000/swagger.