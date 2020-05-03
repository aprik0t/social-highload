using System;

namespace SocialHighload.Exceptions
{
    public class UnknownPersonException: Exception
    {
        public UnknownPersonException(): base("Пользователь не найден в базе")
        {
            
        }
    }
}