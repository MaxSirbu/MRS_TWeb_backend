namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface ITokenActions
{
    /// <summary>
    /// Genereaza un JWT semnat cu HMAC-SHA256.
    /// </summary>
    /// <param name="userId">ID-ul utilizatorului (claim "sub")</param>
    /// <param name="fullName">Numele complet (claim "name")</param>
    /// <param name="role">Rolul utilizatorului: Guest / User / Admin (claim "role")</param>
    /// <returns>Token-ul JWT serializat ca string</returns>
    string GenerateToken(int userId, string fullName, string role);
}
