using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.HelperModels
{
    public class Utilities
    {
        public static string RandomName()
        {
            // generating random password
            string allowedChars = "";

            allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";

            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";

            allowedChars += "1,2,3,4,5,6,7,8,9,0";

            char[] sep = { ',' };

            string[] arr = allowedChars.Split(sep);

            string passwordString = "";

            string temp = "";

            // generating a random text length for password

            Random randNum = new Random();
            int txtPassLength = randNum.Next(8, 32);

            Random rand = new Random();

            for (int i = 0; i < txtPassLength; i++)

            {
                temp = arr[rand.Next(0, arr.Length)];

                passwordString += temp;
            }


            return passwordString;
        }
    }
}
