using System;

namespace SocialHighload.Exceptions
{
    public class UnknownUserException: Exception
    {
        public UnknownUserException(): base("Не удалось найти вас в Базе Данных")
        {
            
        }
    }
}