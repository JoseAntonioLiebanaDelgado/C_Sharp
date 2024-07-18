using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Cryptography;

namespace AccessControl
{
    public class User
    {
        public string UserName { get; set; }
        public string Password_PlainText { get; set; }
        public string Password_Hash { get; set; }
        public string Password_SaltedHash { get; set; }
        public string Password_SaltedHashSlow { get; set; }
        public string Salt { get; set; }


        public User (string _UserName, string _Password)
        {
            UserName = _UserName;
            Password_PlainText = _Password;
        }

        public User ()
        {

        }

        public void AddUser()
        {   
            //Apliquem hash
            //this.Password_Hash=

            //Apliquem hash+salt
            //this.Salt=
            //this.Password_SaltedHash=

            //Apliquem hash+salt amb algorisme de hash lent.
            //this.Password_SaltedHashSlow=

            ((App)Application.Current).Database.Add(this);            
        }

        public bool Validate (string _UserName, string _Password)
        {
            User MyUser = ((App)Application.Current).Database.Find(User => User.UserName == _UserName);
            
            //Validate amb Text pla
            if (!ReferenceEquals(MyUser, null))
            {
                if (MyUser.Password_PlainText.Equals(_Password))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }

            //Validate amb Hash (comenta l'anterior validació)


            //Validate amb Hash i salt (comenta l'anterior validació)


            //Validate amb Hash slow i salt. Pots utilitzar la classe Rfc2898DeriveBytes
        }

        string BytesToStringHex (byte[] result)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in result)
                stringBuilder.AppendFormat("{0:x2}", b);

            return stringBuilder.ToString();
        }
    }

}
