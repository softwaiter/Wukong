using System;
using System.Collections.Generic;
using System.Text;

namespace TestLibrary
{
    public class Chinese
    {
        private string mName;

        public Chinese()
        {
        }

        public Chinese(string name)
        {
            mName = name;
        }

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        public override string ToString() {
            return mName;
        }
    }
}
