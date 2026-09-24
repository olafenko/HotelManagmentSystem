namespace HotelManageSys.API.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base("Niepoprawne dane logowania")
    {
        
    }
}