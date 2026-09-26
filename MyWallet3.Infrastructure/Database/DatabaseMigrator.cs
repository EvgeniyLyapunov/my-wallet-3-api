using DbUp;
using System.Reflection;

namespace MyWallet3.Infrastructure.Database
{
    public static class DatabaseMigrator
    {
        public static void Migrate(string connectionString)
        {
            // Настраиваем DbUp
            var upgrader = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                // Говорим ему искать скрипты прямо в этой сборке (где лежит этот класс)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            // Запускаем миграцию
            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                // Если ошибка в SQL — сервер не запустится и покажет ошибку
                throw new Exception("Ошибка при выполнении миграции БД!", result.Error);
            }
        }
    }
}
