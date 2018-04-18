using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace BookMicroBeta
{
    public class UserClass
    {
        private string _userID;

        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        private string _userFullName;

        public string UserFullName
        {
            get { return _userFullName; }
            set { _userFullName = value; }
        }
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        private string _userType;

        public string UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }
        private string _userEmail;

        public string UserEmail
        {
            get { return _userEmail; }
            set { _userEmail = value; }
        }
    }
}
