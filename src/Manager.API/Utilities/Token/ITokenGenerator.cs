namespace Manager.API.Utilities.Token
{

  public interface ITokenGenerator
  {
    string GenerateToken(string login);
  }

}