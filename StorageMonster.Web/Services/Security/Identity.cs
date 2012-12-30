using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Principal;
using StorageMonster.Domain;


namespace StorageMonster.Web.Services.Security
{
    [Serializable]
    public class Identity : MarshalByRefObject, IIdentity, ISerializable
    {
        private readonly bool _isAuthenticated;
        private string _name;
        private readonly string _email;
        private readonly int _userId;
		private String _locale;

        public Identity(User user, bool isAuthenticated)
        {
            _isAuthenticated = isAuthenticated && user != null;
            _name = user == null ? string.Empty : user.Name;
            _email = user == null ? string.Empty : user.Email;
            _userId = user == null ? 0 : user.Id;
			_locale = user == null ? null : user.Locale;
        }       

        public string AuthenticationType
        {
            get { return string.Empty; }
        }

        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Email
        {
            get { return _email; }
        }
        public int UserId
        {
            get { return _userId; }
        }

		public string Locale
		{
			get { return _locale; }
            set { _locale = value; }
		}

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (context.State == StreamingContextStates.CrossAppDomain)
            {
                GenericIdentity gIdent = new GenericIdentity(Name, AuthenticationType);
                info.SetType(gIdent.GetType());

                MemberInfo[] serializableMembers = FormatterServices.GetSerializableMembers(gIdent.GetType());
                object[] serializableValues = FormatterServices.GetObjectData(gIdent, serializableMembers);

                for (int i = 0; i < serializableMembers.Length; i++)
                {
                    info.AddValue(serializableMembers[i].Name, serializableValues[i]);
                }                
            }
            else
            {
                throw new InvalidOperationException("Serialization not supported");
            }
        }
    }
}
