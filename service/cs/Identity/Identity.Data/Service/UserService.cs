using System;
using Identity.Domain.Interface;

namespace Identity.Data.Service
{
	public class UserService : IDataService
	{
		public UserService()
		{
		}

        public User[] GetAll<User>()
        {
            throw new NotImplementedException();
        }

        public User Get<User>(string id)
        {
            throw new NotImplementedException();
        }

        public void Save<User>(User item)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

    }
}

