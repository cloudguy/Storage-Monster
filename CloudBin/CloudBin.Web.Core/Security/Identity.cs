using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Principal;
using CloudBin.Core.Domain;

namespace CloudBin.Web.Core.Security
{
    [Serializable]
    public class Identity : MarshalByRefObject, IIdentity, ISerializable
    {
        private readonly bool _isAuthenticated;
        private string _name;
        private readonly int _userId;
        private int _timeZone;
        private string _locale;

        public Identity(User user, bool isAuthenticated)
        {
            _isAuthenticated = isAuthenticated && user != null;
            _name = user == null ? string.Empty : user.Name;
            _userId = user == null ? 0 : user.Id;
            _locale = user == null ? null : user.Locale;
            _timeZone = user == null ? 0 : user.TimeZone;
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
      
        public int UserId
        {
            get { return _userId; }
        }

        public string Locale
        {
            get { return _locale; }
            set { _locale = value; }
        }

        public int TimeZone
        {
            get { return _timeZone; }
            set { _timeZone = value; }
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
