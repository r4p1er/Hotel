namespace Identity.Domain.Utils;

/// <summary>
/// Утилита для хэширования паролей
/// </summary>
public static class PasswordHasher
{
    /// <summary>
    /// Получить хэш пароля, алгоритм - blowfish
    /// </summary>
    /// <param name="password">Пароль</param>
    /// <returns>Хэш пароля</returns>
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    /// <summary>
    /// Проверить совпадает ли пароль с хэшем
    /// </summary>
    /// <param name="password">Пароль</param>
    /// <param name="hash">Хэш пароля</param>
    /// <returns>True, если совпадает, False - если нет</returns>
    public static bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}